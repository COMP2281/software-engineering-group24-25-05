using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleter : MonoBehaviour
{
    [SerializeField] private LevelManager.LevelType thisLevelType;
    [SerializeField] private LevelManager.LevelType levelToUnlock;
    [SerializeField] private GameObject levelCompleteUI; // Reference to UI panel showing "Level Complete"
    [SerializeField] private float delayBeforeMainMenu = 2f; // Time to wait before loading main menu
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Scene name of the main menu
    
    void Start()
    {
        // Ensure the level complete UI is hidden at the start
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
    }
    
    // Call this method when the player completes the level
    public void CompleteLevel()
    {
        if (LevelManager.Instance != null)
        {
            // Unlock the next level
            LevelManager.Instance.UnlockLevel(levelToUnlock);
            
            // Show level complete UI
            if (levelCompleteUI != null)
            {
                levelCompleteUI.SetActive(true);
            }
            
            Debug.Log($"Completed {thisLevelType}. Unlocked {levelToUnlock}!");
            
            // Start coroutine to wait before returning to main menu
            StartCoroutine(ReturnToMainMenu());
        }
    }
    
    private IEnumerator ReturnToMainMenu()
    {
        // Wait for specified delay
        yield return new WaitForSeconds(delayBeforeMainMenu);
        
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}