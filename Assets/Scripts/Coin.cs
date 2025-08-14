using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int pointValue = 1;
    public float rotationSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;
    
    [Header("Effects")]
    public ParticleSystem collectEffect;
    public AudioClip collectSound;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color glowColor = Color.yellow;
    public float glowIntensity = 1f;
    
    // Private variables
    private Vector3 startPosition;
    private float bobTime;
    private AudioSource audioSource;
    private bool isCollected = false;
    
    void Start()
    {
        // Initialize
        startPosition = transform.position;
        bobTime = Random.Range(0f, 2f * Mathf.PI); // Random start phase
        
        // Get components
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Set tag
        gameObject.tag = "Collectible";
        
        // Add collider if not present
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }
        
        Debug.Log("Coin: Initialized with tag 'Collectible'");
    }
    
    void Update()
    {
        // Rotate the coin
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        
        // Bob up and down
        BobMovement();
        
        // Glow effect
        UpdateGlow();
    }
    
    void BobMovement()
    {
        bobTime += bobSpeed * Time.deltaTime;
        float bobOffset = Mathf.Sin(bobTime) * bobHeight;
        
        Vector3 newPosition = startPosition;
        newPosition.y += bobOffset;
        transform.position = newPosition;
    }
    
    void UpdateGlow()
    {
        if (spriteRenderer != null)
        {
            float glow = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f;
            spriteRenderer.color = Color.Lerp(Color.white, glowColor, glow * glowIntensity);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player collected this coin
        if (other.CompareTag("Player") && !isCollected)
        {
            Collect();
        }
    }
    
    void Collect()
    {
        if (isCollected) return;
        
        isCollected = true;
        Debug.Log("Coin: Collected by player!");
        
        // Play collect sound
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
        
        // Play collect effect
        if (collectEffect != null)
        {
            collectEffect.transform.position = transform.position;
            collectEffect.Play();
        }
        
        // Notify GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoin();
        }
        else
        {
            Debug.LogError("Coin: GameManager.Instance is null!");
        }
        
        // Destroy the coin
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Establece el valor de puntos de la moneda
    /// </summary>
    public void SetPointValue(int value)
    {
        pointValue = value;
    }
    
    /// <summary>
    /// Establece la velocidad de rotación
    /// </summary>
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    
    /// <summary>
    /// Establece la velocidad del movimiento de balanceo
    /// </summary>
    public void SetBobSpeed(float speed)
    {
        bobSpeed = speed;
    }
}




