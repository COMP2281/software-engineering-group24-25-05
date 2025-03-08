using UnityEngine;

public class ToggleScrollView : MonoBehaviour
{
    [SerializeField] private GameObject scrollView; // Drag the Scroll View here in Inspector

    void Update()
    {
        if (scrollView != null && Input.GetKeyDown(KeyCode.R))
        {
            scrollView.SetActive(!scrollView.activeSelf);
        }
    }
}
