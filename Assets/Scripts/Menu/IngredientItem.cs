using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    public IngredientType type;
    public SpriteRenderer iconRenderer;
    
    // 预制体初始化方法
    public void Initialize(IngredientType itemType, Sprite icon)
    {
        type = itemType;
        if(iconRenderer != null) iconRenderer.sprite = icon;
    }
}
