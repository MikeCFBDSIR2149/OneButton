using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public float longPressThreshold; // 长按的时间阈值（秒）
    public float pressCD;
    public Slider pressSlider;
    private Image fillImage;
    private bool inPressCD;

    private Knife knife;
    private float pressDuration; // 记录按键按下的时间
    private bool wasLongPress;
    
    public bool blockInput;

    private void Start()
    {
        GameStatusManager.Instance.Initialize();
        if (GameStatusManager.Instance.CurrentStatus == Status.GameOver)
            StartCoroutine(BlockInput());
        knife = FindFirstObjectByType<Knife>();
        pressSlider.value = 0;
        fillImage = pressSlider.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        if (blockInput) return;
        if (inPressCD) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressDuration = 0;
            fillImage.color = Color.white;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            pressDuration += Time.deltaTime;
            if (!wasLongPress)
            {
                pressSlider.value = Mathf.Clamp01(pressDuration / longPressThreshold);

                if (pressDuration > longPressThreshold)
                {
                    wasLongPress = true;
                    fillImage.color = Color.yellow;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            pressDuration = 0;
            if (wasLongPress)
            {
                OnLongPress();
            }
            else
            {
                fillImage.color = Color.cyan;
                OnShortPress();
            }

            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        inPressCD = true;
        yield return new WaitForSeconds(pressCD);

        pressSlider.value = 0;
        fillImage.color = Color.white;
        wasLongPress = false;
        inPressCD = false;
    }

    // 长按事件
    private void OnLongPress()
    {
        switch (GameStatusManager.Instance.CurrentStatus)
        {
            case Status.MainMenu:
                GameStatusManager.Instance.ExitGame();
                break;
            case Status.GamePrep:
                GameStatusManager.Instance.StartGame();
                break;
            case Status.GamePlay:
                // do check
                break;
            case Status.GameOver:
                GameStatusManager.Instance.BackToMainMenu();
                break;
        }
    }

    // 短按事件
    private void OnShortPress()
    {
        switch (GameStatusManager.Instance.CurrentStatus)
        {
            case Status.MainMenu:
                GameStatusManager.Instance.StartGamePrep();
                break;
            case Status.GamePlay:
                knife.Cut();
                break;
            case Status.GameOver:
                GameStatusManager.Instance.StartGame();
                break;
            case Status.GamePrep:
            default:
                break;
        }
    }

    private IEnumerator BlockInput()
    {
        blockInput = true;
        yield return new WaitForSeconds(3f);
        blockInput = false;
    }
}
