using UnityEngine;
using System.Collections;

public class BackButton
    : MyButton {

    protected override void Click()
    {
        base.Click();
        Camera.mainCamera.transform.position = new Vector3(0, 8, 0);
    }
}
