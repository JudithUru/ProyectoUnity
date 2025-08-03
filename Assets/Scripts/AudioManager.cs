using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Background Music")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    
    [Header("Sound Effects")]
    public AudioClip collectSound;
    public AudioClip damageSound;
    public AudioClip gameStartSound;
    public AudioClip gameOverSound;
    public AudioClip winSound;
    public AudioClip buttonClickSound;
    
    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    // Singleton instance
    public static AudioManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Initialize audio sources if not assigned
        InitializeAudioSources();
        
        // Set initial volumes
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        
        // Play main menu music
        PlayMainMenuMusic();
    }
    
    void InitializeAudioSources()
    {
        // Get or create music source
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        
        // Get or create SFX source
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
    }
    
    public void PlayMainMenuMusic()
    {
        if (musicSource != null && mainMenuMusic != null)
        {
            musicSource.clip = mainMenuMusic;
            musicSource.Play();
        }
    }
    
    public void PlayGameMusic()
    {
        if (musicSource != null && gameMusic != null)
        {
            musicSource.clip = gameMusic;
            musicSource.Play();
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    
    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
    
    public void PlayCollectSound()
    {
        PlaySFX(collectSound);
    }
    
    public void PlayDamageSound()
    {
        PlaySFX(damageSound);
    }
    
    public void PlayGameStartSound()
    {
        PlaySFX(gameStartSound);
    }
    
    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
    }
    
    public void PlayWinSound()
    {
        PlaySFX(winSound);
    }
    
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }
    
    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }
    
    public void MuteMusic(bool mute)
    {
        if (musicSource != null)
        {
            musicSource.mute = mute;
        }
    }
    
    public void MuteSFX(bool mute)
    {
        if (sfxSource != null)
        {
            sfxSource.mute = mute;
        }
    }
    
    public void MuteAll(bool mute)
    {
        MuteMusic(mute);
        MuteSFX(mute);
    }
    
    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeMusic(0f, duration));
    }
    
    public void FadeInMusic(float duration)
    {
        StartCoroutine(FadeMusic(musicVolume, duration));
    }
    
    System.Collections.IEnumerator FadeMusic(float targetVolume, float duration)
    {
        if (musicSource == null) yield break;
        
        float startVolume = musicSource.volume;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }
        
        musicSource.volume = targetVolume;
    }
} 