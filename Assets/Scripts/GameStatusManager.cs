using System;
using System.Collections;
using TMPro;
using Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Status
{
    MainMenu,
    GamePrep,
    GamePlay,
    GameOver
}

public class GameStatusManager : MonoSingleton<GameStatusManager>
{
    public Status CurrentStatus { get; private set; }

    public void Initialize()
    {
        CurrentStatus = (Status)SceneManager.GetActiveScene().buildIndex;
        #if UNITY_EDITOR
            if (SceneManager.GetActiveScene().buildIndex > 3)
                CurrentStatus = Status.GamePlay;
        #endif
    }

    public void BackToMainMenu()
    {
        SwitchScene(0);
        CurrentStatus = Status.MainMenu;
    }

    public void StartGamePrep()
    {
        SwitchScene(1);
        CurrentStatus = Status.GamePrep;
    }

    public void StartGame()
    {
        SwitchScene(2);
        CurrentStatus = Status.GamePlay;
    }

    public void GameOver()
    {
        SwitchScene(3);
        CurrentStatus = Status.GameOver;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private static void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
