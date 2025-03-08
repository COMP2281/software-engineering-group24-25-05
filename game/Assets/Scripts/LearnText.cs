using UnityEngine;

public class ToggleScrollView : MonoBehaviour
{
    [SerializeField] private GameObject scrollView;
    private AudioSource audioSource;

    private void Start()
    {
        // Disable scroll view on start
        scrollView.SetActive(false);

        // Get the AudioSource from this GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing! Make sure to add one to this GameObject.");
        }
    }

    void Update()
    {
        if (scrollView != null && Input.GetKeyDown(KeyCode.R))
        {
            scrollView.SetActive(!scrollView.activeSelf);

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
