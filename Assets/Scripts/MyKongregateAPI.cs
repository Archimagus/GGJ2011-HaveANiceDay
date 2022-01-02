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
	

    void Start()
    {
        Instance = this;
    }

    public void SubmitStat(string statName, int value) 
    {
    }

}
