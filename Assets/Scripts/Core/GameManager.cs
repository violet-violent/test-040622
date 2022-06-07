using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// main script that controls the game flow
/// </summary>

public class GameManager : MonoBehaviour
{
    static GameManager thisInstance;

    [Header("Main Objects")]
    public Camera OverviewCamera;
    public Transform MainContainer;
    public UIController UIControl;
    public GameObject PlayerPrefab;
    public Logger EventLogger;
    [Header("Settings")]
    public float MouseSensitivity = 1f;
    public float MaxPlayerHeadTilt = 80f;
    public float NPCWaypointReachDist = 0.1f;
    [Header("Game Levels")]
    public GameObject[] Levels;

    private int currentLevel;
    private GameLevel levelScript;
    private PlayerCharacter currentPlayer;

    // score is kept separately to allow player object removing
    private int playerScore;

    // singleton script - we need only one of this in the game
    public static GameManager Instance
    {
        get
        {
            if (!thisInstance)
            {
                thisInstance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (thisInstance == null)
                    Debug.LogError("No active GameManager script found in scene.");
            }

            return thisInstance;
        }
    }

    private void Cleanup()
    {
        for (int z = 0; z < MainContainer.childCount; z++)
            Destroy(MainContainer.GetChild(z).gameObject);
    }

    // switching between active player and overview camera
    private void SetPlayerActive (bool _playerActive)
    {
        OverviewCamera.enabled = !_playerActive;
        currentPlayer.EnablePlayer (_playerActive);
    }

    private void StartLevel(int _level)
    {
        Cleanup();

        // creating a level
        currentLevel = _level;
        GameObject levelObject = Instantiate (Levels[_level], MainContainer);
        levelScript = levelObject.GetComponent<GameLevel>();
        EventLogger.OnLevelStarted(currentLevel);

        // spawning a player, removing spawn placeholder
        GameObject player = Instantiate (PlayerPrefab, levelObject.transform);
        player.transform.SetPositionAndRotation (levelScript.PlayerSpawn.position, levelScript.PlayerSpawn.rotation);
        currentPlayer = player.GetComponent<PlayerCharacter>();
        Destroy (levelScript.PlayerSpawn.gameObject);

        // activating a player, shutting down overview camera
        SetPlayerActive (true);

        // ui
        UIControl.SetState(UIStates.Gameplay);
        UIControl.ShowScore(playerScore);
    }

    public void RecordKill(string _npcName)
    {
        EventLogger.OnEnemyKill(_npcName);
        playerScore++;
        UIControl.ShowScore(playerScore);
    }

    public void FinishLevel ()
    {
        UIControl.SetState(UIStates.LevelEnded);
        SetPlayerActive(false);
        EventLogger.OnLevelFinished(currentLevel);
    }

    public void SetNextLevel ()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, Levels.Length - 1);
        StartLevel(currentLevel);
    }

    public void EndGame()
    {
        UIControl.SetState(UIStates.GameOver);
        UIControl.ShowFinalScore(playerScore);

        // sometimes trigger is activated twice, avoiding log duplicates
        if (currentPlayer.IsPlayable)
            EventLogger.OnGameOver(playerScore);

        SetPlayerActive(false);
    }

    public void StartGame()
    {
        playerScore = 0;
        StartLevel (0);
    }

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        playerScore = 0;
        UIControl.SetState (UIStates.GameStart);
        EventLogger.OnGameStarted();
    }
}
