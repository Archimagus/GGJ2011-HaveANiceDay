using UnityEngine;
using System.Collections;
using System.Timers;
using UnityEngine.SceneManagement;

public enum Happieness
{
	Happy,
	Neutral,
	Angry
}
public class Gui : MonoBehaviour 
{
	public Texture roachHappy;
	public Texture roachNeutral;
	public Texture roachAngry;
	
	public Texture alienHappy;
	public Texture alienNeutral;
	public Texture alienAngry;

    public Texture roachWin;
    public Texture alienWin;
    public Texture tie;
    public Texture roachTurn;
    public Texture alienTurn;

    public Texture greenCheck;
    public Texture redX;
    public GameObject playAgainButton;
    public GameObject mainMenuButton;

    public Happieness PinkMood = Happieness.Neutral;
    public Happieness BlueMood = Happieness.Neutral;
	private string _race = "pink";
    public string Race
    {
        get { return _race; }
        set 
        {
            _race = value;
            if (showTurnMessage)
            {
                TriggerPlayerTurn(_race);
            }
        }
    }
    private Vector2 _smallSize;
    private Vector2 _bigSize;
    private float _edgeOffset = 10;
    private Winner _winner = Winner.None;
    public bool showTurnMessage = true;
    private string _playerTurn = null;
    private bool _soundOn = true;
    public Rect SoundButtonRect { get; set; }
	void Start () 
	{
        var sound = PlayerPrefs.GetString("Sound", "On");
        _soundOn = sound == "On";
        if (!_soundOn)
        {
            SetSound();
        }
        SoundButtonRect = new Rect(0, 0, 0, 0);
		Race = PlayerPrefs.GetString("characterChoice");
        float aspect = roachHappy.width / roachHappy.height;
        float maxHeight = Mathf.Min(roachHappy.height / 3, Screen.height / 6);
        _smallSize = new Vector2(maxHeight/aspect, maxHeight);
        maxHeight = Mathf.Min(roachHappy.height / 2, Screen.height / 4);
        _bigSize = new Vector2(maxHeight / aspect, maxHeight);

        if (Screen.width < 800)
        {
            _edgeOffset = 1;
        }
	}

	void OnGUI () 
	{
        if (SoundButtonRect == new Rect(0, 0, 0, 0))
        {
            SoundButtonRect = new Rect(40, 0, 20, 20);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        Texture tex;
        if (_winner == Winner.None)
        {
            switch (PinkMood)
            {
                case Happieness.Happy:
                    tex = alienHappy;
                    break;
                case Happieness.Neutral:
                    tex = alienNeutral;
                    break;
                case Happieness.Angry:
                    tex = alienAngry;
                    break;
                default:
                    tex = alienAngry;
                    break;
            }
            if (_race == "pink")
            {
                GUI.Label(new Rect(_edgeOffset, Screen.height - _bigSize.y - _edgeOffset, _bigSize.x, _bigSize.y), tex);
            }
            else
            {
                GUI.Label(new Rect(_edgeOffset, Screen.height - _smallSize.y - _edgeOffset, _smallSize.x, _smallSize.y), tex);
            }
            switch (BlueMood)
            {
                case Happieness.Happy:
                    tex = roachHappy;
                    break;
                case Happieness.Neutral:
                    tex = roachNeutral;
                    break;
                case Happieness.Angry:
                    tex = roachAngry;
                    break;
                default:
                    tex = roachAngry;
                    break;
            }
            if (_race == "blue")
            {
                GUI.Label(new Rect(Screen.width - _bigSize.x - _edgeOffset, Screen.height - _bigSize.y - _edgeOffset, _bigSize.x, _bigSize.y), tex);
            }
            else
            {
                GUI.Label(new Rect(Screen.width - _smallSize.x - _edgeOffset, Screen.height - _smallSize.y - _edgeOffset, _smallSize.x, _smallSize.y), tex);
            }
            if (_playerTurn != null)
            {
                if (_playerTurn == "pink")
                {
                    tex = alienTurn;
                }
                else if (_playerTurn == "blue")
                {
                    tex = roachTurn;
                }
                GUI.Label(new Rect(Screen.width / 2 - tex.width / 2, 20, tex.width, tex.height), tex);
            }
        }
        else
        {
            if (_winner == Winner.Blue)
            {
                tex = roachWin;
            }
            else if (_winner == Winner.Pink)
            {
                tex = alienWin;
            }
            else
            {
                tex = tie;
            }
            var aspect = tex.width / (float)tex.height;
            var height = (int)Mathf.Min(Screen.height * 0.85f, tex.height);
            var width = (int)(height * aspect);
            GUI.Label(new Rect(Screen.width / 2 - width / 2, -30, width, height), tex);
            playAgainButton.transform.position = Camera.main.transform.position + new Vector3(-4, -10, -5);
            mainMenuButton.transform.position = Camera.main.transform.position + new Vector3(4, -10, -5);
        }
        if(_soundOn)
        {
            if(GUI.Button(SoundButtonRect, greenCheck))
            {
                PlayerPrefs.SetString("Sound", "Off");
                _soundOn = false;
                SetSound();
            }
        }
        else
        {
            if(GUI.Button(SoundButtonRect, redX))
            {
                PlayerPrefs.SetString("Sound", "On");
                _soundOn = true;
                SetSound();
            }
        }
        GUI.Label(new Rect(0, 0, 50, 20), "Sound");
	}

    private void SetSound()
    {
        var audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (var source in audioSources)
        {
            source.mute = !_soundOn;
        }
    }

    private Winner _w;
    public void TriggerWinner(Winner w)
    {
        _w = w;

        Invoke(nameof(setWinner), 3);
        //Timer t = new Timer(3000);
        //t.AutoReset = false;
        //t.Elapsed += (_, e) => _winner = w;
        //t.Start();
    }
    private void setWinner()
	{
        _winner = _w;
	}
    private void TriggerPlayerTurn(string turn)
    {
        _playerTurn = turn;

        //Timer t = new Timer(2000);
        //t.AutoReset = false;
        //t.Elapsed += (_, e) => _playerTurn = null;
        //t.Start();
        Invoke(nameof(clearPlayerTurn), 2);
    }
    private void clearPlayerTurn()
	{
        _playerTurn = null;
	}


}
