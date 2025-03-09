
[System.Serializable]
public class QuestRecipe
{
    public IngredientType type;
    public int requiredAmount;
    public int currentAmount;
}

public enum IngredientType
{
    Watermelon,
    Apple,
    Strawberry,
    Mango,
    DragonFruit,
    KiwiFruit,
    // 添加更多类型...
}

