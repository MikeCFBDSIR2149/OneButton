using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainAnimation : MonoBehaviour
{
    public GameObject canvas;
    /// <summary>
    /// This method is called by Animation event
    /// </summary>
    public void StartGame()
    {
        GameStatusManager.Instance.StartGame();
    }

    public void CloseTextUI()
    {
        canvas.SetActive(false);
    }
}
