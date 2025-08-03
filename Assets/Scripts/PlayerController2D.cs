using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxSpeed = 8f;
    
    [Header("Boundaries")]
    public float boundaryX = 20f;
    public float boundaryY = 20f;
    
    [Header("Components")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Configure Rigidbody2D
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        Debug.Log("PlayerController2D: Initialized");
        
        // Auto-configure boundaries from camera
        AutoConfigureBoundaries();
    }
    
    void Update()
    {
        // Handle input
        HandleInput();
        
        // Keep player within boundaries
        ClampPosition();
    }
    
    void HandleInput()
    {
        // Get input axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Create movement vector
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        
        // Normalize to prevent faster diagonal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        // Apply movement
        if (rb != null)
        {
            rb.linearVelocity = movement * moveSpeed;
            
            // Clamp velocity to max speed
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
        
        // Flip sprite based on movement direction
        if (horizontalInput != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = horizontalInput < 0;
        }
    }
    
    void ClampPosition()
    {
        Vector3 pos = transform.position;
        
        // Clamp X position
        pos.x = Mathf.Clamp(pos.x, -boundaryX, boundaryX);
        
        // Clamp Y position
        pos.y = Mathf.Clamp(pos.y, -boundaryY, boundaryY);
        
        transform.position = pos;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collisions with collectibles and obstacles
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Player: Collected item!");
            // The Coin script will handle the collection
        }
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Player: Hit obstacle!");
            // The Obstacle script will handle the damage
        }
    }
    
    // Public methods for external control
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    
    public void SetMaxSpeed(float speed)
    {
        maxSpeed = speed;
    }
    
    public void SetBoundaries(float x, float y)
    {
        boundaryX = x;
        boundaryY = y;
    }
    
    /// <summary>
    /// Configura automáticamente los límites basándose en la cámara
    /// </summary>
    private void AutoConfigureBoundaries()
    {
        CameraFollow cameraFollow = Camera.main?.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            SetBoundariesFromCamera(cameraFollow);
        }
        else
        {
            Debug.LogWarning("PlayerController2D: No CameraFollow found, using default boundaries");
        }
    }
    
    /// <summary>
    /// Establece los límites basándose en el tamaño del mundo de la cámara
    /// </summary>
    public void SetBoundariesFromCamera(CameraFollow cameraFollow)
    {
        if (cameraFollow != null)
        {
            boundaryX = cameraFollow.worldWidth / 2f;
            boundaryY = cameraFollow.worldHeight / 2f;
            Debug.Log($"PlayerController2D: Boundaries automatically updated to {boundaryX}x{boundaryY}");
        }
    }
}
