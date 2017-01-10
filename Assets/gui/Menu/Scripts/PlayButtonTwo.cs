using UnityEngine;
using System.Collections;

public class PlayButtonTwo
    : MyButton {

    protected override void Click()
    {
        base.Click();
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("characterChoice")))
        {
            PlayerPrefs.SetString("howToPlay", "skip");
            Application.LoadLevel("PlanetCracker");
        }
        else
        {
            Camera.mainCamera.transform.position = new Vector3(0, 8, 0);
        }
    }
}
