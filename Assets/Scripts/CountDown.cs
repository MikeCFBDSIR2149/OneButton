using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private static readonly int Up = Animator.StringToHash("up");
    public bool isGamePlay;
    public int countdownTime; // 倒计时的时间
    private TextMeshProUGUI countdownText;
    public Animator animator;
    public Image timerImage;
    public int countDownType;
    
    private Coroutine coroutine;
    
    private void Start()
    {
        countdownText = GetComponent<TextMeshProUGUI>();

        if (isGamePlay)
        {
            // countdownText.text = "Time remaining: " + countdownTime + " seconds";
            timerImage.fillAmount = 1f;
            timerImage.color = Color.white;
            StartCoroutine(CountDownCoroutineInGamePlay());
        }
        else
        {
            countdownText.text = "Game will start in " + countdownTime + " seconds...";
            coroutine = StartCoroutine(CountDownCoroutine());
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
        yield return new WaitForSeconds(1f);
        StartGameAnimation();
    }
    
    private IEnumerator CountDownCoroutineInGamePlay()
    {
        int time = countdownTime;
        while (time > 2)
        {
            yield return new WaitForSeconds(1f);
            time--;
            // countdownText.text = "Time remaining: " + time + " seconds";
            timerImage.fillAmount = (float)time / countdownTime;
            if (time == 5)
                timerImage.color = new Color(0.9f, 0f, 0f, 0.9f);
        }
        yield return new WaitForSeconds(1f);
        // countdownText.text = "Time remaining: 1 second";
        timerImage.fillAmount = 1f / countdownTime;
        yield return new WaitForSeconds(1f);
        // timerImage.fillAmount = 1f;
        // countdownText.text = "Game Over!";
        GameStatusManager.Instance.GameOver();
    }

    public void StartGameAnimation()
    {
        StopCoroutine(coroutine);
        countdownText.text = "Game Start!";
        animator?.SetTrigger(Up);
    }
}
