using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuSetup : MonoBehaviour
{
    [Header("Setup")]
    public bool setupOnStart = true;
    
    void Start()
    {
        if (setupOnStart)
        {
            SetupMainMenu();
        }
    }
    
    /// <summary>
    /// Configura automáticamente toda la escena MainMenu
    /// </summary>
    public void SetupMainMenu()
    {
        Debug.Log("MainMenuSetup: Configurando escena MainMenu...");
        
        // Crear Canvas si no existe
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Debug.Log("MainMenuSetup: Canvas creado");
        }
        
        // Crear EventSystem si no existe
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Debug.Log("MainMenuSetup: EventSystem creado");
        }
        
        // Crear MainMenuPanel
        GameObject mainMenuPanel = GameObject.Find("MainMenuPanel");
        if (mainMenuPanel == null)
        {
            mainMenuPanel = CreateUIPanel(canvas.gameObject, "MainMenuPanel");
            Debug.Log("MainMenuSetup: MainMenuPanel creado");
        }
        
        // Crear TitleText
        GameObject titleText = GameObject.Find("TitleText");
        if (titleText == null)
        {
            titleText = CreateText(canvas.gameObject, "TitleText", "MI JUEGO 2D", 72, new Vector2(0, 200));
            Debug.Log("MainMenuSetup: TitleText creado");
        }
        
        // Crear PlayButton
        GameObject playButton = GameObject.Find("PlayButton");
        if (playButton == null)
        {
            playButton = CreateButton(canvas.gameObject, "PlayButton", "JUGAR", 48, new Vector2(0, 0));
            Debug.Log("MainMenuSetup: PlayButton creado");
        }
        
        // Crear QuitButton
        GameObject quitButton = GameObject.Find("QuitButton");
        if (quitButton == null)
        {
            quitButton = CreateButton(canvas.gameObject, "QuitButton", "SALIR", 48, new Vector2(0, -100));
            Debug.Log("MainMenuSetup: QuitButton creado");
        }
        
        // Crear MainMenuUI
        GameObject mainMenuUI = GameObject.Find("MainMenuUI");
        if (mainMenuUI == null)
        {
            mainMenuUI = new GameObject("MainMenuUI");
            MainMenuUI uiScript = mainMenuUI.AddComponent<MainMenuUI>();
            
            // Configurar referencias
            uiScript.playButton = playButton.GetComponent<Button>();
            uiScript.quitButton = quitButton.GetComponent<Button>();
            uiScript.titleText = titleText.GetComponent<TextMeshProUGUI>();
            
            Debug.Log("MainMenuSetup: MainMenuUI creado y configurado");
        }
        
        Debug.Log("MainMenuSetup: Escena MainMenu configurada completamente!");
    }
    
    /// <summary>
    /// Crea un panel UI
    /// </summary>
    private GameObject CreateUIPanel(GameObject parent, string name)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent.transform, false);
        
        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.1f, 0.1f, 0.3f, 0.8f);
        
        return panel;
    }
    
    /// <summary>
    /// Crea un texto UI
    /// </summary>
    private GameObject CreateText(GameObject parent, string name, string text, int fontSize, Vector2 position)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);
        
        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(400, 100);
        
        TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.fontSize = fontSize;
        tmpText.color = Color.white;
        tmpText.alignment = TextAlignmentOptions.Center;
        
        return textObj;
    }
    
    /// <summary>
    /// Crea un botón UI
    /// </summary>
    private GameObject CreateButton(GameObject parent, string name, string text, int fontSize, Vector2 position)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent.transform, false);
        
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(200, 60);
        
        Image image = buttonObj.AddComponent<Image>();
        image.color = Color.white;
        
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = image;
        
        // Crear texto del botón
        GameObject buttonText = new GameObject("Text");
        buttonText.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRectTransform = buttonText.AddComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.offsetMin = Vector2.zero;
        textRectTransform.offsetMax = Vector2.zero;
        
        TextMeshProUGUI tmpText = buttonText.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.fontSize = fontSize;
        tmpText.color = Color.black;
        tmpText.alignment = TextAlignmentOptions.Center;
        
        return buttonObj;
    }
}


