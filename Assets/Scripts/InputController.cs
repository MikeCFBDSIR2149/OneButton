using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public float longPressThreshold; // 长按的时间阈值（秒）
    private float pressDuration; // 记录按键按下的时间
    private bool isPressing; // 记录是否正在按住键
    private bool isLongPress;

    private KnifeController knifeController;
    
    public Slider pressSlider;
    
    private Image fillImage;

    private void Start()
    {
        knifeController = FindFirstObjectByType<KnifeController>();
        pressSlider.value = 0;
        fillImage = pressSlider.fillRect.GetComponent<Image>();
    }

    private void Update()
    {
        if (knifeController.isCD)
            return;
        
        // 检测按键按下
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressDuration = 0;
            pressSlider.value = 0;
            fillImage.color = Color.white;
            isPressing = true;
        }

        if (isPressing && Input.GetKey(KeyCode.Space))
        {
            pressDuration += Time.deltaTime;
            if (pressSlider)
                pressSlider.value = Mathf.Min(pressDuration / longPressThreshold, 1);
        }

        if (!isLongPress && Mathf.Approximately(pressSlider.value, 1f))
        {
            isLongPress = true;
            fillImage.color = Color.yellow;
        }

        // 检测按键松开
        if (!Input.GetKeyUp(KeyCode.Space))
            return;
        
        isPressing = false;

        if (isLongPress)
        {
            OnLongPress();
        }
        else
        {
            fillImage.color = Color.cyan;
            knifeController.MoveKnife();
        }
        
        isLongPress = false;
    }

    // 长按事件
    private void OnLongPress()
    {
        Debug.Log("长按空格键");
    }
}
