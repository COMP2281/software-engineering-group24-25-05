using UnityEngine;
using TMPro;

public class ToggleScrollView : MonoBehaviour
{
    [SerializeField] private GameObject scrollView;        // Your scroll view UI element
    [SerializeField] private Transform player;             // Player's Transform
    [SerializeField] private Transform targetObject;       // The object to get close to
    [SerializeField] private TextMeshProUGUI promptText;   // TMP UI text (prompt message)
    [SerializeField] private float interactionDistance = 3f;

    private AudioSource audioSource;
    private bool isInRange = false;

    private void Start()
    {
        scrollView.SetActive(false);             // Make sure scroll view is off at the start
        promptText.gameObject.SetActive(false);  // Hide the prompt at the start

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing! Make sure to add one to this GameObject.");
        }
    }

    private void Update()
    {
        // Check the distance between the player and the target object
        float distance = Vector3.Distance(player.position, targetObject.position);

        if (distance <= interactionDistance)
        {
            if (!isInRange)
            {
                isInRange = true;
                promptText.gameObject.SetActive(true);  // Show the prompt when in range
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleScrollViewVisibility();
                promptText.gameObject.SetActive(false);  // Hide the prompt text when R is pressed
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                promptText.gameObject.SetActive(false);  // Hide the prompt when out of range

                // Auto-close the scroll view if it's open
                if (scrollView.activeSelf)
                {
                    scrollView.SetActive(false);
                }
            }
        }

        // If scroll view is hidden, show the prompt again
        if (!scrollView.activeSelf && isInRange)
        {
            promptText.gameObject.SetActive(true);  // Show the prompt when scroll view is closed and player is in range
        }
    }

    private void ToggleScrollViewVisibility()
    {
        // Toggle the visibility of the scroll view
        bool newState = !scrollView.activeSelf;
        scrollView.SetActive(newState);

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
