
using System.Collections.Generic;
using UnityEngine;
public class DeliveryZone : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask ingredientLayer;
    public float checkInterval = 0.1f;
    [Tooltip("任务失败后的禁止判定时间")]
    public float failCooldown = 0.5f;

    private QuestManager _questManager;
    private float _lastCheckTime;
    private float _lastFailTime;

    private void Start()
    {
        _questManager = FindFirstObjectByType<QuestManager>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time - _lastCheckTime < checkInterval)
        {
            return;
        }

        // 一次性收集所有食材
        var colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, ingredientLayer);
        var ingredients = new HashSet<IngredientItem>();
        foreach (var collider2d in colliders)
        {
            if (collider2d.TryGetComponent<IngredientItem>(out var ingredient))
            {
                ingredients.Add(ingredient);
            }
        }

        // 如果没有食材，直接返回
        if (ingredients.Count == 0)
        {
            _lastCheckTime = Time.time;
            return;
        }

        // 检查是否处于失败冷却期
        bool isInCooldown = Time.time - _lastFailTime < failCooldown;
        var originalQuest = _questManager.CurrentQuest;
        bool hasFailed = false;

        // 仅在非冷却期处理逻辑
        if (!isInCooldown)
        {
            foreach (var ingredient in ingredients)
            {
                if (hasFailed || _questManager.CurrentQuest != originalQuest)
                {
                    break;
                }

                HandleDelivery(ingredient.type);

                // 如果任务被重置，标记失败并记录时间
                if (_questManager.CurrentQuest != originalQuest)
                {
                    hasFailed = true;
                    _lastFailTime = Time.time;
                }
            }
        }

        // 强制销毁所有食材（无论是否处理过）
        foreach (var ingredient in ingredients)
        {
            Destroy(ingredient.gameObject);
        }

        _lastCheckTime = Time.time;
    }

    private void HandleDelivery(IngredientType type)
    {
        if (!_questManager.IsIngredientRequired(type))
        {
            _questManager.FailCurrentQuest();
            return;
        }

        bool overDeliver = _questManager.TryDeliverIngredient(type);
        if (overDeliver && !_questManager.CheckQuestComplete())
        {
            _questManager.FailCurrentQuest();
        }
    }
}
