using UnityEngine;
using System.Collections;

public class PlayButton : MyButton 
{
	protected override void Click()
	{
        base.Click();

        Camera.mainCamera.transform.position = new Vector3(25, 8, 15);

        //if (string.IsNullOrEmpty(PlayerPrefs.GetString("howToPlay")) &&
        //    !string.IsNullOrEmpty(PlayerPrefs.GetString("characterChoice")))
        //{
        //    Camera.mainCamera.transform.position = new Vector3(25, 8, 15);
        //}
        //else if(!string.IsNullOrEmpty(PlayerPrefs.GetString("characterChoice")))
        //{
        //    Application.LoadLevel("PlanetCracker");
        //}
	}
}
