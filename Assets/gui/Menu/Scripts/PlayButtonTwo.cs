using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButtonTwo
    : MyButton {

    protected override void Click()
    {
        base.Click();
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("characterChoice")))
        {
            PlayerPrefs.SetString("howToPlay", "skip");
            SceneManager.LoadScene("PlanetCracker");
        }
        else
        {
            Camera.main.transform.position = new Vector3(0, 8, 0);
        }
    }
}
