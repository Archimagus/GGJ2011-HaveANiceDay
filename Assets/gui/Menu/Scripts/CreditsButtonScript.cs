using UnityEngine;
using System.Collections;

public class CreditsButtonScript
    : MyButton {
    protected override void Click()
    {
            Camera.mainCamera.transform.position = new Vector3(0, 8, 15);
    }
}
