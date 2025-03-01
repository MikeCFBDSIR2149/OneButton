using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    [Header("UI组件")]
    public Transform questItemParent;
    public GameObject questItemPrefab;
    public Color successColor = Color.green;
    public Color failColor = Color.red;

    [Header("引用")]
    public QuestManager questManager;

    private List<GameObject> _uiItems = new List<GameObject>();
    private bool _isFailed;

    private void Start()
    {
        questManager = FindFirstObjectByType<QuestManager>();
        questManager.OnQuestUpdated += UpdateUI;
        questManager.OnQuestFailed += OnQuestFailed;
        UpdateUI();
    }

    private void UpdateUI()
    {
        ClearUI();
        
        foreach (var recipe in questManager.CurrentQuest)
        {
            var item = Instantiate(questItemPrefab, questItemParent);
            var ui = item.GetComponent<QuestItemUI>();
            
            if (ui != null)
            {
                var prefab = questManager.GetIngredientPrefab(recipe.type);
                var icon = prefab.GetComponent<IngredientItem>().iconRenderer.sprite;

                ui.Initialize(
                    icon,
                    recipe.type.ToString(),
                    $"{recipe.currentAmount}/{recipe.requiredAmount}",
                    _isFailed ? failColor : 
                    recipe.currentAmount >= recipe.requiredAmount ? successColor : Color.white
                );
            }
            _uiItems.Add(item);
        }
    }

    private void OnQuestFailed()
    {
        _isFailed = true;
        UpdateUI();
        StartCoroutine(ResetFailedState());
    }

    private IEnumerator ResetFailedState()
    {
        yield return new WaitForSeconds(2f);
        _isFailed = false;
        UpdateUI();
    }

    private void ClearUI()
    {
        foreach (var item in _uiItems)
        {
            Destroy(item);
        }
        _uiItems.Clear();
    }
}

[RequireComponent(typeof(Image))]
public class QuestItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text amountText;
    public Image background;

    public void Initialize(Sprite iconSprite, string name, string amount, Color bgColor)
    {
        icon.sprite = iconSprite;
        nameText.text = name;
        amountText.text = amount;
        background.color = bgColor;
    }
}
/*UI设置：

创建Scroll View作为任务列表容器 制作任务项预制体并配置QuestItemUI 将预制体拖入QuestUI组件*/
