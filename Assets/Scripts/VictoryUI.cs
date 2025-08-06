using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button mainMenuButton;
    public Button playAgainButton;
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI totalLivesText;
    public TextMeshProUGUI congratulationsText;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip victorySound;
    
    [Header("Effects")]
    public ParticleSystem victoryEffect;
    
    [Header("Components")]
    public GameManager gameManager;
    
    void Start()
    {
        Debug.Log("VictoryUI: Start() called");
        
        // Get GameManager if not assigned
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            Debug.Log("VictoryUI: GameManager found via Instance: " + (gameManager != null));
        }
        else
        {
            Debug.Log("VictoryUI: GameManager assigned via Inspector: " + (gameManager != null));
        }
        
        // Get AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Initialize UI
        InitializeUI();
        
        // Play victory sound
        PlayVictorySound();
        
        // Play victory effect
        PlayVictoryEffect();
        
        // Update UI with final stats
        UpdateFinalStats();
    }
    
    /// <summary>
    /// Inicializa la interfaz de usuario
    /// </summary>
    private void InitializeUI()
    {
        // Set up main menu button
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            Debug.Log("VictoryUI: Main menu button configured");
        }
        else
        {
            Debug.LogWarning("VictoryUI: Main menu button not assigned!");
        }
        
        // Set up play again button
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
            Debug.Log("VictoryUI: Play again button configured");
        }
        else
        {
            Debug.LogWarning("VictoryUI: Play again button not assigned!");
        }
        
        Debug.Log("VictoryUI: UI initialized successfully");
    }
    
    /// <summary>
    /// Se ejecuta cuando se presiona el botón Menú Principal
    /// </summary>
    public void OnMainMenuButtonClicked()
    {
        Debug.Log("VictoryUI: Main menu button clicked!");
        
        // Play button click sound
        PlayButtonClickSound();
        
        // Go to main menu
        if (gameManager != null)
        {
            gameManager.LoadMainMenu();
        }
        else
        {
            Debug.LogError("VictoryUI: GameManager is null, cannot load main menu!");
        }
    }
    
    /// <summary>
    /// Se ejecuta cuando se presiona el botón Jugar de Nuevo
    /// </summary>
    public void OnPlayAgainButtonClicked()
    {
        Debug.Log("VictoryUI: Play again button clicked!");
        
        // Play button click sound
        PlayButtonClickSound();
        
        // No detener el audio aquí, el GameManager se encargará
        // El audio se detendrá solo cuando sea necesario
        
        // Start a new run
        if (gameManager != null)
        {
            gameManager.StartRun();
        }
        else
        {
            Debug.LogError("VictoryUI: GameManager is null, cannot start new run!");
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
        
        // Update total lives text
        if (totalLivesText != null)
        {
            totalLivesText.text = $"Final Lives: {gameManager.lives}";
        }
        
        // Update congratulations text
        if (congratulationsText != null)
        {
            congratulationsText.text = "¡Felicitaciones! ¡Has completado todos los niveles!";
        }
        
        Debug.Log("VictoryUI: Final stats updated");
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
    /// Reproduce el sonido de victoria
    /// </summary>
    private void PlayVictorySound()
    {
        // Usar el AudioManager principal para reproducir música de Win
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWinMusic();
        }
        else if (audioSource != null && victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }
    }
    
    /// <summary>
    /// Reproduce el efecto de victoria
    /// </summary>
    private void PlayVictoryEffect()
    {
        if (victoryEffect != null)
        {
            victoryEffect.Play();
        }
    }
    
    /// <summary>
    /// Establece el texto de victoria
    /// </summary>
    public void SetVictoryText(string text)
    {
        if (victoryText != null)
        {
            victoryText.text = text;
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
    /// Establece el texto de las vidas totales
    /// </summary>
    public void SetTotalLivesText(string text)
    {
        if (totalLivesText != null)
        {
            totalLivesText.text = text;
        }
    }
    
    /// <summary>
    /// Establece el texto de felicitaciones
    /// </summary>
    public void SetCongratulationsText(string text)
    {
        if (congratulationsText != null)
        {
            congratulationsText.text = text;
        }
    }
}




