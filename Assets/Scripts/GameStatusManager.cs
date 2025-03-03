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

    private void Start()
    {
        CurrentStatus = SceneManager.GetActiveScene().buildIndex == 0 ? Status.MainMenu : Status.GamePlay;
    }

    public void StartGamePrep()
    {
        SwitchScene(1);
        CurrentStatus = Status.GamePrep;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // private IEnumerator CountDown()
    // {
    //     int time = 3;
    //     while (time > 0)
    //     {
    //         yield return new WaitForSeconds(1f);
    //         time--;
    //         text.text = time.ToString();
    //     }
    //     countdownCoroutine = null;
    //     text.gameObject.SetActive(false);
    //     CurrentStatus = Status.GamePlay;
    // }
}
