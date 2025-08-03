using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public int damage = 1;
    public float knockbackForce = 5f;
    public bool destroyOnHit = true;
    
    [Header("Effects")]
    public ParticleSystem hitEffect;
    public AudioClip hitSound;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color dangerColor = Color.red;
    public float pulseSpeed = 2f;
    
    // Private variables
    private AudioSource audioSource;
    private bool hasHitPlayer = false;
    
    void Start()
    {
        // Get components
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Set tag
        gameObject.tag = "Obstacle";
        
        // Add collider if not present
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }
        
        Debug.Log("Obstacle: Initialized with tag 'Obstacle'");
    }
    
    void Update()
    {
        // Pulse effect
        UpdatePulse();
    }
    
    void UpdatePulse()
    {
        if (spriteRenderer != null)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.3f + 0.7f;
            spriteRenderer.color = Color.Lerp(Color.white, dangerColor, pulse);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player hit this obstacle
        if (other.CompareTag("Player") && !hasHitPlayer)
        {
            HitPlayer(other.gameObject);
        }
    }
    
    void HitPlayer(GameObject player)
    {
        if (hasHitPlayer) return;
        
        hasHitPlayer = true;
        Debug.Log("Obstacle: Hit player!");
        
        // Play hit sound
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        
        // Play hit effect
        if (hitEffect != null)
        {
            hitEffect.transform.position = transform.position;
            hitEffect.Play();
        }
        
        // Apply knockback to player
        ApplyKnockback(player);
        
        // Notify GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HitObstacle();
        }
        else
        {
            Debug.LogError("Obstacle: GameManager.Instance is null!");
        }
        
        // Destroy obstacle if configured to do so
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
        else
        {
            // Reset hit state after a delay
            Invoke(nameof(ResetHitState), 1f);
        }
    }
    
    /// <summary>
    /// Aplica knockback al jugador
    /// </summary>
    void ApplyKnockback(GameObject player)
    {
        if (knockbackForce <= 0) return;
        
        // Get player's Rigidbody2D
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Calculate knockback direction (away from obstacle)
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
            
            // Apply knockback force
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            
            Debug.Log($"Obstacle: Applied knockback force {knockbackForce} to player");
        }
    }
    
    /// <summary>
    /// Resetea el estado de hit
    /// </summary>
    void ResetHitState()
    {
        hasHitPlayer = false;
    }
    
    /// <summary>
    /// Establece el daño del obstáculo
    /// </summary>
    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }
    
    /// <summary>
    /// Establece la fuerza del knockback
    /// </summary>
    public void SetKnockbackForce(float force)
    {
        knockbackForce = force;
    }
    
    /// <summary>
    /// Establece si el obstáculo se destruye al golpear
    /// </summary>
    public void SetDestroyOnHit(bool destroy)
    {
        destroyOnHit = destroy;
    }
} 