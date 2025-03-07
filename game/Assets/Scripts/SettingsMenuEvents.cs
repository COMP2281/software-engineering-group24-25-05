using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

// Code from https://youtu.be/_jtj73lu2Ko?si=qFlQrmGtG8tRAwfv

public class SettingsMenuEvents : MonoBehaviour
{
    private Button backButton;
    private Button audioButton;
    private Button videoButton;
    private Button controlsButton;
    private Button jumpButton;
    private Button crouchButton;
    private Button rightButton;
    private Button leftButton;
    private Button deviceToggleButton;  // New button to toggle between keyboard/controller
    
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;
    
    // Reference to the player input asset
    private PlayerInput playerInput;
    private InputActionAsset inputActions;
    
    // UI Elements for displaying the rebinding process
    private VisualElement rebindOverlay;
    private Label rebindText;
    
    // For storing the action being rebound
    private InputAction actionToRebind;
    private int bindingIndex;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    
    // Control scheme tracking
    private string currentControlScheme = "MouseKeyboard"; // Default to keyboard
    
    void OnEnable()
    {
        // Load the UXML and USS
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Retrieve the buttons by their name
        backButton = root.Q<Button>("BackButton");
        audioButton = root.Q<Button>("AudioButton");
        videoButton = root.Q<Button>("VideoButton");
        controlsButton = root.Q<Button>("ControlsButton");
        jumpButton = root.Q<Button>("jumpButton");
        crouchButton = root.Q<Button>("crouchButton");
        rightButton = root.Q<Button>("rightButton");
        leftButton = root.Q<Button>("leftButton");
        deviceToggleButton = root.Q<Button>("DeviceToggleButton");

        // Register the button click event
        backButton.RegisterCallback<ClickEvent>(evt => LoadScene("MainMenu"));
        
        // Setup device toggle if it exists
        if (deviceToggleButton != null)
        {
            deviceToggleButton.text = "Current: Keyboard & Mouse";
            deviceToggleButton.clicked += ToggleControlScheme;
        }
        else
        {
            Debug.LogWarning("DeviceToggleButton not found in the UI. Please add it to your UXML.");
        }
        
        // Find rebind overlay elements - this will now look for the elements added by UXMLLoader
        rebindOverlay = root.Q<VisualElement>("RebindOverlay");
        rebindText = root.Q<Label>("RebindText");
        
        if (rebindOverlay != null)
        {
            rebindOverlay.style.display = DisplayStyle.None;
            Debug.Log("Found rebind overlay");
        }
        else
        {
            Debug.LogError("Rebind overlay not found! Make sure UXMLLoader has added it to the UI tree.");
        }
        
        // Get reference to PlayerInput component - usually this would be on a player GameObject
        // For now, we'll find it in the scene (you may need to adjust based on your setup)
        playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            inputActions = playerInput.actions;
            
            // Load any saved bindings
            LoadBindings();
            
            // Set up the control change buttons
            SetupControlButton(jumpButton, "Jump");
            SetupControlButton(crouchButton, "Crouch");
            SetupControlButton(rightButton, "Move", GetCorrectBindingIndex("Move", "rightButton")); // Assuming right is the positive x binding
            SetupControlButton(leftButton, "Move", GetCorrectBindingIndex("Move", "leftButton"));  // Assuming left is the negative x binding
            
            // Update the button labels to show current bindings
            UpdateControlLabels();
        }
        else
        {
            Debug.LogError("PlayerInput not found in scene!");
        }

        // Retrieve all buttons in the menu
        root.Query<Button>().ForEach(button =>
        {
            _menuButtons.Add(button);
            button.RegisterCallback<ClickEvent>(OnAllButtonClick);
            button.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        });

        // Initialize the audio source
        _audioSource = GetComponent<AudioSource>();
    }

    private void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    private void OnButtonHover(MouseEnterEvent evt)
    {
        _audioSource.Play();
    }
    
    // Toggle between keyboard and controller control schemes
    private void ToggleControlScheme()
    {
        if (currentControlScheme == "MouseKeyboard")
        {
            currentControlScheme = "Gamepad";
            if (deviceToggleButton != null)
                deviceToggleButton.text = "Current: Controller";
        }
        else
        {
            currentControlScheme = "MouseKeyboard";
            if (deviceToggleButton != null)
                deviceToggleButton.text = "Current: Keyboard & Mouse";
        }
        
        // Update all control labels for the new scheme
        UpdateControlLabels();
        
        // Play button sound
        if (_audioSource != null)
            _audioSource.Play();
    }
    
    private void SetupControlButton(Button button, string actionName, int bindingIndex = 0)
    {
        if (button != null)
        {
            // Get the correct binding index based on button and action
            int actualBindingIndex = GetCorrectBindingIndex(actionName, button.name);
            
            button.clicked += () => StartRebinding(actionName, actualBindingIndex);
            
            // Update the button label with the current binding
            UpdateButtonLabel(button, actionName, actualBindingIndex);
        }
    }
    
    private void UpdateControlLabels()
    {
        if (inputActions != null)
        {
            // Update each control button with current binding using correct indices for current control scheme
            UpdateButtonLabel(jumpButton, "Jump", 0);
            UpdateButtonLabel(crouchButton, "Crouch", 0);
            UpdateButtonLabel(rightButton, "Move", GetCorrectBindingIndex("Move", "rightButton"));
            UpdateButtonLabel(leftButton, "Move", GetCorrectBindingIndex("Move", "leftButton"));
        }
    }
    
    private void UpdateButtonLabel(Button button, string actionName, int bindingIndex = 0)
    {
        if (button != null && inputActions != null)
        {
            InputAction action = inputActions.FindAction(actionName);
            if (action != null)
            {
                string bindingDisplayString;
                
                // Find the correct binding for the current control scheme
                if (actionName == "Move")
                {
                    // For Move, we need special handling for composite bindings
                    string compositePart = button.name == "rightButton" ? "right" : "left";
                    bindingDisplayString = GetCompositeBindingDisplayString(action, compositePart, currentControlScheme);
                }
                else
                {
                    // For normal actions, find the binding for the current control scheme
                    bindingDisplayString = GetBindingForControlScheme(action, currentControlScheme);
                }
                
                bindingDisplayString = InputControlPath.ToHumanReadableString(bindingDisplayString);
                
                // Find the label element in the button
                Label label = button.Q<Label>();
                if (label != null)
                {
                    label.text = $"Change {GetActionDisplayName(actionName, button.name)}: {bindingDisplayString}";
                }
                else
                {
                    button.text = $"Change {GetActionDisplayName(actionName, button.name)}: {bindingDisplayString}";
                }
            }
            else
            {
                Debug.LogWarning($"Action '{actionName}' not found in input actions");
            }
        }
        else
        {
            Debug.LogWarning("Button or inputActions is null");
        }
    }
    
    private string GetBindingForControlScheme(InputAction action, string controlScheme)
    {
        Debug.Log($"Searching for binding for action '{action.name}' in control scheme '{controlScheme}'");
        
        // Find a binding that matches the control scheme
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            
            // Check if this binding matches the control scheme we want
            if (!binding.isComposite && 
                (string.IsNullOrEmpty(binding.groups) || binding.groups.Contains(controlScheme)))
            {
                Debug.Log($"Found binding: {binding.path} (override: {binding.overridePath})");
                // Return the effective binding path
                return !string.IsNullOrEmpty(binding.overridePath) ? binding.overridePath : binding.path;
            }
        }
        
        Debug.LogWarning($"No binding found for action '{action.name}' in control scheme '{controlScheme}'");
        // Default if not found
        return "Not bound";
    }
    
    // Helper to get binding display for composite bindings
    private string GetCompositeBindingDisplayString(InputAction action, string compositePart, string controlScheme)
    {
        // Find the composite binding that matches our control scheme
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite && 
                (string.IsNullOrEmpty(binding.groups) || binding.groups.Contains(controlScheme)))
            {
                // Found the correct composite, now find the part we want
                for (int j = i + 1; j < action.bindings.Count; j++)
                {
                    var partBinding = action.bindings[j];
                    if (!partBinding.isPartOfComposite)
                        break; // We've gone past the composite parts
                        
                    if (partBinding.name.ToLower() == compositePart.ToLower())
                    {
                        // Return the override path if it exists, otherwise the original path
                        return !string.IsNullOrEmpty(partBinding.overridePath) ? partBinding.overridePath : partBinding.path;
                    }
                }
            }
        }
        
        // Default if not found
        return "Not bound";
    }
    
    private void StartRebinding(string actionName, int bindingIndex = 0)
    {
        InputAction action = inputActions.FindAction(actionName);
        if (action != null)
        {
            // Find the correct binding index for the current control scheme
            int correctBindingIndex = FindBindingIndexForControlScheme(action, actionName, currentControlScheme);
            if (correctBindingIndex == -1)
            {
                Debug.LogError($"Could not find binding for action {actionName} in control scheme {currentControlScheme}");
                return;
            }
            
            // Store the action and binding index for use in callbacks
            actionToRebind = action;
            this.bindingIndex = correctBindingIndex;
            
            // Disable the action before rebinding
            action.Disable();
            
            // Show the rebinding overlay
            if (rebindOverlay != null)
            {
                rebindOverlay.style.display = DisplayStyle.Flex;
                rebindText.text = $"Press any {(currentControlScheme == "Gamepad" ? "button" : "key")} for {actionName}...";
            }
            
            // Debug information
            Debug.Log($"Starting rebind for {actionName}, binding index: {correctBindingIndex}, current path: {action.bindings[correctBindingIndex].effectivePath}");
            
            // Configure the rebinding operation based on the current control scheme
            var rebindOperation = action.PerformInteractiveRebinding(correctBindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .WithoutGeneralizingPathOfSelectedControl();
                
            if (currentControlScheme == "MouseKeyboard")
            {
                rebindOperation = rebindOperation.WithControlsHavingToMatchPath("<Keyboard>")
                                                .WithControlsExcluding("<Gamepad>");
            }
            else if (currentControlScheme == "Gamepad")
            {
                rebindOperation = rebindOperation.WithControlsHavingToMatchPath("<Gamepad>")
                                                .WithControlsExcluding("<Keyboard>");
            }
                
            // Complete the rebinding operation setup
            this.rebindOperation = rebindOperation
                .OnComplete(operation => {
                    Debug.Log($"Rebind complete. New binding: {action.bindings[correctBindingIndex].overridePath}");
                    RebindComplete();
                })
                .OnCancel(operation => {
                    Debug.Log("Rebind cancelled");
                    RebindCancelled();
                })
                .Start();
        }
    }
    
    private int FindBindingIndexForControlScheme(InputAction action, string actionName, string controlScheme)
    {
        if (actionName == "Move")
        {
            // For move action, we need to find the composite binding for this control scheme
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].isComposite && 
                    (string.IsNullOrEmpty(action.bindings[i].groups) || action.bindings[i].groups.Contains(controlScheme)))
                {
                    // Return the index of the composite itself
                    return i;
                }
            }
        }
        else
        {
            // For normal actions, find the binding that matches the control scheme
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!action.bindings[i].isComposite && !action.bindings[i].isPartOfComposite &&
                    (string.IsNullOrEmpty(action.bindings[i].groups) || action.bindings[i].groups.Contains(controlScheme)))
                {
                    return i;
                }
            }
        }
        
        return -1; // Not found
    }
    
    private void RebindComplete()
    {
        // Clean up the rebinding operation
        rebindOperation.Dispose();
        rebindOperation = null;
        
        // Re-enable the action
        actionToRebind.Enable();
        
        // Hide the overlay
        if (rebindOverlay != null)
            rebindOverlay.style.display = DisplayStyle.None;
        
        // Update the control labels
        UpdateControlLabels();
        
        // Save the bindings
        SaveBindings();
    }
    
    private void RebindCancelled()
    {
        // Clean up the rebinding operation
        rebindOperation.Dispose();
        rebindOperation = null;
        
        // Re-enable the action
        actionToRebind.Enable();
        
        // Hide the overlay
        if (rebindOverlay != null)
            rebindOverlay.style.display = DisplayStyle.None;
    }
    
    private void SaveBindings()
    {
        // Save bindings to player preferences
        var bindingOverridesJson = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("InputBindings", bindingOverridesJson);
        PlayerPrefs.Save();
    }
    
    private void LoadBindings()
    {
        // Load bindings from player preferences if available
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            string bindingOverridesJson = PlayerPrefs.GetString("InputBindings");
            inputActions.LoadBindingOverridesFromJson(bindingOverridesJson);
            UpdateControlLabels();
        }
    }

    // Function to display the correct settings menu based on the button clicked
    private void DisplaySettingsMenu(string menuName)
    {
        // Hide all settings menus
        foreach (var button in _menuButtons)
        {
            button.style.display = DisplayStyle.None;
        }

        // Display the selected settings menu
        var selectedMenu = _menuButtons.Find(button => button.name == menuName);
        selectedMenu.style.display = DisplayStyle.Flex;
    }
    
    private void OnDestroy()
    {
        // Clean up any ongoing rebinding operation
        if (rebindOperation != null)
        {
            rebindOperation.Dispose();
            rebindOperation = null;
        }
    }

    // Helper method to get the correct binding index for Move actions
    private int GetCorrectBindingIndex(string actionName, string buttonName)
    {
        InputAction action = inputActions.FindAction(actionName);
        if (action == null)
            return 0;
            
        if (actionName != "Move") 
        {
            // For normal actions, find the binding matching current control scheme
            return FindBindingIndexForControlScheme(action, actionName, currentControlScheme);
        }
            
        // For Move action, find the composite binding for current control scheme,
        // then determine the part index based on the button
        int compositeIndex = FindBindingIndexForControlScheme(action, actionName, currentControlScheme);
        if (compositeIndex == -1)
            return 0;
            
        // Find the parts of this composite
        string partName = buttonName == "rightButton" ? "right" : "left";
        
        for (int i = compositeIndex + 1; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (!binding.isPartOfComposite)
                break;
                
            if (binding.name.ToLower() == partName.ToLower())
                return i;
        }
        
        // Default value if not matching
        return 0;
    }

    // Helper method to get display names for buttons
    private string GetActionDisplayName(string actionName, string buttonName)
    {
        // For Move action, return the direction instead of "Move"
        if (actionName == "Move")
        {
            if (buttonName == "rightButton") return "Right";
            if (buttonName == "leftButton") return "Left";
        }
        
        return actionName;
    }
}
