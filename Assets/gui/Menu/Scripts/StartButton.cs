using UnityEngine;
using System.Collections;

public class StartButton : MyButton 
{
	protected override void Click()
	{
		base.Click();
		if(!string.IsNullOrEmpty(PlayerPrefs.GetString("characterChoice")))
		{
			Application.LoadLevel("PlanetCracker");
		}
	}
}
