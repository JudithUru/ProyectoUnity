using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [Header("UI References")]
    public Button pauseButton;
    public GameObject pausePanel;
    public Button resumeButton;
    public Button quitButton;
    
    [Header("Game Objects to Pause")]
    public PlayerController2D player;
    public Spawner2D spawner;
    
    private bool isPaused = false;
    
    void Start()
    {
        // Get components if not assigned
        if (pauseButton == null)
            pauseButton = GetComponent<Button>();
            
        if (player == null)
            player = FindObjectOfType<PlayerController2D>();
            
        if (spawner == null)
            spawner = FindObjectOfType<Spawner2D>();
        
        // Setup button listeners
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
            
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        // Hide pause panel initially
        if (pausePanel != null)
            pausePanel.SetActive(false);
            
        Debug.Log("PauseButton: Initialized");
    }
    
    void Update()
    {
        // Allow ESC key to pause/unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        if (isPaused) return;
        
        isPaused = true;
        Time.timeScale = 0f; // Freeze time
        
        // Pause player
        if (player != null)
        {
            player.LockControls();
        }
        
        // Pause spawner
        if (spawner != null)
        {
            spawner.SetPaused(true);
        }
        
        // Show pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        Debug.Log("PauseButton: Game paused");
    }
    
    public void ResumeGame()
    {
        if (!isPaused) return;
        
        isPaused = false;
        Time.timeScale = 1f; // Resume time
        
        // Resume player
        if (player != null)
        {
            player.UnlockControls();
        }
        
        // Resume spawner
        if (spawner != null)
        {
            spawner.SetPaused(false);
        }
        
        // Hide pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        Debug.Log("PauseButton: Game resumed");
    }
    
    public void QuitGame()
    {
        Debug.Log("PauseButton: Quitting game");
        
        // Resume time before quitting
        Time.timeScale = 1f;
        
        // Quit the game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public bool IsPaused()
    {
        return isPaused;
    }
}

