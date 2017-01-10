using UnityEngine;
using System.Collections;

public class RobotScript : SlideShowScript 
{

    public Texture defaultTexture;
    public Texture mouseOverTexture;
	
	// Update is called once per frame
	protected override void Update () 
    {
        if (textureIndex >= slideShowImages.Length)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100) &&
                hit.transform == transform)
            {
                slideShowMaterial.SetTexture("_MainTex", mouseOverTexture);
            }
            else
            {
                slideShowMaterial.SetTexture("_MainTex", defaultTexture);
            }
        }
        else
        {
            base.Update();
        }
	
	}
}
