using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Image))]
public class QuestItemUI : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public Text amountText;
    public Image background;

    public void Initialize(Sprite iconSprite, string name, string amount, Color bgColor)
    {
        icon.sprite = iconSprite;
        nameText.text = name;
        amountText.text = amount;
        background.color = bgColor;
    }
}
