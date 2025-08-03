using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip clickSound;
    public AudioClip hoverSound;
    
    [Header("Settings")]
    public bool playOnClick = true;
    public bool playOnHover = false;
    public float volume = 1f;
    
    // Private variables
    private Button button;
    private AudioSource audioSource;
    
    void Start()
    {
        // Get components
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        
        // Add audio source if not present
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = volume;
        }
        
        // Subscribe to button events
        if (button != null)
        {
            if (playOnClick)
            {
                button.onClick.AddListener(PlayClickSound);
            }
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        if (button != null)
        {
            button.onClick.RemoveListener(PlayClickSound);
        }
    }
    
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
        }
        else if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }
    }
    
    public void PlayHoverSound()
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound, volume);
        }
    }
    
    public void SetClickSound(AudioClip sound)
    {
        clickSound = sound;
    }
    
    public void SetHoverSound(AudioClip sound)
    {
        hoverSound = sound;
    }
    
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
    
    public void SetPlayOnClick(bool enabled)
    {
        playOnClick = enabled;
        
        if (button != null)
        {
            if (enabled)
            {
                button.onClick.AddListener(PlayClickSound);
            }
            else
            {
                button.onClick.RemoveListener(PlayClickSound);
            }
        }
    }
    
    public void SetPlayOnHover(bool enabled)
    {
        playOnHover = enabled;
    }
    
    // Event handlers for UI events
    public void OnPointerEnter()
    {
        if (playOnHover)
        {
            PlayHoverSound();
        }
    }
    
    public void OnPointerExit()
    {
        // Optional: Add exit sound if needed
    }
} 