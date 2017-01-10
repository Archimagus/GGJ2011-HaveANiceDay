using UnityEngine;
using System.Collections;

public class MainMenuButton : MyButton {

    protected override void Click()
    {
        base.Click();
        Application.LoadLevel("MainMenu");
    }
}
