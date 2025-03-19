using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

public class ToggleScrollView : MonoBehaviour
{
    [SerializeField] private GameObject scrollView;        
    [SerializeField] private Transform player;             
    [SerializeField] private Transform targetObject;       
    [SerializeField] private float interactionDistance = 3f;  
    [SerializeField] private AudioSource audioSource;      

    private TextMeshProUGUI promptText;   
    private bool isInRange = false;
    private string interactBindingText = "Interact";

    private void Start()
    {
        // Try to find the prompt text (TextMeshProUGUI) under the targetObject (the sprite)
        promptText = targetObject.GetComponentInChildren<TextMeshProUGUI>();

        if (promptText == null)
        {
            Debug.LogError("Prompt Text is not found on the sprite! Make sure it is a child of the target object and has the TextMeshProUGUI component.");
        }
        else
        {
            Debug.Log("Prompt Text found: " + promptText.name);
        }

        scrollView.SetActive(false);             // Make sure scroll view is off at the start
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);  // Hide the prompt at the start
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  // Try to get AudioSource if not set via inspector
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is missing! Make sure to attach an AudioSource component to this GameObject.");
            }
        }

        // Get the current interact binding
        UpdateInteractBindingText();
    }

    private void Update()
    {
        // Check the distance between the player and the target object (the sprite)
        float distance = Vector3.Distance(player.position, targetObject.position);

        if (distance <= interactionDistance)  // Player is in range
        {
            if (!isInRange)
            {
                isInRange = true;
                if (promptText != null)
                {
                    promptText.gameObject.SetActive(true);  // Show the prompt when in range of the sprite
                    // Update the text to show the current binding
                    promptText.text = $"Press {interactBindingText} to read";
                }
            }

            // Use the Input System instead of direct key input
            if (UserInput.Instance.InteractPressed)  // Player presses the interact button
            {
                ToggleScrollViewVisibility();
                if (promptText != null)
                {
                    promptText.gameObject.SetActive(false);  // Hide the prompt text when interact is pressed
                }

                // Play the audio when interact is pressed
                PlayAudio();
            }
        }
        else  // Player is out of range
        {
            if (isInRange)
            {
                isInRange = false;
                if (promptText != null)
                {
                    promptText.gameObject.SetActive(false);  // Hide the prompt when out of range
                }
            }

            // Auto-close the scroll view if it's open when the player is out of range
            if (scrollView.activeSelf)
            {
                scrollView.SetActive(false);
            }
        }

        // If scroll view is hidden, show the prompt again if the player is in range
        if (!scrollView.activeSelf && isInRange)
        {
            if (promptText != null)
            {
                promptText.gameObject.SetActive(true);  // Show the prompt when scroll view is closed and player is in range
            }
        }

        // Position the prompt text slightly above the sprite (target object) in world space
        if (targetObject != null && promptText != null)
        {
            // Offset the prompt text above the sprite by 2 units (adjust the Y value as needed)
            Vector3 offsetPosition = targetObject.position + new Vector3(0, 2, 0);  // Adjust the Y offset here

            // Set the prompt text position above the sprite
            promptText.transform.position = offsetPosition;
        }
    }

    // Get the current binding for the Interact action and convert it to human-readable format
    private void UpdateInteractBindingText()
    {
        if (UserInput.Instance != null)
        {
            var playerInput = UserInput.Instance.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                var inputActions = playerInput.actions;
                if (inputActions != null)
                {
                    var interactAction = inputActions.FindAction("Interact");
                    if (interactAction != null)
                    {
                        // Get the active binding based on the current control scheme
                        bool usingGamepad = UserInput.Instance.UsingController;
                        
                        // Find appropriate binding for current device
                        int bindingIndex = 0;
                        for (int i = 0; i < interactAction.bindings.Count; i++)
                        {
                            var binding = interactAction.bindings[i];
                            if (binding.isComposite || binding.isPartOfComposite)
                                continue;
                                
                            bool isGamepadBinding = binding.path.ToLower().Contains("gamepad");
                            if ((usingGamepad && isGamepadBinding) || (!usingGamepad && !isGamepadBinding))
                            {
                                bindingIndex = i;
                                break;
                            }
                        }
                        
                        // Get the binding path
                        string bindingPath = interactAction.bindings[bindingIndex].effectivePath;
                        string fullDisplayString = InputControlPath.ToHumanReadableString(bindingPath);
                        
                        // Clean up the display string to remove device information
                        interactBindingText = CleanBindingDisplayText(fullDisplayString, usingGamepad);
                    }
                }
            }
        }
    }
    
    // Clean up the binding display text to show just the key name without device prefix
    private string CleanBindingDisplayText(string displayText, bool isGamepad)
    {
        if (string.IsNullOrEmpty(displayText))
            return "Interact";
            
        if (isGamepad)
        {
            // For gamepad, keep the button name but remove unnecessary text
            // Replace "Gamepad " with just "Button "
            displayText = displayText.Replace("Gamepad ", "");
            
            // Some gamepad buttons have special names we want to keep as-is
            return displayText;
        }
        else
        {
            // For keyboard/mouse, extract just the key name
            // Remove the "[Keyboard]" or other device prefix
            int bracketIndex = displayText.IndexOf(']');
            if (bracketIndex >= 0 && bracketIndex + 1 < displayText.Length)
            {
                return displayText.Substring(bracketIndex + 1).Trim();
            }
            
            // Fallback if the format is different
            string pattern = @"[\[\(].*?[\]\)]";
            return Regex.Replace(displayText, pattern, "").Trim();
        }
    }

    // Listen for control binding changes
    private void OnEnable()
    {
        if (UserInput.Instance != null)
        {
            var playerInput = UserInput.Instance.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.onControlsChanged += OnControlsChanged;
            }
        }
    }

    private void OnDisable()
    {
        if (UserInput.Instance != null)
        {
            var playerInput = UserInput.Instance.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.onControlsChanged -= OnControlsChanged;
            }
        }
    }

    private void OnControlsChanged(PlayerInput input)
    {
        // Update the binding text when controls change
        UpdateInteractBindingText();
        
        // If prompt is visible, update it
        if (isInRange && promptText != null && promptText.gameObject.activeSelf)
        {
            promptText.text = $"Press {interactBindingText} to read";
        }
    }

    private void ToggleScrollViewVisibility()
    {
        // Toggle the visibility of the scroll view
        bool newState = !scrollView.activeSelf;
        scrollView.SetActive(newState);
    }

    private void PlayAudio()
    {
        // Check if the audio source is set and if an audio clip is assigned
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();  // Play the audio
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is not set. Make sure an AudioSource with a valid AudioClip is assigned.");
        }
    }
}
