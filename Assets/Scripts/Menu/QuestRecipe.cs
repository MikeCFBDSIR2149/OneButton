
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

