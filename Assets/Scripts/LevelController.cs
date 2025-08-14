using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Level Components")]
    public Spawner2D spawner;
    public HUDController hudController;
    public GameObject victoryPanel;
    public GameObject gameOverPanel;
    public LevelTransitionPanel levelTransitionPanel;
    
    [Header("Level Celebration")]
    public ParticleSystem levelCompleteParticles;
    public float celebrationDuration = 2f;

    [Header("Level Settings")]
    public int levelIndex = 1;

    private GameManager gameManager;
    private LevelConfig currentLevelConfig;

    void Start()
    {
        Debug.Log($"LevelController: Initializing level {levelIndex}");
        
        // Get GameManager
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("LevelController: GameManager not found!");
            return;
        }

        // Get current level config
        currentLevelConfig = gameManager.GetCurrentLevelConfig();
        if (currentLevelConfig == null)
        {
            Debug.LogError($"LevelController: No config found for level {levelIndex}!");
            return;
        }

        // Configure spawner
        if (spawner != null)
        {
            ConfigureSpawner();
        }
        else
        {
            Debug.LogWarning("LevelController: Spawner not assigned!");
        }

        // Initialize HUD
        if (hudController != null)
        {
            hudController.InitializeHUD();
        }
        else
        {
            Debug.LogWarning("LevelController: HUDController not assigned!");
        }

        // Subscribe to events
        SubscribeToEvents();
        
        // Start level
        StartLevel();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void ConfigureSpawner()
    {
        if (currentLevelConfig == null) return;
        
        Debug.Log($"LevelController: Configuring spawner for level {levelIndex}");
        
        spawner.collectibleSpawnInterval = currentLevelConfig.collectibleSpawnInterval;
        spawner.obstacleSpawnInterval = currentLevelConfig.obstacleSpawnInterval;
        spawner.maxObstaclesOnScreen = currentLevelConfig.maxObstaclesOnScreen;
        spawner.targetCoins = currentLevelConfig.targetCoins;
        spawner.obstacleSpeed = currentLevelConfig.obstacleSpeed;
        spawner.collectibleSpeed = currentLevelConfig.collectibleSpeed;
        
        Debug.Log($"LevelController: Spawner configured - Target Coins: {spawner.targetCoins}");
    }

    private void StartLevel()
    {
        Debug.Log($"LevelController: Starting level {levelIndex}");
        gameManager.StartGame();
        
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
    }

    private void SubscribeToEvents()
    {
        if (gameManager == null) return;
        
        gameManager.OnLevelComplete += OnLevelComplete;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnGameWin += OnGameWin;
    }

    private void UnsubscribeFromEvents()
    {
        if (gameManager == null) return;
        
        gameManager.OnLevelComplete -= OnLevelComplete;
        gameManager.OnGameOver -= OnGameOver;
        gameManager.OnGameWin -= OnGameWin;
    }

    private void OnLevelComplete()
    {
        Debug.Log($"LevelController: Level {levelIndex} completed!");
        
        if (spawner != null)
        {
            spawner.StopSpawning();
        }
        
        // Show level transition panel if not the final level
        if (levelTransitionPanel != null && levelIndex < 3)
        {
            levelTransitionPanel.ShowLevelTransition(levelIndex + 1);
        }
        
        // Start level celebration
        StartCoroutine(CelebrateLevelComplete());
    }
    
    /// <summary>
    /// Corrutina para celebrar la completación del nivel
    /// </summary>
    private System.Collections.IEnumerator CelebrateLevelComplete()
    {
        Debug.Log($"LevelController: Starting celebration for level {levelIndex}");
        
        // Play level complete sound
        if (gameManager != null && gameManager.audioManager != null)
        {
            if (levelIndex < 3)
            {
                gameManager.audioManager.PlayLevelCompleteSound();
            }
            else
            {
                gameManager.audioManager.PlayWinSound();
            }
        }
        
        // Show celebration particles
        if (levelCompleteParticles != null)
        {
            levelCompleteParticles.Play();
        }
        
        // Wait for celebration duration
        yield return new WaitForSeconds(celebrationDuration);
        
        // Stop particles
        if (levelCompleteParticles != null)
        {
            levelCompleteParticles.Stop();
        }
        
        Debug.Log($"LevelController: Celebration completed for level {levelIndex}");
    }

    private void OnGameOver()
    {
        Debug.Log($"LevelController: Game Over on level {levelIndex}");
        
        if (spawner != null)
        {
            spawner.StopSpawning();
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("LevelController: GameOverPanel not assigned!");
        }
    }

    private void OnGameWin()
    {
        Debug.Log($"LevelController: Game Won on level {levelIndex}");
        
        if (spawner != null)
        {
            spawner.StopSpawning();
        }
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("LevelController: VictoryPanel not assigned!");
        }
    }

    // Public methods for UI buttons
    public void RestartLevel()
    {
        Debug.Log($"LevelController: Restarting level {levelIndex}");
        
        // Reset the run first
        if (gameManager != null)
        {
            gameManager.ResetRun();
        }
        
        // Then reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Debug.Log("LevelController: Loading main menu");
        gameManager.LoadMainMenu();
    }

    public void RetryRun()
    {
        Debug.Log("LevelController: Retrying run");
        gameManager.ResetRun();
        gameManager.LoadLevel(1);
    }
}

