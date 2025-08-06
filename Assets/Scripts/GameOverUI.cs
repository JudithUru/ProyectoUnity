using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button retryButton;
    public Button mainMenuButton;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI levelReachedText;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip gameOverSound;
    
    [Header("Components")]
    public GameManager gameManager;
    
    void Start()
    {
        Debug.Log("GameOverUI: Start() called");
        
        // Get GameManager if not assigned
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            Debug.Log("GameOverUI: GameManager found via Instance: " + (gameManager != null));
        }
        else
        {
            Debug.Log("GameOverUI: GameManager assigned via Inspector: " + (gameManager != null));
        }
        
        // Get AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Initialize UI
        InitializeUI();
        
        // Play game over sound
        PlayGameOverSound();
        
        // Update UI with final stats
        UpdateFinalStats();
    }
    
    /// <summary>
    /// Inicializa la interfaz de usuario
    /// </summary>
    private void InitializeUI()
    {
        // Set up retry button
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryButtonClicked);
            Debug.Log("GameOverUI: Retry button configured");
        }
        else
        {
            Debug.LogWarning("GameOverUI: Retry button not assigned!");
        }
        
        // Set up main menu button
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            Debug.Log("GameOverUI: Main menu button configured");
        }
        else
        {
            Debug.LogWarning("GameOverUI: Main menu button not assigned!");
        }
        
        Debug.Log("GameOverUI: UI initialized successfully");
    }
    
    /// <summary>
    /// Se ejecuta cuando se presiona el botón Reintentar
    /// </summary>
    public void OnRetryButtonClicked()
    {
        Debug.Log("GameOverUI: Retry button clicked!");
        
        // Play button click sound
        PlayButtonClickSound();
        
        // No detener el audio aquí, el GameManager se encargará
        // El audio se detendrá solo cuando sea necesario
        
        // Retry the run
        if (gameManager != null)
        {
            gameManager.ResetRun();
        }
        else
        {
            Debug.LogError("GameOverUI: GameManager is null, cannot retry!");
        }
    }
    
    /// <summary>
    /// Se ejecuta cuando se presiona el botón Menú Principal
    /// </summary>
    public void OnMainMenuButtonClicked()
    {
        Debug.Log("GameOverUI: Main menu button clicked!");
        
        // Play button click sound
        PlayButtonClickSound();
        
        // Go to main menu
        if (gameManager != null)
        {
            gameManager.LoadMainMenu();
        }
        else
        {
            Debug.LogError("GameOverUI: GameManager is null, cannot load main menu!");
        }
    }
    
    /// <summary>
    /// Actualiza las estadísticas finales
    /// </summary>
    private void UpdateFinalStats()
    {
        if (gameManager == null) return;
        
        // Update final score text
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {gameManager.coinsCollected}";
        }
        
        // Update level reached text
        if (levelReachedText != null)
        {
            levelReachedText.text = $"Level Reached: {gameManager.currentLevel}";
        }
        
        Debug.Log("GameOverUI: Final stats updated");
    }
    
    /// <summary>
    /// Reproduce el sonido del botón
    /// </summary>
    private void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de Game Over
    /// </summary>
    private void PlayGameOverSound()
    {
        // Usar el AudioManager principal para reproducir música de Game Over
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverMusic();
        }
        else if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
    }
    
    /// <summary>
    /// Establece el texto de Game Over
    /// </summary>
    public void SetGameOverText(string text)
    {
        if (gameOverText != null)
        {
            gameOverText.text = text;
        }
    }
    
    /// <summary>
    /// Establece el texto del puntaje final
    /// </summary>
    public void SetFinalScoreText(string text)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = text;
        }
    }
    
    /// <summary>
    /// Establece el texto del nivel alcanzado
    /// </summary>
    public void SetLevelReachedText(string text)
    {
        if (levelReachedText != null)
        {
            levelReachedText.text = text;
        }
    }
}




