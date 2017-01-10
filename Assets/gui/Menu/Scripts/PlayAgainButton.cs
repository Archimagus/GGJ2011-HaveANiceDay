using UnityEngine;
using System.Collections;

public class PlayAgainButton
    : MyButton {
    protected override void Click()
    {
        base.Click();
        Application.LoadLevel("PlanetCracker");
    }
}
