using UnityEngine;
using System.Collections;

public class MyKongregateAPI : MonoBehaviour
{

    public static MyKongregateAPI Instance { get; private set; }

    public static string multiplayerGamesWonAsRoach = "multiplayerGamesWonAsRoach";
    public static string multiplayerGamesWonAsAlien = "multiplayerGamesWonAsAlien";
    public static string multiplayerGameTies = "multiplayerGameTies";
    public static string singleplayerGamesWonAsRoach = "singleplayerGamesWonAsRoach";
    public static string singleplayerGamesWonAsAlien = "singleplayerGamesWonAsAlien";
    public static string gamesWon = "gamesWon";
    public static string planetsDestroyed = "planetsDestroyed";
    public static string shotsWasted = "shotsWasted";
    public static string easterEggFound = "easterEggFound";
	
    bool isKongregate = false;
    int userId = 0;
    string username = "Guest";
    string gameAuthToken = "";


    void Start () 
    {
        Instance = this;
        // Begin the API loading process if it is available
        Application.ExternalEval(
          "if(typeof(kongregateUnitySupport) != 'undefined'){" +
          " kongregateUnitySupport.initAPI('MyKongregateAPI', 'OnKongregateAPILoaded');" +
          "};");
    }

    void Update() 
    {

    }

    void OnKongregateAPILoaded(string userInfoString) 
    {
        // We now know we're on Kongregate
        isKongregate = true;

        // Split the user info up into tokens
        var parameters = userInfoString.Split('|');
        userId = int.Parse(parameters[0]);
        username = parameters[1];
        gameAuthToken = parameters[2];


        // Register a sign in handler to let us know if the user signs in to Kongregate. Notice how we are using the 
        // Javascript API along with Application.ExternalEval, and then calling back into our app using SendMessage.
        // We deliver the new user information as a simple pipe-delimited string, which we can easily parse using String.Split. 
        Application.ExternalEval(
        "kongregate.services.addEventListener('login', function(){" +
        "   var services = kongregate.services;" +
        "   var params=[services.getUserId(), services.getUsername(), services.getGameAuthToken()].join('|');" +
        "   kongregateUnitySupport.getUnityObject().SendMessage('MyKongregateAPI', 'OnKongregateUserSignedIn', params);" +
        "});");
    }

    // Called when the Kongregate user signs in, parse the tokenized user-info string that we
    // generate below using Javascript.
    void OnKongregateUserSignedIn(string userInfoString) 
    {
        var parameters = userInfoString.Split('|');
        userId = int.Parse(parameters[0]);
        username = parameters[1];
        gameAuthToken = parameters[2];

        Debug.Log(userId);
        Debug.Log(username);
        Debug.Log(gameAuthToken);
    }

    public void SubmitStat(string statName, int value) 
    {
        if (isKongregate) 
        {
            Application.ExternalCall("kongregate.stats.submit", statName, value);
        }
    }

}
