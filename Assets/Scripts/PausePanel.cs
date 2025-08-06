using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [Header("UI References")]
    public Button resumeButton;
    public Button quitButton;
    
    [Header("Audio")]
    public AudioClip buttonClickSound;
    
    private AudioSource audioSource;
    private PauseButton pauseButton;
    
    void Start()
    {
        // Get components
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
            
        pauseButton = FindObjectOfType<PauseButton>();
        
        // Setup button listeners
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        // Hide panel initially
        gameObject.SetActive(false);
        
        Debug.Log("PausePanel: Initialized");
    }
    
    public void ResumeGame()
    {
        PlayButtonSound();
        
        if (pauseButton != null)
        {
            pauseButton.ResumeGame();
        }
        else
        {
            Debug.LogWarning("PausePanel: PauseButton not found!");
        }
    }
    
    public void QuitGame()
    {
        PlayButtonSound();
        
        if (pauseButton != null)
        {
            pauseButton.QuitGame();
        }
        else
        {
            Debug.LogWarning("PausePanel: PauseButton not found!");
        }
    }
    
    private void PlayButtonSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}

