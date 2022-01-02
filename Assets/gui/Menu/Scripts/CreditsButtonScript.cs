using UnityEngine;
using System.Collections;

public class CreditsButtonScript
    : MyButton {
    protected override void Click()
    {
            Camera.main.transform.position = new Vector3(0, 8, 15);
    }
}
