using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton instance
    public static LevelManager Instance { get; private set; }
    
    // Define level types
    public enum LevelType
    {
        ArtificialIntelligence,
        DataAnalytics,
        CyberSecurity,
        Tutorial
    }
    
    // Dictionary to store level unlock status
    private Dictionary<LevelType, bool> levelUnlockStatus = new Dictionary<LevelType, bool>();
    
    // Scene names corresponding to each level type
    private Dictionary<LevelType, string> levelSceneNames = new Dictionary<LevelType, string>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLevelData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeLevelData()
    {
        // Set up scene names for each level type
        levelSceneNames[LevelType.Tutorial] = "TutorialScene";
        levelSceneNames[LevelType.ArtificialIntelligence] = "SampleScene";
        levelSceneNames[LevelType.DataAnalytics] = "DataScene"; 
        levelSceneNames[LevelType.CyberSecurity] = "CyberScene";
        
        // By default, only AI is unlocked
        levelUnlockStatus[LevelType.Tutorial] = true; // Tutorial is always unlocked
        levelUnlockStatus[LevelType.ArtificialIntelligence] = false;
        levelUnlockStatus[LevelType.DataAnalytics] = false;
        levelUnlockStatus[LevelType.CyberSecurity] = false;
        
        // Load saved unlock status
        LoadLevelUnlockStatus();
    }
    
    // Check if a level is unlocked
    public bool IsLevelUnlocked(LevelType levelType)
    {
        return levelUnlockStatus.ContainsKey(levelType) && levelUnlockStatus[levelType];
    }
    
    // Unlock a specific level
    public void UnlockLevel(LevelType levelType)
    {
        levelUnlockStatus[levelType] = true;
        SaveLevelUnlockStatus();
    }
    
    // Get the scene name for a level type
    public string GetSceneName(LevelType levelType)
    {
        if (levelSceneNames.ContainsKey(levelType))
        {
            return levelSceneNames[levelType];
        }
        return "MainMenu"; // Default fallback
    }
    
    // Save unlock status to PlayerPrefs
    private void SaveLevelUnlockStatus()
    {
        foreach (var level in levelUnlockStatus)
        {
            PlayerPrefs.SetInt("Level_" + level.Key.ToString(), level.Value ? 1 : 0);
        }
        PlayerPrefs.Save();
    }
    
    // Load unlock status from PlayerPrefs
    private void LoadLevelUnlockStatus()
    {
        foreach (LevelType levelType in System.Enum.GetValues(typeof(LevelType)))
        {
            if (PlayerPrefs.HasKey("Level_" + levelType.ToString()))
            {
                levelUnlockStatus[levelType] = PlayerPrefs.GetInt("Level_" + levelType.ToString()) == 1;
            }
        }
    }
    
    // Reset all level unlock status (for testing)
    public void ResetLevelProgress()
    {
        levelUnlockStatus[LevelType.Tutorial] = true; // Tutorial is always unlocked
        levelUnlockStatus[LevelType.ArtificialIntelligence] = false;
        levelUnlockStatus[LevelType.DataAnalytics] = false;
        levelUnlockStatus[LevelType.CyberSecurity] = false;
        SaveLevelUnlockStatus();
    }
}