using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level1Setup : MonoBehaviour
{
    [Header("Setup")]
    public bool setupOnStart = true;
    
    void Start()
    {
        if (setupOnStart)
        {
            SetupLevel1();
        }
    }
    
    /// <summary>
    /// Configura automáticamente toda la escena Level1
    /// </summary>
    public void SetupLevel1()
    {
        Debug.Log("Level1Setup: Configurando escena Level1...");
        
        // Crear Canvas si no existe
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Debug.Log("Level1Setup: Canvas creado");
        }
        
        // Crear EventSystem si no existe
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Debug.Log("Level1Setup: EventSystem creado");
        }
        
        // Crear Main Camera si no existe
        Camera mainCamera = FindObjectOfType<Camera>();
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5f;
            cameraObj.tag = "MainCamera";
            Debug.Log("Level1Setup: Main Camera creada");
        }
        
        // Crear Player si no existe
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            player = CreatePlayer();
            Debug.Log("Level1Setup: Player creado");
        }
        
        // Crear LevelSetup (Spawner + HUD)
        GameObject levelSetup = GameObject.Find("LevelSetup");
        if (levelSetup == null)
        {
            levelSetup = CreateLevelSetup();
            Debug.Log("Level1Setup: LevelSetup creado");
        }
        
        // Crear LevelController
        GameObject levelController = GameObject.Find("LevelController");
        if (levelController == null)
        {
            levelController = CreateLevelController();
            Debug.Log("Level1Setup: LevelController creado");
        }
        
        // Crear HUD
        GameObject hud = GameObject.Find("HUD");
        if (hud == null)
        {
            hud = CreateHUD(canvas.gameObject);
            Debug.Log("Level1Setup: HUD creado");
        }
        
        // Crear Game Over Panel
        GameObject gameOverPanel = GameObject.Find("GameOverPanel");
        if (gameOverPanel == null)
        {
            gameOverPanel = CreateGameOverPanel(canvas.gameObject);
            Debug.Log("Level1Setup: GameOverPanel creado");
        }
        
        // Crear Victory Panel
        GameObject victoryPanel = GameObject.Find("VictoryPanel");
        if (victoryPanel == null)
        {
            victoryPanel = CreateVictoryPanel(canvas.gameObject);
            Debug.Log("Level1Setup: VictoryPanel creado");
        }
        
        Debug.Log("Level1Setup: Escena Level1 configurada completamente!");
    }
    
    /// <summary>
    /// Crea el jugador
    /// </summary>
    private GameObject CreatePlayer()
    {
        GameObject player = new GameObject("Player");
        
        // Agregar SpriteRenderer
        SpriteRenderer spriteRenderer = player.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateDefaultSprite();
        spriteRenderer.color = Color.blue;
        
        // Agregar Rigidbody2D
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Agregar Collider2D
        CircleCollider2D collider = player.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;
        
        // Agregar PlayerController
        player.AddComponent<PlayerController2D>();
        
        // Posicionar en el centro
        player.transform.position = Vector3.zero;
        player.tag = "Player";
        
        return player;
    }
    
    /// <summary>
    /// Crea un sprite por defecto
    /// </summary>
    private Sprite CreateDefaultSprite()
    {
        // Crear una textura 32x32 azul
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.blue;
        }
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
    }
    
    /// <summary>
    /// Crea el LevelSetup con Spawner y HUD
    /// </summary>
    private GameObject CreateLevelSetup()
    {
        GameObject levelSetup = new GameObject("LevelSetup");
        
        // Agregar Spawner2D
        Spawner2D spawner = levelSetup.AddComponent<Spawner2D>();
        
        // Configurar Spawner2D
        spawner.collectibleSpawnInterval = 2f;
        spawner.obstacleSpawnInterval = 3f;
        spawner.maxObstaclesOnScreen = 15;
        spawner.targetCoins = 10;
        spawner.spawnMargin = 1f;
        spawner.obstacleSpeed = 3f;
        spawner.collectibleSpeed = 2f;
        
        // Asignar prefabs si existen
        GameObject[] collectiblePrefabs = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject[] obstaclePrefabs = Resources.FindObjectsOfTypeAll<GameObject>();
        
        // Filtrar por tags
        System.Collections.Generic.List<GameObject> collectibles = new System.Collections.Generic.List<GameObject>();
        System.Collections.Generic.List<GameObject> obstacles = new System.Collections.Generic.List<GameObject>();
        
        foreach (GameObject obj in collectiblePrefabs)
        {
            if (obj.CompareTag("Collectible") && obj != levelSetup)
                collectibles.Add(obj);
        }
        
        foreach (GameObject obj in obstaclePrefabs)
        {
            if (obj.CompareTag("Obstacle") && obj != levelSetup)
                obstacles.Add(obj);
        }
        
        if (collectibles.Count > 0)
            spawner.collectiblePrefabs = collectibles.ToArray();
        
        if (obstacles.Count > 0)
            spawner.obstaclePrefabs = obstacles.ToArray();
        
        return levelSetup;
    }
    
    /// <summary>
    /// Crea el LevelController
    /// </summary>
    private GameObject CreateLevelController()
    {
        GameObject levelController = new GameObject("LevelController");
        
        // Agregar LevelController script
        LevelController controller = levelController.AddComponent<LevelController>();
        controller.levelIndex = 1;
        
        // Asignar referencias
        controller.spawner = FindObjectOfType<Spawner2D>();
        controller.hudController = FindObjectOfType<HUDController>();
        
        return levelController;
    }
    
    /// <summary>
    /// Crea el HUD
    /// </summary>
    private GameObject CreateHUD(GameObject canvas)
    {
        GameObject hud = new GameObject("HUD");
        hud.transform.SetParent(canvas.transform, false);
        
        // Agregar HUDController
        HUDController hudController = hud.AddComponent<HUDController>();
        
        // Crear elementos del HUD
        GameObject coinsText = CreateText(canvas, "CoinsText", "Coins: 0/10", 24, new Vector2(-200, 200));
        GameObject livesText = CreateText(canvas, "LivesText", "Lives: 3", 24, new Vector2(200, 200));
        GameObject levelText = CreateText(canvas, "LevelText", "Level 1", 36, new Vector2(0, 250));
        GameObject targetText = CreateText(canvas, "TargetText", "Target: 10", 20, new Vector2(0, -200));
        
        // Asignar referencias al HUDController
        hudController.coinsText = coinsText.GetComponent<TextMeshProUGUI>();
        hudController.livesText = livesText.GetComponent<TextMeshProUGUI>();
        hudController.levelText = levelText.GetComponent<TextMeshProUGUI>();
        hudController.targetText = targetText.GetComponent<TextMeshProUGUI>();
        
        // HUD configurado sin paneles adicionales
        
        return hud;
    }
    
    /// <summary>
    /// Crea el Game Over Panel
    /// </summary>
    private GameObject CreateGameOverPanel(GameObject canvas)
    {
        GameObject gameOverPanel = CreateUIPanel(canvas, "GameOverPanel");
        gameOverPanel.SetActive(false);
        
        // Crear texto de Game Over
        GameObject gameOverText = CreateText(canvas, "GameOverText", "GAME OVER", 72, new Vector2(0, 100));
        gameOverText.transform.SetParent(gameOverPanel.transform, false);
        
        // Crear botón de retry
        GameObject retryButton = CreateButton(canvas, "RetryButton", "REINTENTAR", 36, new Vector2(0, 0));
        retryButton.transform.SetParent(gameOverPanel.transform, false);
        
        // Crear botón de main menu
        GameObject mainMenuButton = CreateButton(canvas, "MainMenuButton", "MENU PRINCIPAL", 36, new Vector2(0, -80));
        mainMenuButton.transform.SetParent(gameOverPanel.transform, false);
        
        // Crear GameOverUI
        GameObject gameOverUI = new GameObject("GameOverUI");
        gameOverUI.transform.SetParent(gameOverPanel.transform, false);
        GameOverUI uiScript = gameOverUI.AddComponent<GameOverUI>();
        
        // Configurar referencias
        uiScript.retryButton = retryButton.GetComponent<Button>();
        uiScript.mainMenuButton = mainMenuButton.GetComponent<Button>();
        uiScript.gameOverText = gameOverText.GetComponent<TextMeshProUGUI>();
        
        return gameOverPanel;
    }
    
    /// <summary>
    /// Crea el Victory Panel
    /// </summary>
    private GameObject CreateVictoryPanel(GameObject canvas)
    {
        GameObject victoryPanel = CreateUIPanel(canvas, "VictoryPanel");
        victoryPanel.SetActive(false);
        
        // Crear texto de victoria
        GameObject victoryText = CreateText(canvas, "VictoryText", "¡VICTORIA!", 72, new Vector2(0, 100));
        victoryText.transform.SetParent(victoryPanel.transform, false);
        
        // Crear botón de main menu
        GameObject mainMenuButton = CreateButton(canvas, "VictoryMainMenuButton", "MENU PRINCIPAL", 36, new Vector2(0, 0));
        mainMenuButton.transform.SetParent(victoryPanel.transform, false);
        
        // Crear botón de play again
        GameObject playAgainButton = CreateButton(canvas, "PlayAgainButton", "JUGAR DE NUEVO", 36, new Vector2(0, -80));
        playAgainButton.transform.SetParent(victoryPanel.transform, false);
        
        // Crear VictoryUI
        GameObject victoryUI = new GameObject("VictoryUI");
        victoryUI.transform.SetParent(victoryPanel.transform, false);
        VictoryUI uiScript = victoryUI.AddComponent<VictoryUI>();
        
        // Configurar referencias
        uiScript.mainMenuButton = mainMenuButton.GetComponent<Button>();
        uiScript.playAgainButton = playAgainButton.GetComponent<Button>();
        uiScript.victoryText = victoryText.GetComponent<TextMeshProUGUI>();
        
        return victoryPanel;
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
