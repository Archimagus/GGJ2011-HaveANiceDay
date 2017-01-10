using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	public AudioClip explosionSound;
	public GameObject detonator;
    public ShipController shipController;
    public bool released = false;
	//public float Speed;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.position.magnitude > 40)
		{
            MyKongregateAPI.Instance.SubmitStat(MyKongregateAPI.shotsWasted, 1);
            Destroy(gameObject);
            shipController.MoonDead();
		}	
	}
	
	
	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.GetComponent<PlanetController>() != null ||
            collision.gameObject.GetComponent<SunController>() ||
            collision.gameObject.GetComponent<ProjectileController>())
		{
            var det = detonator.GetComponent<Detonator>();
			det.transform.position = transform.position;
            var audio = det.GetComponent<AudioSource>();
			audio.Play();
			det.Explode();		
			Destroy(gameObject);
            shipController.MoonDead();
		}
	}
}
