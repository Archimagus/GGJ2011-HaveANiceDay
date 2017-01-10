using UnityEngine;
using System.Collections;

public class MyButton : MonoBehaviour {

	public string preference;
	public string preferenceValue;
	public Material mat;
	public Texture def;
	public Texture mouseOver;
	public Texture click;
    bool clickedLastFrame = false;
	// Use this for initialization
	void Start () 
	{
		mat.SetTexture("_MainTex", def);
	}
	
	// Update is called once per frame
	void Update () 
	{
        // this is to prevent buttons that dynamicly replace the clicked button 
        // from getting clicked in the same frame as this button.
        if (clickedLastFrame)
        {
            clickedLastFrame = false;
            Click();
        }
        else
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100) &&
                hit.transform == transform)
            {
                if (Input.GetButton("Fire1") && click != null)
                {
                    mat.SetTexture("_MainTex", click);
                }
                else if(mouseOver != null)
                {
                    mat.SetTexture("_MainTex", mouseOver);
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    clickedLastFrame = true;
                }
            }
            else
            {
                if (def != null)
                {
                    mat.SetTexture("_MainTex", def);
                }
            }
        }
	}
	protected virtual void Click()
	{
		if(!string.IsNullOrEmpty(preference))
		{
			PlayerPrefs.SetString(preference, preferenceValue);
		}
	}
}
