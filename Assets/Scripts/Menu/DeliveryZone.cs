// using UnityEngine;
//
// public class DeliveryZone : MonoBehaviour
// {
//     [Header("设置")]
//     public LayerMask ingredientLayer;
//     public float cooldown = 0.5f;
//
//     private QuestManager _questManager;
//     private float _lastDelivery;
//
//     private void Start()
//     {
//         _questManager = FindFirstObjectByType<QuestManager>();
//         GetComponent<Collider2D>().isTrigger = true;
//     }
//
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (Time.time - _lastDelivery < cooldown) return;
//
//         if (((1 << other.gameObject.layer) & ingredientLayer) != 0)
//         {
//             if (other.TryGetComponent<IngredientItem>(out var ingredient))
//             {
//                 HandleDelivery(ingredient.type);
//                 Destroy(other.gameObject);
//                 _lastDelivery = Time.time;
//             }
//         }
//     }
//
//     private void HandleDelivery(IngredientType type)
//     {
//         bool overDeliver = _questManager.TryDeliverIngredient(type);
//         
//         if (overDeliver)
//         {
//             _questManager.ResetCurrentQuest();
//         }
//     }
// }

using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask ingredientLayer;
    public float checkInterval = 0.1f;

    private QuestManager _questManager;
    private float _lastCheckTime;

    private void Start()
    {
        _questManager = FindFirstObjectByType<QuestManager>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time - _lastCheckTime < checkInterval) return;

        var ingredients = new System.Collections.Generic.HashSet<IngredientItem>();

        var colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, ingredientLayer);
        foreach (Collider2D collider2d in colliders)
        {
            if (collider2d.TryGetComponent<IngredientItem>(out var ingredient))
            {
                ingredients.Add(ingredient);
            }
        }

        foreach (IngredientItem ingredient in ingredients)
        {
            Debug.Log(ingredient.type);
            HandleDelivery(ingredient.type);
            Destroy(ingredient.gameObject);
        }

        _lastCheckTime = Time.time;
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
