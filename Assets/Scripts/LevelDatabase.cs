using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/Level Database")]
public class LevelDatabase : ScriptableObject
{
    [Header("Level Configurations")]
    public LevelConfig[] levels = new LevelConfig[3];
    
    [Header("Default Settings")]
    public int startingLives = 3;
    public string[] sceneNames = { "Level1", "Level2", "Level3" };
    
    /// <summary>
    /// Obtiene la configuración de un nivel específico
    /// </summary>
    public LevelConfig GetLevelConfig(int levelIndex)
    {
        if (levelIndex < 1 || levelIndex > levels.Length)
        {
            Debug.LogWarning($"Level index {levelIndex} out of range. Returning null.");
            return null;
        }
        
        return levels[levelIndex - 1];
    }
    
    /// <summary>
    /// Obtiene el nombre de la escena para un nivel
    /// </summary>
    public string GetSceneName(int levelIndex)
    {
        if (levelIndex < 1 || levelIndex > sceneNames.Length)
        {
            Debug.LogWarning($"Level index {levelIndex} out of range. Returning Level1.");
            return "Level1";
        }
        
        return sceneNames[levelIndex - 1];
    }
    
    /// <summary>
    /// Verifica si un nivel existe
    /// </summary>
    public bool LevelExists(int levelIndex)
    {
        return levelIndex >= 1 && levelIndex <= levels.Length;
    }
    
    /// <summary>
    /// Obtiene el número total de niveles
    /// </summary>
    public int GetTotalLevels()
    {
        return levels.Length;
    }
}




