using UnityEngine;
using System.Collections;

public class Spawner2D : MonoBehaviour
{
    [Header("Spawn Objects")]
    public GameObject[] collectiblePrefabs;
    public GameObject[] obstaclePrefabs;
    
    [Header("Spawn Settings")]
    public float spawnMargin = 1f;
    public float collectibleSpawnInterval = 2f;
    public float obstacleSpawnInterval = 3f;
    public int maxObstaclesOnScreen = 15;
    public int targetCoins = 10;
    
    [Header("Movement Settings")]
    public float obstacleSpeed = 3f;
    public float collectibleSpeed = 2f;
    
    [Header("Components")]
    public GameManager gameManager;
    
    // Private variables
    private Coroutine collectibleSpawnCoroutine;
    private Coroutine obstacleSpawnCoroutine;
    private bool isSpawning = false;
    private Vector2 screenBounds;
    private Camera mainCamera;
    private int coinsSpawned = 0;
    
    void Start()
    {
        Debug.Log("Spawner2D: Start() called");
        
        // Get main camera and calculate screen bounds
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
            Debug.Log("Spawner2D: Screen bounds calculated - Width: " + screenBounds.x + ", Height: " + screenBounds.y);
        }
        
        // Get GameManager if not assigned
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            Debug.Log("Spawner2D: GameManager found via Instance: " + (gameManager != null));
        }
        else
        {
            Debug.Log("Spawner2D: GameManager assigned via Inspector: " + (gameManager != null));
        }
        
        // Subscribe to game events
        if (gameManager != null)
        {
            gameManager.OnGameStart += StartSpawning;
            gameManager.OnGameOver += StopSpawning;
            gameManager.OnGameWin += StopSpawning;
            gameManager.OnLevelComplete += StopSpawning;
            Debug.Log("Spawner2D: Subscribed to GameManager events");
        }
        else
        {
            Debug.LogWarning("Spawner2D: GameManager is null, cannot subscribe to events!");
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        if (gameManager != null)
        {
            gameManager.OnGameStart -= StartSpawning;
            gameManager.OnGameOver -= StopSpawning;
            gameManager.OnGameWin -= StopSpawning;
            gameManager.OnLevelComplete -= StopSpawning;
        }
    }
    
    /// <summary>
    /// Inicia el spawning de objetos
    /// </summary>
    public void StartSpawning()
    {
        Debug.Log("Spawner2D: StartSpawning() called");
        
        if (isSpawning) 
        {
            Debug.Log("Spawner2D: Already spawning, returning");
            return;
        }
        
        isSpawning = true;
        coinsSpawned = 0;
        Debug.Log("Spawner2D: Starting spawn coroutines");
        
        // Start spawn coroutines
        collectibleSpawnCoroutine = StartCoroutine(SpawnCollectibles());
        obstacleSpawnCoroutine = StartCoroutine(SpawnObstacles());
        
        Debug.Log("Spawner2D: Spawn coroutines started");
    }
    
    /// <summary>
    /// Detiene el spawning de objetos
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        
        // Stop spawn coroutines
        if (collectibleSpawnCoroutine != null)
        {
            StopCoroutine(collectibleSpawnCoroutine);
            collectibleSpawnCoroutine = null;
        }
        
        if (obstacleSpawnCoroutine != null)
        {
            StopCoroutine(obstacleSpawnCoroutine);
            obstacleSpawnCoroutine = null;
        }
        
        Debug.Log("Spawner2D: Spawning stopped");
    }
    
    /// <summary>
    /// Corrutina para spawnear collectibles
    /// </summary>
    IEnumerator SpawnCollectibles()
    {
        while (isSpawning)
        {
            // Check if we can spawn more collectibles
            if (coinsSpawned < targetCoins)
            {
                SpawnCollectible();
                coinsSpawned++;
                Debug.Log($"Spawner2D: Collectible spawned ({coinsSpawned}/{targetCoins})");
            }
            else
            {
                Debug.Log("Spawner2D: Target coins reached, stopping collectible spawning");
                break;
            }
            
            // Wait for next spawn
            yield return new WaitForSeconds(collectibleSpawnInterval);
        }
    }
    
    /// <summary>
    /// Corrutina para spawnear obstáculos
    /// </summary>
    IEnumerator SpawnObstacles()
    {
        while (isSpawning)
        {
            // Check if we can spawn more obstacles
            if (CountObjectsWithTag("Obstacle") < maxObstaclesOnScreen)
            {
                SpawnObstacle();
            }
            
            // Wait for next spawn
            yield return new WaitForSeconds(obstacleSpawnInterval);
        }
    }
    
    /// <summary>
    /// Spawnea un collectible
    /// </summary>
    void SpawnCollectible()
    {
        if (collectiblePrefabs.Length == 0) return;
        
        // Choose random collectible
        GameObject collectiblePrefab = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];
        
        // Calculate spawn position within playable area
        Vector3 spawnPosition = GetRandomSpawnPositionInPlayArea();
        
        Debug.Log("Spawner2D: Spawning collectible at position: " + spawnPosition);
        
        // Instantiate collectible
        GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
        
        // Add Coin component if not present
        if (collectible.GetComponent<Coin>() == null)
        {
            collectible.AddComponent<Coin>();
        }
        
        // Add movement component if needed
        if (collectibleSpeed > 0)
        {
            AddMovementComponent(collectible, collectibleSpeed);
        }
    }
    
    /// <summary>
    /// Spawnea un obstáculo
    /// </summary>
    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;
        
        // Choose random obstacle
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        
        // Calculate spawn position within playable area
        Vector3 spawnPosition = GetRandomSpawnPositionInPlayArea();
        
        Debug.Log("Spawner2D: Spawning obstacle at position: " + spawnPosition);
        
        // Instantiate obstacle
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        
        // Add Obstacle component if not present
        if (obstacle.GetComponent<Obstacle>() == null)
        {
            obstacle.AddComponent<Obstacle>();
        }
        
        // Add movement component if needed
        if (obstacleSpeed > 0)
        {
            AddMovementComponent(obstacle, obstacleSpeed);
        }
    }
    
    /// <summary>
    /// Calcula una posición de spawn aleatoria dentro del área jugable
    /// </summary>
    Vector3 GetRandomSpawnPositionInPlayArea()
    {
        // Calcular área de spawn dentro de los límites de la pantalla
        float minX = -screenBounds.x + spawnMargin;
        float maxX = screenBounds.x - spawnMargin;
        float minY = -screenBounds.y + spawnMargin;
        float maxY = screenBounds.y - spawnMargin;
        
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        
        Vector3 position = new Vector3(randomX, randomY, 0f);
        
        Debug.Log("Spawner2D: Random spawn position in play area - X: " + randomX + ", Y: " + randomY);
        
        return position;
    }
    
    /// <summary>
    /// Agrega componente de movimiento a un objeto
    /// </summary>
    void AddMovementComponent(GameObject obj, float speed)
    {
        // Add or get ObjectMover component
        ObjectMover mover = obj.GetComponent<ObjectMover>();
        if (mover == null)
            mover = obj.AddComponent<ObjectMover>();
        
        mover.speed = speed;
    }
    
    /// <summary>
    /// Cuenta objetos con un tag específico
    /// </summary>
    int CountObjectsWithTag(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag).Length;
    }
    
    /// <summary>
    /// Limpia todos los objetos de la escena
    /// </summary>
    public void ClearAllObjects()
    {
        // Destroy all collectibles and obstacles
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        
        foreach (GameObject obj in collectibles)
        {
            Destroy(obj);
        }
        
        foreach (GameObject obj in obstacles)
        {
            Destroy(obj);
        }
        
        Debug.Log("Spawner2D: All objects cleared");
    }
    
    /// <summary>
    /// Obtiene el número de monedas spawneadas
    /// </summary>
    public int GetCoinsSpawned()
    {
        return coinsSpawned;
    }
    
    /// <summary>
    /// Obtiene el número de monedas restantes por spawnear
    /// </summary>
    public int GetRemainingCoinsToSpawn()
    {
        return targetCoins - coinsSpawned;
    }
}


