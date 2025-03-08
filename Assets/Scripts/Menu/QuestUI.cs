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
    private Color successColor = new(0.4f, 1f, 0.4f, 1f);
    private Color failColor = new(1f, 0.4f, 0.4f, 1f);

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
            
            if (ui)
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
        UpdateColorOnly(); // 只更新颜色为红色
        StartCoroutine(ResetFailedState()); // 启动协程，等待两秒后恢复颜色并更新任务内容
    }

    private void UpdateColorOnly()
    {
        foreach (var item in _uiItems)
        {
            var ui = item.GetComponent<QuestItemUI>();
            if (ui)
            {
                ui.SetBackgroundColor(_isFailed ? failColor : Color.white); // 更新背景颜色
            }
        }
    }

    private IEnumerator ResetFailedState()
    {
        yield return new WaitForSeconds(2f); // 等待两秒
        _isFailed = false;
        UpdateColorOnly(); // 恢复颜色为白色
        questManager.GenerateNewQuest(); // 手动生成新任务
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

