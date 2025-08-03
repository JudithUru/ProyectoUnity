using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Level System")]
    public LevelDatabase levelDatabase;
    
    [Header("Game State")]
    public int currentLevel = 1;
    public int lives = 3;
    public int coinsCollected = 0;
    public int targetCoins = 10;
    
    [Header("Game Status")]
    public bool isGameActive = false;
    public bool isGamePaused = false;
    public bool isLevelComplete = false;
    
    [Header("Audio")]
    public AudioManager audioManager;
    
    // Singleton instance
    public static GameManager Instance { get; private set; }
    
    // Events
    public Action<int> OnCoinsChanged;
    public Action<int> OnLivesChanged;
    public Action<int> OnLevelChanged;
    public Action OnLevelComplete;
    public Action OnGameOver;
    public Action OnGameWin;
    public Action OnGameStart;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager: Singleton instance created and marked as DontDestroyOnLoad");
        }
        else
        {
            Debug.Log("GameManager: Duplicate instance found, destroying this one");
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Initialize if no level database assigned
        if (levelDatabase == null)
        {
            Debug.LogWarning("GameManager: No LevelDatabase assigned! Creating default configuration.");
            CreateDefaultLevelDatabase();
        }
        
        // Get audio manager if not assigned
        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();
    }
    
    void OnDestroy()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    /// <summary>
    /// Inicia una nueva partida desde el menú principal
    /// </summary>
    public void StartRun()
    {
        Debug.Log("GameManager: Starting new run");
        
        // Reset game state
        ResetRun();
        
        // Load first level
        LoadLevel(1);
    }
    
    /// <summary>
    /// Carga un nivel específico
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        if (!levelDatabase.LevelExists(levelIndex))
        {
            Debug.LogError($"GameManager: Level {levelIndex} does not exist!");
            return;
        }
        
        Debug.Log($"GameManager: Loading level {levelIndex}");
        
        currentLevel = levelIndex;
        isLevelComplete = false;
        
        // Get level configuration
        LevelConfig levelConfig = levelDatabase.GetLevelConfig(levelIndex);
        if (levelConfig != null)
        {
            targetCoins = levelConfig.targetCoins;
        }
        
        // Reset coins for new level
        coinsCollected = 0;
        
        // Notify level change
        OnLevelChanged?.Invoke(currentLevel);
        
        // Load scene
        string sceneName = levelDatabase.GetSceneName(levelIndex);
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// Agrega una moneda y verifica si se completó el nivel
    /// </summary>
    public void AddCoin()
    {
        if (!isGameActive) return;
        
        coinsCollected++;
        Debug.Log($"GameManager: Coin collected! {coinsCollected}/{targetCoins}");
        
        // Notify UI
        OnCoinsChanged?.Invoke(coinsCollected);
        
        // Check if level is complete
        if (coinsCollected >= targetCoins)
        {
            CompleteLevel();
        }
        
        // Play collect sound
        if (audioManager != null)
            audioManager.PlayCollectSound();
    }
    
    /// <summary>
    /// Maneja el daño por obstáculo
    /// </summary>
    public void HitObstacle()
    {
        if (!isGameActive) return;
        
        lives--;
        Debug.Log($"GameManager: Hit obstacle! Lives remaining: {lives}");
        
        // Notify UI
        OnLivesChanged?.Invoke(lives);
        
        // Check if game over
        if (lives <= 0)
        {
            GameOver();
        }
        
        // Play damage sound
        if (audioManager != null)
            audioManager.PlayDamageSound();
    }
    
    /// <summary>
    /// Completa el nivel actual
    /// </summary>
    private void CompleteLevel()
    {
        if (isLevelComplete) return;
        
        isLevelComplete = true;
        Debug.Log($"GameManager: Level {currentLevel} completed!");
        
        // Add bonus life
        lives++;
        OnLivesChanged?.Invoke(lives);
        
        // Notify level complete
        OnLevelComplete?.Invoke();
        
        // Check if game won
        if (currentLevel >= levelDatabase.GetTotalLevels())
        {
            GameWin();
        }
        else
        {
            // Load next level after delay
            Invoke(nameof(LoadNextLevel), 2f);
        }
    }
    
    /// <summary>
    /// Carga el siguiente nivel
    /// </summary>
    private void LoadNextLevel()
    {
        LoadLevel(currentLevel + 1);
    }
    
    /// <summary>
    /// Carga el siguiente nivel o muestra victoria
    /// </summary>
    public void LoadNextLevelOrWin()
    {
        if (currentLevel >= levelDatabase.GetTotalLevels())
        {
            GameWin();
        }
        else
        {
            LoadLevel(currentLevel + 1);
        }
    }
    
    /// <summary>
    /// Reinicia la partida
    /// </summary>
    public void ResetRun()
    {
        Debug.Log("GameManager: Resetting run");
        
        // Reset game state
        currentLevel = 1;
        lives = levelDatabase.startingLives;
        coinsCollected = 0;
        targetCoins = 10;
        isGameActive = false;
        isGamePaused = false;
        isLevelComplete = false;
        
        // Notify UI
        OnCoinsChanged?.Invoke(coinsCollected);
        OnLivesChanged?.Invoke(lives);
        OnLevelChanged?.Invoke(currentLevel);
    }
    
    /// <summary>
    /// Game Over
    /// </summary>
    public void GameOver()
    {
        isGameActive = false;
        Debug.Log("GameManager: Game Over!");
        
        // Notify game over
        OnGameOver?.Invoke();
        
        // Play game over sound
        if (audioManager != null)
            audioManager.PlayGameOverSound();
    }
    
    /// <summary>
    /// Victoria del juego
    /// </summary>
    public void GameWin()
    {
        isGameActive = false;
        Debug.Log("GameManager: Game Won!");
        
        // Notify game win
        OnGameWin?.Invoke();
        
        // Play win sound
        if (audioManager != null)
            audioManager.PlayWinSound();
    }
    
    /// <summary>
    /// Inicia el juego (llamado desde LevelController)
    /// </summary>
    public void StartGame()
    {
        isGameActive = true;
        Debug.Log("GameManager: Game started!");
        
        // Notify game start
        OnGameStart?.Invoke();
    }
    
    /// <summary>
    /// Pausa el juego
    /// </summary>
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        Debug.Log("GameManager: Game paused");
    }
    
    /// <summary>
    /// Reanuda el juego
    /// </summary>
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        Debug.Log("GameManager: Game resumed");
    }
    
    /// <summary>
    /// Vuelve al menú principal
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    /// <summary>
    /// Se ejecuta cuando se carga una nueva escena
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"GameManager: Scene loaded: {scene.name}");
        
        // Reset coins when entering a new level
        if (scene.name.StartsWith("Level"))
        {
            coinsCollected = 0;
            OnCoinsChanged?.Invoke(coinsCollected);
        }
    }
    
    /// <summary>
    /// Crea una base de datos de niveles por defecto si no hay una asignada
    /// </summary>
    private void CreateDefaultLevelDatabase()
    {
        levelDatabase = ScriptableObject.CreateInstance<LevelDatabase>();
        levelDatabase.startingLives = 3;
        levelDatabase.sceneNames = new string[] { "Level1", "Level2", "Level3" };
        
        // Create default level configs
        levelDatabase.levels = new LevelConfig[3];
        
        for (int i = 0; i < 3; i++)
        {
            levelDatabase.levels[i] = ScriptableObject.CreateInstance<LevelConfig>();
            levelDatabase.levels[i].levelIndex = i + 1;
            levelDatabase.levels[i].levelName = $"Level {i + 1}";
            levelDatabase.levels[i].targetCoins = 10 + (i * 5); // 10, 15, 20
            levelDatabase.levels[i].collectibleSpawnInterval = 2f;
            levelDatabase.levels[i].obstacleSpawnInterval = 3f;
            levelDatabase.levels[i].maxObstaclesOnScreen = 15;
        }
        
        Debug.Log("GameManager: Default LevelDatabase created");
    }
    
    /// <summary>
    /// Obtiene la configuración del nivel actual
    /// </summary>
    public LevelConfig GetCurrentLevelConfig()
    {
        return levelDatabase.GetLevelConfig(currentLevel);
    }
} 