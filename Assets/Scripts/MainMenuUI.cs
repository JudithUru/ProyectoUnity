using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button playButton;
    public Button quitButton;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI versionText;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip menuMusic;
    
    [Header("Components")]
    public GameManager gameManager;
    
    void Start()
    {
        Debug.Log("MainMenuUI: Start() called");
        
        // Get GameManager if not assigned
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            Debug.Log("MainMenuUI: GameManager found via Instance: " + (gameManager != null));
        }
        else
        {
            Debug.Log("MainMenuUI: GameManager assigned via Inspector: " + (gameManager != null));
        }
        
        // Get AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Initialize UI
        InitializeUI();
        
        // Play menu music
        PlayMenuMusic();
    }
    
    /// <summary>
    /// Inicializa la interfaz de usuario
    /// </summary>
    private void InitializeUI()
    {
        // Set up play button
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayButtonClicked);
            Debug.Log("MainMenuUI: Play button configured");
        }
        else
        {
            Debug.LogWarning("MainMenuUI: Play button not assigned!");
        }
        
        // Set up quit button
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            Debug.Log("MainMenuUI: Quit button configured");
        }
        else
        {
            Debug.LogWarning("MainMenuUI: Quit button not assigned!");
        }
        
        // Set version text
        if (versionText != null)
        {
            versionText.text = "v1.0.0";
        }
        
        Debug.Log("MainMenuUI: UI initialized successfully");
    }
    
    /// <summary>
    /// Se ejecuta cuando se presiona el botón Jugar
    /// </summary>
    public void OnPlayButtonClicked()
    {
        Debug.Log("MainMenuUI: Play button clicked!");
        
        // Play button click sound
        PlayButtonClickSound();
        
        // Start the game
        if (gameManager != null)
        {
            gameManager.StartRun();
        }
        else
        {
            Debug.LogError("MainMenuUI: GameManager is null, cannot start game!");
        }
    }
    
    /// <summary>
    /// Se ejecuta cuando se presiona el botón Salir
    /// </summary>
    public void OnQuitButtonClicked()
    {
        Debug.Log("MainMenuUI: Quit button clicked!");
        
        // Play button click sound
        PlayButtonClickSound();
        
        // Quit the game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
    /// Reproduce la música del menú
    /// </summary>
    private void PlayMenuMusic()
    {
        if (audioSource != null && menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    
    /// <summary>
    /// Establece el texto del título
    /// </summary>
    public void SetTitleText(string title)
    {
        if (titleText != null)
        {
            titleText.text = title;
        }
    }
    
    /// <summary>
    /// Establece el texto de la versión
    /// </summary>
    public void SetVersionText(string version)
    {
        if (versionText != null)
        {
            versionText.text = version;
        }
    }
}
