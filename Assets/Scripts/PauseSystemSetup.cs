using UnityEngine;
using UnityEngine.UI;

public class PauseSystemSetup : MonoBehaviour
{
    [Header("Pause System Prefabs")]
    public GameObject pauseSystemPrefab;
    public GameObject pausePanelPrefab;
    
    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    
    private GameObject pauseSystemInstance;
    private GameObject pausePanelInstance;
    
    void Start()
    {
        if (autoSetupOnStart)
        {
            SetupPauseSystem();
        }
    }
    
    /// <summary>
    /// Configura automáticamente el sistema de pausa
    /// </summary>
    public void SetupPauseSystem()
    {
        // Check if pause system already exists
        if (FindObjectOfType<PauseButton>() != null)
        {
            Debug.Log("PauseSystemSetup: Pause system already exists, skipping setup");
            return;
        }
        
        Debug.Log("PauseSystemSetup: Setting up pause system...");
        
        // Create pause system
        if (pauseSystemPrefab != null)
        {
            pauseSystemInstance = Instantiate(pauseSystemPrefab);
            pauseSystemInstance.name = "PauseSystem";
        }
        else
        {
            CreatePauseSystemFromScratch();
        }
        
        // Create pause panel
        if (pausePanelPrefab != null)
        {
            pausePanelInstance = Instantiate(pausePanelPrefab);
            pausePanelInstance.name = "PausePanel";
        }
        else
        {
            CreatePausePanelFromScratch();
        }
        
        // Setup references
        SetupReferences();
        
        Debug.Log("PauseSystemSetup: Pause system setup complete!");
    }
    
    /// <summary>
    /// Crea el sistema de pausa desde cero
    /// </summary>
    private void CreatePauseSystemFromScratch()
    {
        pauseSystemInstance = new GameObject("PauseSystem");
        PauseButton pauseButton = pauseSystemInstance.AddComponent<PauseButton>();
        
        // Auto-find components
        pauseButton.player = FindObjectOfType<PlayerController2D>();
        pauseButton.spawner = FindObjectOfType<Spawner2D>();
        
        Debug.Log("PauseSystemSetup: Created PauseSystem from scratch");
    }
    
    /// <summary>
    /// Crea el panel de pausa desde cero
    /// </summary>
    private void CreatePausePanelFromScratch()
    {
        // Find Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("PauseSystemSetup: No Canvas found in scene!");
            return;
        }
        
        // Create panel
        pausePanelInstance = new GameObject("PausePanel");
        pausePanelInstance.transform.SetParent(canvas.transform, false);
        
        // Add components
        Image panelImage = pausePanelInstance.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        
        RectTransform panelRect = pausePanelInstance.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Add PausePanel script
        PausePanel pausePanel = pausePanelInstance.AddComponent<PausePanel>();
        
        // Create title
        CreatePauseTitle(pausePanelInstance);
        
        // Create buttons
        CreatePauseButtons(pausePanelInstance, pausePanel);
        
        Debug.Log("PauseSystemSetup: Created PausePanel from scratch");
    }
    
    /// <summary>
    /// Crea el título del panel de pausa
    /// </summary>
    private void CreatePauseTitle(GameObject parent)
    {
        GameObject titleObj = new GameObject("PauseTitle");
        titleObj.transform.SetParent(parent.transform, false);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "JUEGO PAUSADO";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 48;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleCenter;
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.8f);
        titleRect.anchorMax = new Vector2(0.5f, 0.9f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;
    }
    
    /// <summary>
    /// Crea los botones del panel de pausa
    /// </summary>
    private void CreatePauseButtons(GameObject parent, PausePanel pausePanel)
    {
        // Resume button
        GameObject resumeButton = CreateButton("ResumeButton", "REANUDAR", parent);
        pausePanel.resumeButton = resumeButton.GetComponent<Button>();
        
        RectTransform resumeRect = resumeButton.GetComponent<RectTransform>();
        resumeRect.anchorMin = new Vector2(0.5f, 0.4f);
        resumeRect.anchorMax = new Vector2(0.5f, 0.5f);
        resumeRect.offsetMin = new Vector2(-100, -30);
        resumeRect.offsetMax = new Vector2(100, 30);
        
        // Quit button
        GameObject quitButton = CreateButton("QuitButton", "SALIR", parent);
        pausePanel.quitButton = quitButton.GetComponent<Button>();
        
        RectTransform quitRect = quitButton.GetComponent<RectTransform>();
        quitRect.anchorMin = new Vector2(0.5f, 0.25f);
        quitRect.anchorMax = new Vector2(0.5f, 0.35f);
        quitRect.offsetMin = new Vector2(-100, -30);
        quitRect.offsetMax = new Vector2(100, 30);
    }
    
    /// <summary>
    /// Crea un botón básico
    /// </summary>
    private GameObject CreateButton(string name, string text, GameObject parent)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent.transform, false);
        
        // Add Button component
        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.2f, 0.8f, 1f);
        
        // Add Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = text;
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.fontSize = 32;
        buttonText.color = Color.white;
        buttonText.alignment = TextAnchor.MiddleCenter;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        return buttonObj;
    }
    
    /// <summary>
    /// Configura las referencias entre componentes
    /// </summary>
    private void SetupReferences()
    {
        if (pauseSystemInstance != null && pausePanelInstance != null)
        {
            PauseButton pauseButton = pauseSystemInstance.GetComponent<PauseButton>();
            PausePanel pausePanel = pausePanelInstance.GetComponent<PausePanel>();
            
            if (pauseButton != null && pausePanel != null)
            {
                // Connect pause panel
                pauseButton.pausePanel = pausePanelInstance;
                
                // Connect buttons
                pauseButton.resumeButton = pausePanel.resumeButton;
                pauseButton.quitButton = pausePanel.quitButton;
                
                // Connect pause button (if exists in scene)
                Button[] sceneButtons = FindObjectsOfType<Button>();
                foreach (Button button in sceneButtons)
                {
                    if (button.name.Contains("Pause") || button.name.Contains("pause"))
                    {
                        pauseButton.pauseButton = button;
                        break;
                    }
                }
                
                Debug.Log("PauseSystemSetup: References configured successfully");
            }
        }
    }
}

