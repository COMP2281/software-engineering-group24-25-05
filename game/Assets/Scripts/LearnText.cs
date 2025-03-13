using UnityEngine;
using TMPro;

public class ToggleScrollView : MonoBehaviour
{
    [SerializeField] private GameObject scrollView;  // This is your scroll view UI element
    [SerializeField] private Transform player;        // Player's Transform
    [SerializeField] private Transform targetObject;  // Object to get close to
    [SerializeField] private TextMeshProUGUI promptText; // TMP UI text
    [SerializeField] private float interactionDistance = 3f;

    private AudioSource audioSource;
    private bool isInRange = false;

    private void Start()
    {
        scrollView.SetActive(false);           // Make sure scroll view is off at the start
        promptText.gameObject.SetActive(false);  // Hide the prompt at the start

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing! Make sure to add one to this GameObject.");
        }
    }

    private void Update()
    {
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
    }

    private void ToggleScrollViewVisibility()
    {
        bool newState = !scrollView.activeSelf;
        scrollView.SetActive(newState);

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
