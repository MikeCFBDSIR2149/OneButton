using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Image))]
public class QuestItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;
    public Image background;

    public void Initialize(Sprite iconSprite, string name, string amount, Color bgColor)
    {
        icon.sprite = iconSprite;
        nameText.text = name;
        amountText.text = amount;
        background.color = bgColor;
    }
    public void SetBackgroundColor(Color color)
    {
        // 假设任务项的背景是一个 Image 组件
        background.color = color;
    }
}
