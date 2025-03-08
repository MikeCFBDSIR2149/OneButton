using System;
using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    public IngredientType type;
    public SpriteRenderer iconRenderer;
    private Knife knife;
    private Rigidbody2D rb;
    public PhysicsMaterial2D originalMaterial;
    public PhysicsMaterial2D finalMaterial;
    private PolygonCollider2D polygonCollider;
    
    // 预制体初始化方法
    public void Initialize(IngredientType itemType, Sprite icon)
    {
        type = itemType;
        if(iconRenderer) iconRenderer.sprite = icon;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider) 
            polygonCollider.sharedMaterial = originalMaterial;
    }

    private void OnEnable()
    {
        knife = FindFirstObjectByType<Knife>();
        knife.cutEvent += Move;
    }

    private void OnDisable()
    {
        knife.cutEvent -= Move;
    }

    private void Move()
    {
        if (polygonCollider)
            polygonCollider.sharedMaterial = finalMaterial;
        rb.velocityY = Mathf.Clamp(rb.velocityY, -1.4f, 1);
        rb.position += new Vector2(0, 1.5f);
        // rb.velocity = new Vector2(4f, 0f);
        rb.AddForce(new Vector2(6f, 5f), ForceMode2D.Impulse);
    }
}
