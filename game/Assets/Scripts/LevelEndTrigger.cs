using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name); // Log the object entering the trigger

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the level end trigger."); // Log the player entering the trigger

            LevelCompleter levelCompleter = GetComponent<LevelCompleter>();
            if (levelCompleter != null)
            {
                levelCompleter.CompleteLevel();
            }
            else
            {
                Debug.LogWarning("LevelCompleter component not found on LevelEndTrigger GameObject.");
            }
        }
    }
}
