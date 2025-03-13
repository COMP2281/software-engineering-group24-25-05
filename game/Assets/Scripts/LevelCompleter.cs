using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleter : MonoBehaviour
{
    [SerializeField] private LevelManager.LevelType thisLevelType;
    [SerializeField] private LevelManager.LevelType levelToUnlock;
    
    // Call this method when the player completes the level
    public void CompleteLevel()
    {
        if (LevelManager.Instance != null)
        {
            // Unlock the next level
            LevelManager.Instance.UnlockLevel(levelToUnlock);
            
            // Show completion UI or navigate to next level
            Debug.Log($"Completed {thisLevelType}. Unlocked {levelToUnlock}!");
        }
    }
}