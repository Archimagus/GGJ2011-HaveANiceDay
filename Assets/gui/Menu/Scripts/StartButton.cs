using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartButton : MyButton 
{
	protected override void Click()
	{
		base.Click();
		if(!string.IsNullOrEmpty(PlayerPrefs.GetString("characterChoice")))
		{
			SceneManager.LoadScene("PlanetCracker");
		}
	}
}
