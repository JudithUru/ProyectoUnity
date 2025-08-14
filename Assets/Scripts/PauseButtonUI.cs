using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : MonoBehaviour
{
    [Header("UI References")]
    public Button pauseButton;
    public GameObject pausePanel;
    
    [Header("Button Settings")]
    public Sprite pauseIcon;
    public Sprite resumeIcon;
    
    private PauseButton pauseController;
    private Image buttonImage;
    private bool isPaused = false;
    
    void Start()
    {
        // Get components
        if (pauseButton == null)
            pauseButton = GetComponent<Button>();
            
        buttonImage = GetComponent<Image>();
        
        // Find pause controller
        pauseController = FindObjectOfType<PauseButton>();
        
        // Setup button listener
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
        
        // Set initial icon
        if (buttonImage != null && pauseIcon != null)
            buttonImage.sprite = pauseIcon;
            
        Debug.Log("PauseButtonUI: Initialized");
    }
    
    void Update()
    {
        // Update button icon based on pause state
        if (pauseController != null)
        {
            bool currentPausedState = pauseController.IsPaused();
            if (currentPausedState != isPaused)
            {
                isPaused = currentPausedState;
                UpdateButtonIcon();
            }
        }
    }
    
    public void TogglePause()
    {
        if (pauseController != null)
        {
            pauseController.TogglePause();
        }
        else
        {
            Debug.LogWarning("PauseButtonUI: PauseController not found!");
        }
    }
    
    private void UpdateButtonIcon()
    {
        if (buttonImage != null)
        {
            if (isPaused && resumeIcon != null)
            {
                buttonImage.sprite = resumeIcon;
            }
            else if (!isPaused && pauseIcon != null)
            {
                buttonImage.sprite = pauseIcon;
            }
        }
    }
}

