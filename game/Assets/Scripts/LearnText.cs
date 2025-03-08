using UnityEngine;
//using UnityEngine.UI; // If you're using Text
using TMPro; // Uncomment this line if you're using TextMeshPro

public class ToggleTextVisibility : MonoBehaviour
{
    public GameObject textToToggle; // Drag the Text GameObject here in the Inspector

    void Update()
    {
        // Check if the 'R' key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Toggle the active state of the text
            textToToggle.SetActive(!textToToggle.activeSelf);
        }
    }
}
