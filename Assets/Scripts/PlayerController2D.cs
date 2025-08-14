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
    private ParticleManager particleManager;
    private Animator animator; // Agregamos referencia al Animator
    
    [Header("Gravity Settings")]
    public float gravityScale = 0.5f;
    public float maxFallSpeed = 10f;
    public float idleGravityScale = 0.3f; // Gravedad cuando está quieto
    public float movingGravityScale = 0.1f; // Gravedad cuando se mueve
    
    [Header("Control Settings")]
    public bool controlsLocked = false; // Controla si los controles están bloqueados
    
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Obtenemos el componente Animator
        
        // Configure Rigidbody2D
        if (rb != null)
        {
            rb.gravityScale = 0.1f; // Reduced gravity for better control
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        // Get or create ParticleManager
        particleManager = GetComponent<ParticleManager>();
        if (particleManager == null)
        {
            // Add ParticleManager component and make it visible in Inspector
            particleManager = gameObject.AddComponent<ParticleManager>();
            
            // Force Unity to refresh the Inspector
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
            
            Debug.Log("PlayerController2D: ParticleManager component added");
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
        // Check if controls are locked
        if (controlsLocked)
        {
            // Stop all movement when controls are locked
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f; // Disable gravity when controls are locked
            }
            
            // Set idle animation when controls are locked
            if (animator != null)
            {
                animator.SetBool("IsMoving", false);
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 0);
            }
            
            return; // Exit early, don't process any input
        }
        
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
        
        // Check if player is moving
        bool isMoving = movement.magnitude > 0.1f;
        
        // Update Animator parameters
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
            
            // Set Direction parameter based on input
            if (isMoving)
            {
                // Determine direction based on input
                if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
                {
                    // Horizontal movement is dominant
                    if (horizontalInput > 0)
                    {
                        animator.SetInteger("Direction", 1); // Derecha
                    }
                    else
                    {
                        animator.SetInteger("Direction", 2); // Izquierda
                    }
                }
                else
                {
                    // Vertical movement is dominant
                    if (verticalInput > 0)
                    {
                        animator.SetInteger("Direction", 3); // Arriba (asumiendo que 3 es arriba)
                    }
                    else
                    {
                        animator.SetInteger("Direction", 0); // Abajo
                    }
                }
            }
            else
            {
                // When not moving, keep the last direction
                // This prevents the animation from changing when stopping
            }
        }
        
        // Apply gravity based on movement state
        if (rb != null)
        {
            if (isMoving)
            {
                // When moving: apply movement and light gravity
                rb.gravityScale = movingGravityScale;
                
                // Apply movement in all directions
                Vector2 newVelocity = movement * moveSpeed;
                
                // Apply movement
                rb.linearVelocity = newVelocity;
                
                // Clamp velocity to max speed
                if (rb.linearVelocity.magnitude > maxSpeed)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
                }
            }
            else
            {
                // When idle: apply heavy gravity for falling effect
                rb.gravityScale = idleGravityScale;
                
                // Keep current horizontal velocity but let gravity affect Y
                Vector2 currentVelocity = rb.linearVelocity;
                currentVelocity.x = Mathf.Lerp(currentVelocity.x, 0f, Time.deltaTime * 5f); // Slow horizontal stop
                rb.linearVelocity = currentVelocity;
            }
            
            // Clamp fall speed to prevent falling too fast
            if (rb.linearVelocity.y < -maxFallSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);
            }
        }
        
        // NOTA: Ya no usamos flipX aquí, el Animator se encarga de la dirección
        // El código anterior era:
        // if (horizontalInput != 0 && spriteRenderer != null)
        // {
        //     spriteRenderer.flipX = horizontalInput < 0;
        // }
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
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collisions with collectibles and obstacles
        if (collision.gameObject.CompareTag("Collectible"))
        {
            Debug.Log("Player: Collected item!");
            // The Coin script will handle the collection
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
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
    
    /// <summary>
    /// Bloquea los controles del jugador (usado cuando pierde)
    /// </summary>
    public void LockControls()
    {
        controlsLocked = true;
        Debug.Log("PlayerController2D: Controls locked");
        
        // Stop all movement and disable gravity
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
    }
    
    /// <summary>
    /// Desbloquea los controles del jugador (usado al reiniciar)
    /// </summary>
    public void UnlockControls()
    {
        controlsLocked = false;
        Debug.Log("PlayerController2D: Controls unlocked");
        
        // Restore normal gravity
        if (rb != null)
        {
            rb.gravityScale = idleGravityScale;
        }
    }
}
