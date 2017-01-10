using UnityEngine;
using System.Collections;
using System.Timers;

public enum Happieness
{
	Happy,
	Neutral,
	Angry
}
public class Gui : MonoBehaviour 
{
	public Texture roachHappy;
	public Texture roachNeutral;
	public Texture roachAngry;
	
	public Texture alienHappy;
	public Texture alienNeutral;
	public Texture alienAngry;

    public Texture roachWin;
    public Texture alienWin;
    public Texture tie;
    public Texture roachTurn;
    public Texture alienTurn;

    public Texture greenCheck;
    public Texture redX;
    public GameObject playAgainButton;
    public GameObject mainMenuButton;

    public Happieness PinkMood = Happieness.Neutral;
    public Happieness BlueMood = Happieness.Neutral;
	private string race = "pink";
    public string Race
    {
        get { return race; }
        set 
        {
            race = value;
            if (showTurnMessage)
            {
                TriggerPlayerTurn(race);
            }
        }
    }
    private Vector2 smallSize;
    private Vector2 bigSize;
    private float edgeOffset = 10;
    private Winner winner = Winner.None;
    public bool showTurnMessage = true;
    private string playerTurn = null;
    private bool soundOn = true;
    public Rect SoundButtonRect { get; set; }
	void Start () 
	{
        var sound = PlayerPrefs.GetString("Sound", "On");
        soundOn = sound == "On";
        if (!soundOn)
        {
            SetSound();
        }
        SoundButtonRect = new Rect(0, 0, 0, 0);
		Race = PlayerPrefs.GetString("characterChoice");
        float aspect = roachHappy.width / roachHappy.height;
        float maxHeight = Mathf.Min(roachHappy.height / 3, Screen.height / 6);
        smallSize = new Vector2(maxHeight/aspect, maxHeight);
        maxHeight = Mathf.Min(roachHappy.height / 2, Screen.height / 4);
        bigSize = new Vector2(maxHeight / aspect, maxHeight);

        if (Screen.width < 800)
        {
            edgeOffset = 1;
        }
	}

	void OnGUI () 
	{
        if (SoundButtonRect == new Rect(0, 0, 0, 0))
        {
            SoundButtonRect = new Rect(40, 0, 20, 20);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
        Texture tex;
        if (winner == Winner.None)
        {
            switch (PinkMood)
            {
                case Happieness.Happy:
                    tex = alienHappy;
                    break;
                case Happieness.Neutral:
                    tex = alienNeutral;
                    break;
                case Happieness.Angry:
                    tex = alienAngry;
                    break;
                default:
                    tex = alienAngry;
                    break;
            }
            if (race == "pink")
            {
                GUI.Label(new Rect(edgeOffset, Screen.height - bigSize.y - edgeOffset, bigSize.x, bigSize.y), tex);
            }
            else
            {
                GUI.Label(new Rect(edgeOffset, Screen.height - smallSize.y - edgeOffset, smallSize.x, smallSize.y), tex);
            }
            switch (BlueMood)
            {
                case Happieness.Happy:
                    tex = roachHappy;
                    break;
                case Happieness.Neutral:
                    tex = roachNeutral;
                    break;
                case Happieness.Angry:
                    tex = roachAngry;
                    break;
                default:
                    tex = roachAngry;
                    break;
            }
            if (race == "blue")
            {
                GUI.Label(new Rect(Screen.width - bigSize.x - edgeOffset, Screen.height - bigSize.y - edgeOffset, bigSize.x, bigSize.y), tex);
            }
            else
            {
                GUI.Label(new Rect(Screen.width - smallSize.x - edgeOffset, Screen.height - smallSize.y - edgeOffset, smallSize.x, smallSize.y), tex);
            }
            if (playerTurn != null)
            {
                if (playerTurn == "pink")
                {
                    tex = alienTurn;
                }
                else if (playerTurn == "blue")
                {
                    tex = roachTurn;
                }
                GUI.Label(new Rect(Screen.width / 2 - tex.width / 2, 20, tex.width, tex.height), tex);
            }
        }
        else
        {
            if (winner == Winner.Blue)
            {
                tex = roachWin;
            }
            else if (winner == Winner.Pink)
            {
                tex = alienWin;
            }
            else
            {
                tex = tie;
            }
            var aspect = tex.width / (float)tex.height;
            var height = (int)Mathf.Min(Screen.height * 0.85f, tex.height);
            var width = (int)(height * aspect);
            GUI.Label(new Rect(Screen.width / 2 - width / 2, -30, width, height), tex);
            playAgainButton.transform.position = Camera.main.transform.position + new Vector3(-4, -10, -5);
            mainMenuButton.transform.position = Camera.main.transform.position + new Vector3(4, -10, -5);
        }
        if(soundOn)
        {
            if(GUI.Button(SoundButtonRect, greenCheck))
            {
                PlayerPrefs.SetString("Sound", "Off");
                soundOn = false;
                SetSound();
            }
        }
        else
        {
            if(GUI.Button(SoundButtonRect, redX))
            {
                PlayerPrefs.SetString("Sound", "On");
                soundOn = true;
                SetSound();
            }
        }
        GUI.Label(new Rect(0, 0, 50, 20), "Sound");
	}

    private void SetSound()
    {
        var audioSources = FindObjectsOfTypeIncludingAssets(typeof(AudioSource)) as AudioSource[];
        foreach (var source in audioSources)
        {
            source.mute = !soundOn;
        }
    }

    public void TriggerWinner(Winner w)
    {
        Timer t = new Timer(3000);
        t.AutoReset = false;
        t.Elapsed += (_, e) => winner = w;
        t.Start();
    }

    private void TriggerPlayerTurn(string turn)
    {
        playerTurn = turn;
        Timer t = new Timer(2000);
        t.AutoReset = false;
        t.Elapsed += (_, e) => playerTurn = null;
        t.Start();
    }


}
