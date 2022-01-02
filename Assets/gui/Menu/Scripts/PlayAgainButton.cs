using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayAgainButton
    : MyButton {
    protected override void Click()
    {
        base.Click();
        SceneManager.LoadScene("PlanetCracker");
    }
}
