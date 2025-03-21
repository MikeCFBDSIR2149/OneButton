using System.Collections;
using TMPro;
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
    
    public bool allowBarIndicator;

    private void Start()
    {
        GameStatusManager.Instance.Initialize();
        AudioManager.Instance.Initialize();
        if (GameStatusManager.Instance.CurrentStatus == Status.GameOver)
            StartCoroutine(BlockInput());
        knife = FindFirstObjectByType<Knife>();
        if (allowBarIndicator)
        {
            pressSlider.value = 0;
            fillImage = pressSlider.fillRect.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (blockInput) return;
        if (inPressCD) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressDuration = 0;
            if (fillImage) fillImage.color = Color.white;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            pressDuration += Time.deltaTime;
            if (!wasLongPress)
            {
                if (allowBarIndicator) pressSlider.value = Mathf.Clamp01(pressDuration / longPressThreshold);

                if (pressDuration > longPressThreshold)
                {
                    wasLongPress = true;
                    if (fillImage) fillImage.color = Color.yellow;
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
                if (fillImage)
                {
                    fillImage.color = Color.white;
                    pressSlider.value = 1;
                }
                OnShortPress();
            }

            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        inPressCD = true;
        yield return new WaitForSeconds(pressCD);

        if (allowBarIndicator) pressSlider.value = 0;
        if (fillImage) fillImage.color = Color.white;
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
                FindFirstObjectByType<CountDown>()?.StartGameAnimation();
                break;
            case Status.GamePlay:
                knife.LongCut();
                AudioManager.Instance.PlaySFX(1);
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
                AudioManager.Instance.PlaySFX(0);
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
        yield return new WaitForSeconds(1f);
        Cite cite = FindFirstObjectByType<Cite>();
        if (cite)
        {
            cite.gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        blockInput = false;
    }
}
