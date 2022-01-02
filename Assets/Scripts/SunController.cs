using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Winner
{
    None,
    Pink,
    Blue,
    Tie
}
public class SunController : MonoBehaviour {

	public GameObject []planetChoices;
	public AudioClip []screams;
    public GameObject pinkPlayer;
    public GameObject bluePlayer;
	
	public GameObject sunFire;
	public GameObject planetDetonator;
	public GameObject detonator;
	public int MaxShots = 10;
	public int startingPlanets = 20;
	private int shots = 0;
	public List<GameObject> planets = new List<GameObject>();
    public GameObject gui;
    public Winner winner = Winner.None;
    public bool twoPlayer { get; set; }
    private string race;
    public string lastShotRace = "";
    GameObject ship;

    Vector3 cameraVelocity = Vector3.zero;
	// Use this for initialization
    internal void ChangePlayer(ShipController shipController)
    {
        CheckForWin();
            var pos = ship.transform.position;
            Destroy(ship);
            if (race == "blue")
                race = "pink";
            else
                race = "blue";

            pos = SetShip(pos);

            planets.ForEach(p => 
            {
                if (p != null)
                {
                    var pc = p.GetComponent<PlanetController>();
                    if (pc.race == race)
                    {
                        pc.TriggerTargetRing();
                    }
                }
            });
    }

    private Vector3 SetShip(Vector3 pos)
    {
        if (race == "pink")
        {
            ship = Instantiate(pinkPlayer, pos, Quaternion.identity) as GameObject;
        }
        else
        {
            ship = Instantiate(bluePlayer, pos, Quaternion.identity) as GameObject;
        }

        var sc = ship.GetComponent<ShipController>() as ShipController;
        sc.sunController = this;
        sc.race = race;
        var ui = (gui.GetComponent<Gui>());
        ui.Race = race;
        sc.Gui = ui;
        return pos;
    }

    public bool CheckForWin()
    {
        if (winner == Winner.None)
        {
            bool bluePlanetsLeft = false;
            bool pinkPlanetsLeft = false;
            foreach (var planet in planets)
            {
                if (planet != null)
                {
                    var pc = planet.GetComponent<PlanetController>();
                    if (pc.race == "blue")
                        bluePlanetsLeft = true;
                    else if (pc.race == "pink")
                        pinkPlanetsLeft = true;
                }
            }
            if (twoPlayer)
            {
                if (pinkPlanetsLeft && bluePlanetsLeft)
                    winner = Winner.None;
                else if (pinkPlanetsLeft)
                {
                    winner = Winner.Blue;
                    MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.multiplayerGamesWonAsRoach, 1);
                }
                else if (bluePlanetsLeft)
                {
                    winner = Winner.Pink;
                    MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.multiplayerGamesWonAsAlien, 1);
                }
                else
                {
                    winner = Winner.Tie;
                    MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.multiplayerGameTies, 1);
                }
            }
            else
            {
                if (pinkPlanetsLeft || bluePlanetsLeft)
                    winner = Winner.None;
                else if (race == "pink")
                {
                    winner = Winner.Pink;
                    MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.singleplayerGamesWonAsRoach, 1);
                }
                else
                {
                    winner = Winner.Blue;
                    MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.singleplayerGamesWonAsAlien, 1);
                }

            }
            if (winner != Winner.None)
            {
                var ui = (gui.GetComponent<Gui>());
                ui.showTurnMessage = false;
                MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.gamesWon, 1);
            }
        }
        return winner != Winner.None;
    }
	void Start ()
    {	
        race = PlayerPrefs.GetString("characterChoice");

        string numPlayers = PlayerPrefs.GetString("twoPlayer");
        if (!string.IsNullOrEmpty(numPlayers))
        {
            twoPlayer = (numPlayers == "two");
        }
        else
        {
            twoPlayer = true;
        }

        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30));
        mousePosition.y = 0;
        SetShip(mousePosition);
		for(int i=0; i < startingPlanets; i++)
		{
            int p = 0;
            if (twoPlayer)
            {
                p = (int)((i * planetChoices.Length / startingPlanets));
            }
            else if (race == "pink")
            {
                p = 0;
            }
            else
            {
                p = 1;
            }
			var s = Random.Range(1.5f, 3);
			var pos = Random.insideUnitCircle*20;
			pos += pos.normalized*5;
			var planet = Instantiate(planetChoices[p], pos, Quaternion.identity) as GameObject;
			planet.transform.localScale = new Vector3(s,s,s);
			planet.transform.position = new Vector3(pos.x, 0, pos.y);
			planet.name = "Planet"+i.ToString();
            var rb = planet.GetComponent<Rigidbody>();
			rb.useGravity = false;
			rb.mass = s/2.0f;
			rb.isKinematic = false;
			var pc = planet.GetComponent<PlanetController>();
			pc.OrbitSpeed = (1/pos.magnitude) * 10;
			pc.detonator = Instantiate(planetDetonator, planet.transform.position, Quaternion.identity) as GameObject;
			pc.scream = screams[(int)(Random.value * screams.Length)];
            pc.race = p == 0 ? "pink" : "blue";
            if (pc.race == race)
                pc.TriggerTargetRing();
			planets.Add(planet);
		}
	}
	float lastFire;
	// Update is called once per frame
	void Update () 
    {
		if(Time.timeSinceLevelLoad - lastFire > 0.25)
		{
            var det = sunFire.GetComponent<Detonator>();
			det.transform.position = transform.position;
            //var audio = det.GetComponent<AudioSource>() as AudioSource;
            //audio.Play();
			det.Explode();
			lastFire = Time.timeSinceLevelLoad;
		}
        if (winner != Winner.None)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, new Vector3(0, 40, 0), ref cameraVelocity, 1.0f);
            if (cameraVelocity.magnitude < 0.05f)
            {

                var ui = (gui.GetComponent<Gui>());
                ui.TriggerWinner(winner);
                Detonate();
            }
        }
        else
        {
            CheckForWin();
        }
	}
	
	public void Detonate()
	{
        detonator.GetComponent<Detonator>().Explode();
        var sound = detonator.GetComponent<AudioSource>();
        sound.Play();
        DestroyAllPlanets();
        Destroy(ship);
		Destroy(gameObject);
	}
	void OnCollisionEnter(Collision collision)
	{
        var pc = collision.gameObject.GetComponent<PlanetController>();
        if (pc != null)
        {
            var sc = ship.GetComponent<ShipController>() as ShipController;
            var ui = gui.GetComponent<Gui>();
            if (pc.race == lastShotRace)
            {
                if (sc.shotSuccess != ShotSuccess.BadHit)
                {
                    sc.shotSuccess = ShotSuccess.GoodHit;
                }
                if (pc.race == "pink")
                {
                    ui.PinkMood = Happieness.Happy;
                    ui.BlueMood = Happieness.Neutral;
                }
                else
                {
                    ui.BlueMood = Happieness.Happy;
                    ui.PinkMood = Happieness.Neutral;
                }
            }
            else
            {
                sc.shotSuccess = ShotSuccess.BadHit;
                if (pc.race == "pink")
                {
                    ui.BlueMood = Happieness.Angry;
                    ui.PinkMood = Happieness.Happy;
                }
                else
                {
                    ui.PinkMood = Happieness.Angry;
                    ui.BlueMood = Happieness.Happy;
                }
            }
        }
        if (collision.gameObject.GetComponent<ProjectileController>() != null)
		{
			shots++;
			if(shots >= MaxShots)
			{

                MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.easterEggFound, 1);
                winner = Winner.Tie;
			}
		}
	}

    private void DestroyAllPlanets()
    {
        foreach (var p in planets)
        {
            if (p != null)
            {
                p.GetComponent<PlanetController>().Detonate();
            }
        }
    }

}
