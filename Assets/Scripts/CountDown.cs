using System.Collections;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public bool isGamePlay;
    public int countdownTime; // 倒计时的时间
    private TextMeshProUGUI countdownText;
    
    private void Start()
    {
        countdownText = GetComponent<TextMeshProUGUI>();

        if (isGamePlay)
        {
            countdownText.text = "Time remaining: " + countdownTime + " seconds";
            StartCoroutine(CountDownCoroutineInGamePlay());
        }
        else
        {
            countdownText.text = "Game will start in " + countdownTime + " seconds...";
            StartCoroutine(CountDownCoroutine());
        }
    }
    
    private IEnumerator CountDownCoroutine()
    {
        int time = countdownTime;
        while (time > 2)
        {
            yield return new WaitForSeconds(1f);
            time--;
            countdownText.text = "Game will start in " + time + " seconds...";
        }
        yield return new WaitForSeconds(1f);
        countdownText.text = "Game will start in 1 second...";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "Game Start!";
        GameStatusManager.Instance.StartGame();
    }
    
    private IEnumerator CountDownCoroutineInGamePlay()
    {
        int time = countdownTime;
        while (time > 2)
        {
            yield return new WaitForSeconds(1f);
            time--;
            countdownText.text = "Time remaining: " + time + " seconds";
        }
        yield return new WaitForSeconds(1f);
        countdownText.text = "Time remaining: 1 second";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "Game Over!";
        GameStatusManager.Instance.GameOver();
    }
}
