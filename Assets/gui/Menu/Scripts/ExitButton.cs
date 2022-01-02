using UnityEngine;
using System.Collections;

public class ExitButton : MyButton 
{
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Destroy(gameObject);
        }
    }
	protected override void Click()
	{
		Debug.Log("Quitting");
		Application.Quit();
	}
}
