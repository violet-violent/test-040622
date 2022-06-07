using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIStates
{
    None = 0,
    GameStart = 1,
    Gameplay = 2,
    GameOver = 3,
    LevelEnded = 4
}

[System.Serializable]
public class UIScreen
{
    public UIStates State;
    public GameObject ScreenObject;
}

public class UIController : MonoBehaviour
{
    public UIScreen[] Screens;
    [Header ("Status Texts")]
    public Text KillCounter;
    public Text FinalScoreText;
    public UIStates CurrentState { get; private set; }

    public void SetState (UIStates _state)
    {
        foreach (UIScreen ui in Screens)
            ui.ScreenObject.SetActive (ui.State == _state);

        CurrentState = _state;
    }

    public void ShowScore (int _score)
    {
        if (CurrentState != UIStates.Gameplay)
            return;

        KillCounter.text = _score.ToString();
    }

    public void ShowFinalScore (int _score)
    {
        if (CurrentState != UIStates.GameOver)
            return;

        FinalScoreText.text = _score.ToString();
    }
}
