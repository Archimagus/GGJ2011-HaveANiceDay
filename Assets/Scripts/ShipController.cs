using UnityEngine;

public enum ShotSuccess
{
    NoHit,
    GoodHit,
    BadHit
}
public class ShipController : MonoBehaviour {

    public SunController sunController;
	public GameObject ship3;
    public LineRenderer band;
    public GameObject bandAttachPoint;
	public GameObject projectile;
	public GameObject projectileDetonator;
	public AudioClip explosionSound;
    public AudioClip shotReleaseSound;

	public float shipDistance = 30;


    public string race { get; set; }
    public ShotSuccess shotSuccess { get; set; }
    public Gui Gui { get; set; }
	bool oldFireDown = false;
	bool recoverPosition = false;
	private Vector3 recoverPositionVelocity = Vector3.zero;
	private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 ship3Velocity = Vector3.zero;
    private Vector3 shipVelocity = Vector3.zero;
	private GameObject moon;
    private bool moonFlying;
    private Vector3 ship3Offset;
    bool twoPlayer = true;
    bool flyOff = false;
	// Use this for initialization
	void Start () 
    {
        shotSuccess = ShotSuccess.NoHit;
        ship3Offset = ship3.transform.localPosition;
        ship3Offset.z = -4;
		Update ();
		var newCameraPosition = transform.position *0.5f;
		newCameraPosition.y = 30;
        
		//Camera.main.transform.position = newCameraPosition;
        string numPlayers = PlayerPrefs.GetString("twoPlayer");
        if (!string.IsNullOrEmpty(numPlayers))
        {
            twoPlayer = (numPlayers == "two");
        }
	}
	// Update is called once per frame
	void Update () 
    {
        CalculatePlanetPriority();
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30));
		var fireDown = false;
        Vector2 mouseGuiPos = Input.mousePosition;
        mouseGuiPos.y = Screen.height - mouseGuiPos.y;
        band.SetPosition(1, band.transform.worldToLocalMatrix.MultiplyPoint(bandAttachPoint.transform.position));
        if (!Gui.SoundButtonRect.Contains(mouseGuiPos))
        {
            fireDown = Input.GetButton("Fire1");
        }
        if (sunController.winner == Winner.None)
        {
            mousePosition.y = 0;
            if (fireDown && !shotFired)
            {
                if (moon == null)
                {
                    moon = GetMoon();
                }
                var direction = transform.position - mousePosition;
                if (mousePosition.magnitude > transform.position.magnitude)
                {
                    transform.LookAt(transform.position + Vector3.Normalize(direction));
                }
                else
                {
                    transform.LookAt(transform.position + Vector3.Normalize(-direction));
                }
                ship3.transform.position = Vector3.SmoothDamp(ship3.transform.position, transform.position + -transform.forward * Mathf.Min(10, direction.magnitude), ref ship3Velocity, 0.1f);
                moon.transform.position = ship3.transform.position + transform.forward * 3;
            }
            else
            {
                var direction = Vector3.Normalize(mousePosition);
                if (oldFireDown == true && moon != null && !moonFlying)
                {
                    FireMoon(mousePosition);
                    recoverPosition = true;
                }
                if (recoverPosition)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, direction * shipDistance, ref recoverPositionVelocity, 0.1f);
                    transform.LookAt(Vector3.zero);
                    ship3.transform.position = transform.position + -transform.forward * 5;
                    if (Vector3.Angle(transform.position, direction) < 1)
                    {
                        recoverPosition = false;
                    }
                }
                else
                {
                    transform.position = direction * shipDistance;
                    transform.LookAt(Vector3.zero);
                    ship3.transform.localPosition = ship3Offset;
                }
            }
        }
        else
        {
            if (flyOff == false)
            {
                var newPos = new Vector3(0, 0, -1) * shipDistance;
                transform.forward = Vector3.forward;
                ship3.transform.localPosition = ship3Offset;
                transform.position = Vector3.SmoothDamp(transform.position, newPos, ref shipVelocity, 0.5f);
                if ((transform.position - newPos).magnitude < 0.1f)
                {
                    flyOff = true;
                }
            }
            else
            {
                var newPos = new Vector3(0, 0, 1) * shipDistance * 2;
                transform.forward = Vector3.forward;
                transform.position = Vector3.SmoothDamp(transform.position, newPos, ref shipVelocity, 1.5f);
            }
        }

        if (sunController.winner == Winner.None)
        {
            var newCameraPosition = transform.position * 0.5f;
		    newCameraPosition.y = 30;
            float smoothTime = 0.3f;
            if (moonFlying)
            {
                newCameraPosition = moon.transform.position;
                newCameraPosition.y = 15;
            }
            else if (shotFired)
            {
                newCameraPosition = averageMovement * 0.5f;
                newCameraPosition.y = 15;
                smoothTime = 1.0f;
            }
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, newCameraPosition, ref cameraVelocity, smoothTime);
            oldFireDown = fireDown;
        }
	}
    bool shotFired = false;
    Vector3 averageMovement;
    private void CalculatePlanetPriority()
    {
        if (shotFired)
        {
            var planetsMoving = false;
            averageMovement = Vector3.zero;
            int movingPlanets = 0;
            foreach (var p in sunController.planets)
            {
                if (p != null)
                {
                    var rb = p.GetComponent<Rigidbody>();
                    var velocity = rb.velocity.magnitude;
                    var velocityTowardSun = Vector3.Dot(rb.velocity, -p.transform.position.normalized);
                    if (velocityTowardSun > 0.2f || velocity > 0.5f)
                    {
                        planetsMoving = true;
                        averageMovement += p.transform.position;
                        movingPlanets++;
                    }
                }
            }
            
            if (planetsMoving)
            {
                averageMovement *= 1.0f / movingPlanets;
            }
            else
            {
                if (!moonFlying)
                {
                    shotFired = false;
                    //sunController.CheckForWin();
                    if (twoPlayer)
                    {
                        if (shotSuccess == ShotSuccess.GoodHit)
                        {
                            shotSuccess = ShotSuccess.NoHit;
                        }
                        else
                        {
                            sunController.ChangePlayer(this);
                        }
                    }
                }
            }
        }
    }

	void FireMoon(Vector3 mousePosition)
	{
		moon.transform.position = transform.position;
		moon.transform.forward = transform.forward;
        var rb = moon.GetComponent<Rigidbody>();
		rb.useGravity = false;
		var power = Mathf.Min(15, (transform.position - mousePosition).magnitude);
        power = Mathf.Max(power, 5);
		rb.velocity = transform.forward * power;
		rb.mass = 4;
		var pc = moon.AddComponent<ProjectileController>();
		pc.detonator = Instantiate(projectileDetonator, moon.transform.position, Quaternion.identity) as GameObject;
		pc.explosionSound = Instantiate(explosionSound, moon.transform.position, Quaternion.identity) as AudioClip;
        pc.shipController = this;
        pc.released = true;
        moonFlying = true;
        shotFired = true;
        sunController.lastShotRace = race;
        var sound = GetComponent<AudioSource>();
        sound.PlayOneShot(shotReleaseSound);
	}
    public void MoonDead()
    {
        moon = null;
        moonFlying = false;
    }
	GameObject GetMoon()
	{
		//var moon = GameObject.CreatePrimitive(PrimitiveType.Sphere) as GameObject;
		var moon = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		moon.name = "Projectile";
		return moon;	
	}
}