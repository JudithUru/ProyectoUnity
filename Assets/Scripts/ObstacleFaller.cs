using UnityEngine;

public class ObstacleFaller : MonoBehaviour
{
    [Header("Falling Settings")]
    public float fallSpeed = 3f;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        // Get or add Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }
        
        // Set initial velocity downward
        rb.linearVelocity = Vector2.down * fallSpeed;
        
        Debug.Log($"ObstacleFaller: Obstacle falling with speed {fallSpeed}");
    }
    
    void Update()
    {
        // Keep moving downward at constant speed
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, -fallSpeed);
        }
    }
    
    void OnBecameInvisible()
    {
        // Destroy obstacle when it goes off screen
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
            Debug.Log("ObstacleFaller: Obstacle destroyed (off screen)");
        }
    }
}

