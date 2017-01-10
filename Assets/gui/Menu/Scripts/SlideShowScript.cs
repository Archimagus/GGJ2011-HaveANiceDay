using UnityEngine;
using System.Collections;

public class SlideShowScript : MonoBehaviour
{
    public Material slideShowMaterial;
    public Texture[] slideShowImages;
    /// <summary>
    /// The time when the respective texture finishes.
    /// </summary>
    public float[] textureChangeTimes;
    public bool destroyWhenFinished = true;

    protected int textureIndex;
	// Use this for initialization
	void Start () {
        textureIndex = 0;
        slideShowMaterial.SetTexture("_MainTex", slideShowImages[textureIndex]);
	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
        if (textureIndex < slideShowImages.Length)
        {
            if (Time.timeSinceLevelLoad > textureChangeTimes[textureIndex])
            {
                textureIndex++;
                if (textureIndex >= slideShowImages.Length)
                {
                    if (destroyWhenFinished)
                        Destroy(gameObject);
                }
                else
                {
                    slideShowMaterial.SetTexture("_MainTex", slideShowImages[textureIndex]);
                }
            }
        }
	}
}
