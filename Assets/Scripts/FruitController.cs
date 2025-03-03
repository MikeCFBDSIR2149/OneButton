using UnityEngine;

public class FruitController : MonoBehaviour
{
    public float verticalForce;
    public float horizontalForce;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * verticalForce + Vector2.right * horizontalForce, ForceMode2D.Impulse);
    }
}
