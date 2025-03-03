using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [Header("设置")]
    public LayerMask ingredientLayer;
    public float cooldown = 0.5f;

    private QuestManager _questManager;
    private float _lastDelivery;

    private void Start()
    {
        _questManager = FindFirstObjectByType<QuestManager>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time - _lastDelivery < cooldown) return;

        if (((1 << other.gameObject.layer) & ingredientLayer) != 0)
        {
            if (other.TryGetComponent<IngredientItem>(out var ingredient))
            {
                HandleDelivery(ingredient.type);
                Destroy(other.gameObject);
                _lastDelivery = Time.time;
            }
        }
    }

    private void HandleDelivery(IngredientType type)
    {
        bool overDeliver = _questManager.TryDeliverIngredient(type);
        
        if (overDeliver)
        {
            _questManager.ResetCurrentQuest();
        }
    }
}
