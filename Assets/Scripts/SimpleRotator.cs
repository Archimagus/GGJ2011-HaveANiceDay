using UnityEngine;
using System.Collections;

public class SimpleRotator: MonoBehaviour {

	public Vector3 axis = Vector3.up;
	public float speed = 30;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.zero, axis, speed*Time.deltaTime);
	}
}
