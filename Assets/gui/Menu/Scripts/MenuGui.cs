using UnityEngine;
using System.Collections;

public class MenuGui : MonoBehaviour {

    public Texture greenCheck;
    public Texture redX;

    bool soundOn = true;
    public Rect SoundButtonRect { get; set; }
    //bool musicRunning = false;
	// Use this for initialization
	void Start () 
    {
        SoundButtonRect = new Rect(0, 0, 0, 0);
        gameObject.GetComponent<AudioSource>().Play(44100 * 29);
        var sound = PlayerPrefs.GetString("Sound", "On");
        soundOn = sound == "On";
        if (!soundOn)
        {
            SetSound();
        }
	}

    void OnGUI()
    {
        Debug.Log("On Gui");
        if (SoundButtonRect == new Rect(0, 0, 0, 0))
        {
            SoundButtonRect = new Rect(40, 0, 20, 20);
        }

        if (soundOn)
        {
            if (GUI.Button(SoundButtonRect, greenCheck))
            {
                PlayerPrefs.SetString("Sound", "Off");
                soundOn = false;
                SetSound();
            }
        }
        else
        {
            if (GUI.Button(SoundButtonRect, redX))
            {
                soundOn = true;
                PlayerPrefs.SetString("Sound", "On");
                SetSound();
            }
        }
        GUI.Label(new Rect(0, 0, 50, 20), "Sound");
    }

    private void SetSound()
    {
        var audioSources = FindObjectsOfTypeIncludingAssets(typeof(AudioSource)) as AudioSource[];
        foreach (var source in audioSources)
        {
            source.mute = !soundOn;
        }
    }
}
