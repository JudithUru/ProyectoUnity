using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTransitionPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI descriptionText;
    
    [Header("Animation")]
    public float showDuration = 3f;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 0.5f;
    
    [Header("Components")]
    public CanvasGroup canvasGroup;
    
    // Private variables for animation
    private bool isAnimating = false;
    private float animationTimer = 0f;
    private AnimationState currentState = AnimationState.None;
    
    private enum AnimationState
    {
        None,
        FadingIn,
        Showing,
        FadingOut
    }
    
    private void Awake()
    {
        // Get or create CanvasGroup for fade effects
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        // Start hidden
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (!isAnimating) return;
        
        animationTimer += Time.deltaTime;
        
        switch (currentState)
        {
            case AnimationState.FadingIn:
                HandleFadeIn();
                break;
            case AnimationState.Showing:
                HandleShowing();
                break;
            case AnimationState.FadingOut:
                HandleFadeOut();
                break;
        }
    }
    
    /// <summary>
    /// Muestra el panel de transición para un nivel específico
    /// </summary>
    public void ShowLevelTransition(int levelNumber)
    {
        Debug.Log($"LevelTransitionPanel: Showing transition for Level {levelNumber}");
        
        // Set level text
        if (levelText != null)
        {
            levelText.text = $"NIVEL {levelNumber}";
        }
        
        // Set description text
        if (descriptionText != null)
        {
            descriptionText.text = "¡Preparate para el siguiente desafío!";
        }
        
        // Show panel and start animation
        gameObject.SetActive(true);
        StartAnimation();
    }
    
    /// <summary>
    /// Inicia la animación del panel
    /// </summary>
    private void StartAnimation()
    {
        isAnimating = true;
        animationTimer = 0f;
        currentState = AnimationState.FadingIn;
        canvasGroup.alpha = 0f;
    }
    
    /// <summary>
    /// Maneja el fade in
    /// </summary>
    private void HandleFadeIn()
    {
        float progress = animationTimer / fadeInTime;
        canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
        
        if (progress >= 1f)
        {
            currentState = AnimationState.Showing;
            animationTimer = 0f;
        }
    }
    
    /// <summary>
    /// Maneja el tiempo de visualización
    /// </summary>
    private void HandleShowing()
    {
        if (animationTimer >= showDuration)
        {
            currentState = AnimationState.FadingOut;
            animationTimer = 0f;
        }
    }
    
    /// <summary>
    /// Maneja el fade out
    /// </summary>
    private void HandleFadeOut()
    {
        float progress = animationTimer / fadeOutTime;
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
        
        if (progress >= 1f)
        {
            // Animation complete
            isAnimating = false;
            currentState = AnimationState.None;
            gameObject.SetActive(false);
            Debug.Log("LevelTransitionPanel: Transition sequence completed");
        }
    }
    
    /// <summary>
    /// Establece el texto del nivel
    /// </summary>
    public void SetLevelText(string text)
    {
        if (levelText != null)
        {
            levelText.text = text;
        }
    }
    
    /// <summary>
    /// Establece el texto de descripción
    /// </summary>
    public void SetDescriptionText(string text)
    {
        if (descriptionText != null)
        {
            descriptionText.text = text;
        }
    }
    
    /// <summary>
    /// Establece la duración de visualización
    /// </summary>
    public void SetShowDuration(float duration)
    {
        showDuration = duration;
    }
}
