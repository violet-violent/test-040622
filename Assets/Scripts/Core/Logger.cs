using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour
{
    public string LogFileName;

    private void LogString (string _str)
    {
        StreamWriter logStream = new StreamWriter(LogFileName, true);
        logStream.WriteLine(System.DateTime.Now.ToString() + " : " + _str);
        logStream.Close();
    }

    public void OnGameStarted()
    {
        LogString("Game Started");
    }

    public void OnLevelStarted(int _levelNum)
    {
        LogString("Level " + _levelNum.ToString() + " Started");
    }

    public void OnLevelFinished(int _levelNum)
    {
        LogString("Level " + _levelNum.ToString() + " Finished");
    }

    public void OnGameOver(int _finalScore)
    {
        LogString("Game Over, Score: " + _finalScore.ToString());
    }

    public void OnShot (string _weaponName)
    {
        LogString("Player Shots From " + _weaponName);
    }

    public void OnEnemyHit (string _enemyName)
    {
        LogString("Player Hits " + _enemyName);
    }
}
