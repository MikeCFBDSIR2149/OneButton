using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestConfig
    {
        public int minIngredients = 2;
        public int maxIngredients = 5;
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
        return overDeliver;
    }

    public void GenerateNewQuest()
    {
        CurrentQuest = new List<QuestRecipe>();
        var types = new List<IngredientType>(_prefabMap.Keys);
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

[System.Serializable]
public class QuestRecipe
{
    public IngredientType type;
    public int requiredAmount;
    public int currentAmount;
}

public enum IngredientType
{
    Apple,
    Banana,
    Carrot,
    // 添加更多类型...
}
