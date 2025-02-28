using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float verticalForce;
    public float horizontalForce;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * verticalForce + Vector2.right * horizontalForce, ForceMode2D.Impulse);
    }
}
