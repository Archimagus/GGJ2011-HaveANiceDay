using UnityEngine;
using System.Collections;
using System.Timers;

public class PlanetController : MonoBehaviour {

    public string race;
	public SunController sun;
	public AudioClip scream;
	public GameObject detonator;
    public GameObject target;
	public float OrbitSpeed = 20;
	Rigidbody body;
	Vector3 velocityDampening;
	bool shouldScream = true;
    bool twoPlayer = true;
	// Use this for initialization
	void Start () {

        string numPlayers = PlayerPrefs.GetString("twoPlayer");
        if (!string.IsNullOrEmpty(numPlayers))
        {
            twoPlayer = (numPlayers == "two");
        }
        body = gameObject.GetComponent<Rigidbody>();
		body.velocity = Vector3.Cross(Vector3.up, transform.position).normalized * OrbitSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.forward = Vector3.Cross(Vector3.up, transform.position).normalized;
        transform.position += transform.forward * OrbitSpeed * Time.deltaTime;
        var position = transform.position;
        position.y = 0;
        transform.position = position;
        var velocity = Vector3.SmoothDamp(body.velocity,  Vector3.zero ,ref velocityDampening, 3);
        velocity.y = 0;
        body.velocity = velocity;
        
		
		if(transform.position.magnitude > 28)
		{
			body.velocity =  Vector3.Reflect(body.velocity, transform.position.normalized);
            transform.position = transform.position.normalized * 28;
		}
		if(transform.position.magnitude < 5 && shouldScream)
		{
			shouldScream = false;
            var det = detonator.GetComponent<Detonator>();
            var audio = det.GetComponent<AudioSource>();
			audio.PlayOneShot(scream);
		}
	}
    public void TriggerTargetRing()
    {
        if (twoPlayer)
        {
            var tgt = Instantiate(target, transform.position, Quaternion.identity) as GameObject;
            tgt.transform.parent = transform;
            Destroy(tgt, 4);
        }
    }
	public void Detonate()
	{
        var det = detonator.GetComponent<Detonator>();
		det.transform.position = transform.position;
        var audio = det.GetComponent<AudioSource>();
        audio.volume = 0.8f;
		audio.Play();
		det.Explode();
		Destroy (gameObject);
	}
	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.GetComponent<SunController>() != null)
		{
            MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.planetsDestroyed, 1);
			Detonate();
		}
	}
}
