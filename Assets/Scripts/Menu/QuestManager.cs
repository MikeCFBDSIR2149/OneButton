using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestConfig
    {
        public int minIngredients = 1;
        public int maxIngredients = 3;
        public int minAmount = 1;
        public int maxAmount = 5;
    }

    [Header("任务配置")]
    public QuestConfig config;
    
    [Header("食材预制体")]
    public List<GameObject> ingredientPrefabs;

    public List<QuestRecipe> CurrentQuest { get; private set; }
    public System.Action OnQuestUpdated;
    public System.Action OnQuestFailed;

    private Dictionary<IngredientType, GameObject> _prefabMap;
    // 新增事件：任务完成时触发
    public System.Action OnQuestCompleted;

    private void Awake()
    {
        InitializePrefabMap();
        GenerateNewQuest();
    }

    private void InitializePrefabMap()
    {
        _prefabMap = new Dictionary<IngredientType, GameObject>();
        foreach (var prefab in ingredientPrefabs)
        {
            var item = prefab.GetComponent<IngredientItem>();
            if (item != null)
            {
                _prefabMap[item.type] = prefab;
            }
        }
    }
    private bool CheckQuestComplete()
    {
        // 检查所有食材是否满足需求
        foreach (var recipe in CurrentQuest)
        {
            if (recipe.currentAmount < recipe.requiredAmount)
                return false;
        }
        return true;
    }

    public bool TryDeliverIngredient(IngredientType type)
    {
        bool overDeliver = false;
        
        foreach (var recipe in CurrentQuest)
        {
            if (recipe.type == type)
            {
                if (recipe.currentAmount >= recipe.requiredAmount)
                {
                    overDeliver = true;
                }
                else
                {
                    recipe.currentAmount++;
                }
                break;
            }
        }

        OnQuestUpdated?.Invoke();

        // 新增：任务完成时自动生成新任务
        if (CheckQuestComplete())
        {
            OnQuestCompleted?.Invoke();
            GenerateNewQuest(); // 生成新任务
        }

        return overDeliver;
    }
    public void GenerateNewQuest()
    {
        CurrentQuest = new List<QuestRecipe>();
        var types = new List<IngredientType>(_prefabMap.Keys);
        // 强制生成至少2种食材需求
        // int minCount = Mathf.Max(2, config.minIngredients);
        int count = Random.Range(config.minIngredients, config.maxIngredients + 1);

        for (int i = 0; i < count && types.Count > 0; i++)
        {
            int index = Random.Range(0, types.Count);
            var type = types[index];
            
            CurrentQuest.Add(new QuestRecipe
            {
                type = type,
                requiredAmount = Random.Range(config.minAmount, config.maxAmount + 1),
                currentAmount = 0
            });

            types.RemoveAt(index);
        }
        Debug.Log($"生成新任务，包含 {CurrentQuest.Count} 种食材需求");
        OnQuestUpdated?.Invoke();
    }

    public void ResetCurrentQuest()
    {
        foreach (var recipe in CurrentQuest)
        {
            recipe.currentAmount = 0;
        }
        OnQuestFailed?.Invoke();
        OnQuestUpdated?.Invoke();
    }

    public GameObject GetIngredientPrefab(IngredientType type)
    {
        return _prefabMap.ContainsKey(type) ? _prefabMap[type] : null;
    }
}

