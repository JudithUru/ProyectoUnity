using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Background Music")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    public AudioClip gameOverMusic;
    public AudioClip winMusic;
    
    [Header("Sound Effects")]
    public AudioClip collectSound;
    public AudioClip damageSound;
    public AudioClip gameStartSound;
    public AudioClip gameOverSound;
    public AudioClip winSound;
    public AudioClip buttonClickSound;
    public AudioClip levelCompleteSound;
    public AudioClip levelUpSound;
    
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
        Debug.Log("AudioManager: Initializing audio sources...");
        
        // Get or create music source
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            Debug.Log("AudioManager: Created new music source");
        }
        else
        {
            Debug.Log("AudioManager: Using existing music source");
        }
        
        // Get or create SFX source
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            Debug.Log("AudioManager: Created new SFX source");
        }
        else
        {
            Debug.Log("AudioManager: Using existing SFX source");
        }
        
        Debug.Log($"AudioManager: Audio sources initialized. Music: {musicSource != null}, SFX: {sfxSource != null}");
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
        Debug.Log("AudioManager: PlayGameMusic() called");
        
        if (musicSource == null)
        {
            Debug.LogError("AudioManager: musicSource is null!");
            return;
        }
        
        if (gameMusic == null)
        {
            Debug.LogError("AudioManager: gameMusic clip is null!");
            return;
        }
        
        Debug.Log($"AudioManager: Setting game music clip and playing. Current clip: {musicSource.clip}, New clip: {gameMusic}");
        
        // Siempre cambiar y reproducir la música del juego
        musicSource.clip = gameMusic;
        musicSource.volume = musicVolume;
        musicSource.Play();
        
        Debug.Log($"AudioManager: Game music started. Is playing: {musicSource.isPlaying}, Volume: {musicSource.volume}");
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
        // Apagar la música del juego con fade out
        FadeOutMusic(1f);
        // Reproducir el sonido de game over
        PlaySFX(gameOverSound);
    }
    
    /// <summary>
    /// Reproduce el sonido de Game Over y apaga la música del juego
    /// </summary>
    public void PlayGameOverMusic()
    {
        // Apagar la música del juego
        StopMusic();
        // Reproducir la música de Game Over
        if (gameOverMusic != null)
        {
            musicSource.clip = gameOverMusic;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }
    
    public void PlayWinSound()
    {
        // Apagar la música del juego con fade out
        FadeOutMusic(1f);
        // Reproducir el sonido de victoria
        PlaySFX(winSound);
    }
    
    /// <summary>
    /// Reproduce la música de Win y apaga la música del juego
    /// </summary>
    public void PlayWinMusic()
    {
        // Apagar la música del juego
        StopMusic();
        // Reproducir la música de Win
        if (winMusic != null)
        {
            musicSource.clip = winMusic;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }
    
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }
    
    public void PlayLevelCompleteSound()
    {
        PlaySFX(levelCompleteSound);
    }
    
    public void PlayLevelUpSound()
    {
        // Reproducir sonido de subir nivel
        PlaySFX(levelUpSound);
        // La música del juego continúa sin interrupción
    }
    
    /// <summary>
    /// Transición suave entre niveles (música continúa)
    /// </summary>
    public void OnLevelTransition()
    {
        // Solo reproducir el sonido, la música continúa
        PlaySFX(levelUpSound);
        // No se apaga la música del juego
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
    
    /// <summary>
    /// Verifica si la música del juego está sonando
    /// </summary>
    public bool IsGameMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying && musicSource.clip == gameMusic;
    }
    
    /// <summary>
    /// Verifica si la música del menú está sonando
    /// </summary>
    public bool IsMenuMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying && musicSource.clip == mainMenuMusic;
    }
    
    /// <summary>
    /// Detiene inmediatamente todos los efectos de audio
    /// </summary>
    public void StopAllSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }
    
    /// <summary>
    /// Detiene inmediatamente la música y efectos (para reiniciar)
    /// </summary>
    public void StopAllAudio()
    {
        StopMusic();
        StopAllSFX();
    }
    
    /// <summary>
    /// Reinicia el audio para una nueva partida
    /// </summary>
    public void RestartAudio()
    {
        // Detener todo el audio actual
        StopAllAudio();
        
        // Iniciar música del juego
        PlayGameMusic();
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