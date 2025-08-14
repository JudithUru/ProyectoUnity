using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Level Settings")]
    public int levelIndex = 1;
    public string levelName = "Level 1";
    
    [Header("Coin Goals")]
    public int targetCoins = 10;
    
    [Header("Spawn Settings")]
    public float collectibleSpawnInterval = 2f;
    public float obstacleSpawnInterval = 3f;
    public int maxObstaclesOnScreen = 15;
    
    [Header("Difficulty")]
    public float obstacleSpeed = 3f;
    public float collectibleSpeed = 2f;
    
    [Header("Visual")]
    public Color levelColor = Color.white;
    public string levelDescription = "Collect all coins to advance!";
}




