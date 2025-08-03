using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI targetText;

    [Header("Components")]
    public GameManager gameManager;

    void Start()
    {
        Debug.Log("HUDController: Start() called");
        
        // Get GameManager if not assigned
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            Debug.Log("HUDController: GameManager found via Instance: " + (gameManager != null));
        }
        else
        {
            Debug.Log("HUDController: GameManager assigned via Inspector: " + (gameManager != null));
        }
        
        // Subscribe to events
        SubscribeToEvents();
        
        // Initialize HUD
        InitializeHUD();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void InitializeHUD()
    {
        Debug.Log("HUDController: Initializing HUD");
        
        if (gameManager == null)
        {
            Debug.LogWarning("HUDController: GameManager is null, cannot initialize HUD!");
            return;
        }
        
        // Update displays
        UpdateCoinsDisplay(gameManager.coinsCollected);
        UpdateLivesDisplay(gameManager.lives);
        UpdateLevelDisplay(gameManager.currentLevel);
        UpdateTargetDisplay(gameManager.targetCoins);
        
        Debug.Log("HUDController: HUD initialized successfully");
    }

    private void SubscribeToEvents()
    {
        if (gameManager == null) return;
        
        gameManager.OnCoinsChanged += UpdateCoinsDisplay;
        gameManager.OnLivesChanged += UpdateLivesDisplay;
        gameManager.OnLevelChanged += UpdateLevelDisplay;
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnGameWin += OnGameWin;
        
        Debug.Log("HUDController: Subscribed to GameManager events");
    }

    private void UnsubscribeFromEvents()
    {
        if (gameManager == null) return;
        
        gameManager.OnCoinsChanged -= UpdateCoinsDisplay;
        gameManager.OnLivesChanged -= UpdateLivesDisplay;
        gameManager.OnLevelChanged -= UpdateLevelDisplay;
        gameManager.OnGameStart -= OnGameStart;
        gameManager.OnGameOver -= OnGameOver;
        gameManager.OnGameWin -= OnGameWin;
    }

    // Update methods
    private void UpdateCoinsDisplay(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = $"Coins: {coins}";
            Debug.Log($"HUDController: Coins updated to {coins}");
        }
        else
        {
            Debug.LogWarning("HUDController: coinsText is null!");
        }
    }

    private void UpdateLivesDisplay(int lives)
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {lives}";
            Debug.Log($"HUDController: Lives updated to {lives}");
        }
        else
        {
            Debug.LogWarning("HUDController: livesText is null!");
        }
    }

    private void UpdateLevelDisplay(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Level {level}";
            Debug.Log($"HUDController: Level updated to {level}");
        }
        else
        {
            Debug.LogWarning("HUDController: levelText is null!");
        }
    }

    private void UpdateTargetDisplay(int target)
    {
        if (targetText != null)
        {
            targetText.text = $"Target: {target}";
            Debug.Log($"HUDController: Target updated to {target}");
        }
        else
        {
            Debug.LogWarning("HUDController: targetText is null!");
        }
    }

    // Event handlers
    private void OnGameStart()
    {
        Debug.Log("HUDController: Game started");
    }

    private void OnGameOver()
    {
        Debug.Log("HUDController: Game Over");
    }

    private void OnGameWin()
    {
        Debug.Log("HUDController: Game Won");
    }

    // Public methods for manual updates (if needed)
    public void UpdateCoins(int coins)
    {
        UpdateCoinsDisplay(coins);
    }

    public void UpdateLives(int lives)
    {
        UpdateLivesDisplay(lives);
    }

    public void UpdateLevel(int level)
    {
        UpdateLevelDisplay(level);
    }

    public void UpdateTarget(int target)
    {
        UpdateTargetDisplay(target);
    }
}

