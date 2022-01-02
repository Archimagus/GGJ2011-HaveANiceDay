using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuButton : MyButton {

    protected override void Click()
    {
        base.Click();;
        SceneManager.LoadScene("MainMenu");
    }
}
