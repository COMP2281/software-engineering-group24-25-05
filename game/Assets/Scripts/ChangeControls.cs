using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ChangeControls : MonoBehaviour
{
    // UI Elements for displaying the rebinding process
    private VisualElement rebindOverlay;
    private Label rebindText;
    
    // Reference to the player input asset
    private PlayerInput playerInput;
    private InputActionAsset inputActions;
    
    // For storing the action being rebound
    private InputAction actionToRebind;
    private int bindingIndex;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    
    // Control scheme tracking
    private string currentControlScheme = "MouseKeyboard"; // Default to keyboard
    
    // Control buttons
    private Button jumpButton;
    private Button crouchButton;
    private Button rightButton;
    private Button leftButton;
    private Button deviceToggleButton;
    
    // Button callback
    public delegate void ButtonCallback();
    private ButtonCallback onRebindComplete;
    
    public void Initialize(
        VisualElement root,
        PlayerInput inputComponent,
        ButtonCallback rebindCompleteCallback = null)
    {
        // Store callback
        onRebindComplete = rebindCompleteCallback;
        
        // Find rebind overlay elements
        rebindOverlay = root.Q<VisualElement>("RebindOverlay");
        rebindText = rebindOverlay?.Q<Label>("RebindText");
        
        if (rebindOverlay != null)
        {
            rebindOverlay.style.display = DisplayStyle.None;
        }
        
        // Find control buttons
        jumpButton = root.Q<Button>("jumpButton");
        crouchButton = root.Q<Button>("crouchButton");
        rightButton = root.Q<Button>("rightButton");
        leftButton = root.Q<Button>("leftButton");
        deviceToggleButton = root.Q<Button>("DeviceToggleButton");
        
        // Setup device toggle if it exists
        if (deviceToggleButton != null)
        {
            deviceToggleButton.text = "Current: Keyboard & Mouse";
            deviceToggleButton.clicked += ToggleControlScheme;
        }
        
        playerInput = inputComponent;
        if (playerInput != null)
        {
            inputActions = playerInput.actions;
            
            // Set the correct initial control scheme name from the actual available schemes
            currentControlScheme = GetKeyboardControlSchemeName();
            
            // Load any saved bindings
            LoadBindings();
            
            // Set up the control change buttons
            SetupControlButton(jumpButton, "Jump");
            SetupControlButton(crouchButton, "Crouch");
            SetupControlButton(rightButton, "Move", GetCorrectBindingIndex("Move", "rightButton"));
            SetupControlButton(leftButton, "Move", GetCorrectBindingIndex("Move", "leftButton"));
            
            // Update the button labels to show current bindings
            UpdateControlLabels();
        }
    }

    // Toggle between keyboard and controller control schemes
    public void ToggleControlScheme()
    {
        if (currentControlScheme == GetKeyboardControlSchemeName())
        {
            currentControlScheme = GetGamepadControlSchemeName();
            if (deviceToggleButton != null)
                deviceToggleButton.text = "Current: Controller";
        }
        else
        {
            currentControlScheme = GetKeyboardControlSchemeName();
            if (deviceToggleButton != null)
                deviceToggleButton.text = "Current: Keyboard & Mouse";
        }
        
        // Reset the SetupControlButton for movement bindings to ensure we get the correct indices for the new scheme
        SetupControlButton(rightButton, "Move", GetCorrectBindingIndex("Move", "rightButton"));
        SetupControlButton(leftButton, "Move", GetCorrectBindingIndex("Move", "leftButton"));
        SetupControlButton(jumpButton, "Jump", GetCorrectBindingIndex("Jump", jumpButton.name));
        SetupControlButton(crouchButton, "Crouch", GetCorrectBindingIndex("Crouch", crouchButton.name));
    }
    
    private void SetupControlButton(Button button, string actionName, int bindingIndex = 0)
    {
        if (button != null)
        {
            // Remove any existing click events to avoid duplicates
            button.clicked -= () => StartRebinding(actionName, bindingIndex);
            
            // Get the correct binding index based on button and action
            int actualBindingIndex = GetCorrectBindingIndex(actionName, button.name);
            
            // Add the new click event
            button.clicked += () => StartRebinding(actionName, actualBindingIndex);
            
            // Update the button label with the current binding
            UpdateButtonLabel(button, actionName, actualBindingIndex);
        }
    }
    
    public void UpdateControlLabels()
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
        }
    }
    
    private string GetBindingForControlScheme(InputAction action, string controlScheme)
    {
        // Find a binding that matches the control scheme
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            
            // Check if this binding matches the control scheme we want with case-insensitive comparison
            if (!binding.isComposite && 
                (string.IsNullOrEmpty(binding.groups) || 
                 ContainsIgnoreCase(binding.groups, controlScheme)))
            {
                // Return the effective binding path
                return !string.IsNullOrEmpty(binding.overridePath) ? binding.overridePath : binding.path;
            }
        }
        
        // Default if not found
        return "Not bound";
    }
    
    // Helper to get binding display for composite bindings with case-insensitive comparisons
    private string GetCompositeBindingDisplayString(InputAction action, string compositePart, string controlScheme)
    {
        // Try a different approach to find the correct binding for each control scheme
        bool isGamepad = controlScheme.ToLower().Contains("gamepad") || controlScheme.ToLower().Contains("controller");
        
        // Find a binding part with the right name that belongs to the right device
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            
            // Skip if not a part of composite
            if (!binding.isPartOfComposite) continue;
                
            // Check if the binding name matches what we want
            if (string.Equals(binding.name, compositePart, System.StringComparison.OrdinalIgnoreCase))
            {
                string path = !string.IsNullOrEmpty(binding.overridePath) ? binding.overridePath : binding.path;
                
                // Check if this matches our current device type
                bool isGamepadBinding = path.ToLower().Contains("gamepad");
                
                if ((isGamepad && isGamepadBinding) || (!isGamepad && !isGamepadBinding))
                {
                    return path;
                }
            }
        }

        // If not found above, try the original approach but with better scheme checking
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            
            // For a composite binding, the group may be on the composite itself rather than each part
            if (binding.isComposite)
            {
                bool matchesScheme = string.IsNullOrEmpty(binding.groups) || 
                                     ContainsIgnoreCase(binding.groups, controlScheme);
                
                if (matchesScheme)
                {
                    // Found the correct composite, now find the part we want
                    for (int j = i + 1; j < action.bindings.Count; j++)
                    {
                        var partBinding = action.bindings[j];
                        if (!partBinding.isPartOfComposite)
                            break; // We've gone past the composite parts
                        
                        if (string.Equals(partBinding.name, compositePart, System.StringComparison.OrdinalIgnoreCase))
                        {
                            // Return the override path if it exists, otherwise the original path
                            return !string.IsNullOrEmpty(partBinding.overridePath) ? partBinding.overridePath : partBinding.path;
                        }
                    }
                }
            }
        }
        
        return "Not bound";
    }

    private void StartRebinding(string actionName, int bindingIndex = 0)
    {
        // Make sure we have valid input actions before attempting to rebind
        if (inputActions == null)
        {
            Debug.LogError("Input actions asset is null. Cannot start rebinding.");
            return;
        }

        InputAction action = inputActions.FindAction(actionName);
        if (action != null)
        {
            // Use the supplied bindingIndex
            int correctBindingIndex = bindingIndex;
            if(correctBindingIndex < 0)
            {
                Debug.LogWarning($"Invalid binding index {correctBindingIndex} for action {actionName}");
                return;
            }
            
            // Ensure the binding index is valid for this action
            if (correctBindingIndex >= action.bindings.Count)
            {
                Debug.LogWarning($"Binding index {correctBindingIndex} is out of range for action {actionName}");
                return;
            }
            
            // For composite parts we need special handling
            var bindingIsCompositePart = correctBindingIndex > 0 && correctBindingIndex < action.bindings.Count && 
                                        action.bindings[correctBindingIndex].isPartOfComposite;
            
            // Store the action and binding index for use in callbacks
            actionToRebind = action;
            this.bindingIndex = correctBindingIndex;
            
            // Disable the action before rebinding
            action.Disable();
            
            // Show the rebinding overlay
            if (rebindOverlay != null)
            {
                rebindOverlay.style.display = DisplayStyle.Flex;
                string actionText = actionName;
                
                // For composite parts, show the specific direction
                if (bindingIsCompositePart)
                {
                    string partName = action.bindings[correctBindingIndex].name;
                    actionText = $"{actionName} {partName}";
                }
                
                if (rebindText != null)
                {
                    rebindText.text = $"Press any {(currentControlScheme.ToLower().Contains("gamepad") ? "button" : "key")} for {actionText}...";
                }
            }
            
            try
            {
                // Configure the rebinding operation
                var rebindOperation = action.PerformInteractiveRebinding(correctBindingIndex)
                    .WithCancelingThrough("<Keyboard>/escape")
                    .OnMatchWaitForAnother(0.1f)
                    .WithoutGeneralizingPathOfSelectedControl();
                    
                // Add device constraints based on the current control scheme
                bool isGamepad = currentControlScheme.ToLower().Contains("gamepad") || 
                                currentControlScheme.ToLower().Contains("controller");
                                
                if (!isGamepad) // Keyboard/Mouse
                {
                    rebindOperation = rebindOperation
                        .WithControlsHavingToMatchPath("<Keyboard>")
                        .WithControlsExcluding("<Gamepad>");
                }
                else // Gamepad
                {
                    rebindOperation = rebindOperation
                        .WithControlsHavingToMatchPath("<Gamepad>")
                        .WithControlsExcluding("<Keyboard>");
                }
                    
                // Complete the rebinding operation setup with clean error handling
                this.rebindOperation = rebindOperation
                    .OnComplete(operation => {
                        try {
                            RebindComplete();
                        } catch (System.Exception e) {
                            Debug.LogError($"Error during rebind completion: {e.Message}");
                            SafeCleanup();
                        }
                    })
                    .OnCancel(operation => {
                        try {
                            RebindCancelled();
                        } catch (System.Exception e) {
                            Debug.LogError($"Error during rebind cancellation: {e.Message}");
                            SafeCleanup();
                        }
                    })
                    .Start();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error starting rebinding operation: {e.Message}");
                SafeCleanup();
            }
        }
    }
    
    // A safe cleanup method to be called whenever we encounter an exception
    private void SafeCleanup()
    {
        if (rebindOperation != null)
        {
            try
            {
                rebindOperation.Dispose();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error disposing rebind operation: {e.Message}");
            }
            rebindOperation = null;
        }
        
        if (actionToRebind != null)
        {
            try
            {
                actionToRebind.Enable();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error enabling action: {e.Message}");
            }
        }
        
        if (rebindOverlay != null)
        {
            rebindOverlay.style.display = DisplayStyle.None;
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
                    (string.IsNullOrEmpty(action.bindings[i].groups) || 
                     ContainsIgnoreCase(action.bindings[i].groups, controlScheme)))
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
                    (string.IsNullOrEmpty(action.bindings[i].groups) || 
                     ContainsIgnoreCase(action.bindings[i].groups, controlScheme)))
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
        if (rebindOperation != null)
        {
            rebindOperation.Dispose();
            rebindOperation = null;
        }
        
        // Re-enable the action
        if (actionToRebind != null)
            actionToRebind.Enable();
        
        // Hide the overlay
        if (rebindOverlay != null)
            rebindOverlay.style.display = DisplayStyle.None;
        
        // Update the control labels
        UpdateControlLabels();
        
        // Save the bindings
        SaveBindings();
        
        // Call the callback
        onRebindComplete?.Invoke();
    }
    
    private void RebindCancelled()
    {
        // Clean up the rebinding operation
        if (rebindOperation != null)
        {
            rebindOperation.Dispose();
            rebindOperation = null;
        }
        
        // Re-enable the action
        if (actionToRebind != null)
            actionToRebind.Enable();
        
        // Hide the overlay
        if (rebindOverlay != null)
            rebindOverlay.style.display = DisplayStyle.None;
    }
    
    public void SaveBindings()
    {
        try
        {
            if (inputActions != null)
            {
                // Save bindings to player preferences
                var bindingOverridesJson = inputActions.SaveBindingOverridesAsJson();
                if (!string.IsNullOrEmpty(bindingOverridesJson))
                {
                    PlayerPrefs.SetString("InputBindings", bindingOverridesJson);
                    PlayerPrefs.Save();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving bindings: {e.Message}");
        }
    }
    
    public void LoadBindings()
    {
        try
        {
            // Load bindings from player preferences if available
            if (PlayerPrefs.HasKey("InputBindings") && inputActions != null)
            {
                string bindingOverridesJson = PlayerPrefs.GetString("InputBindings");
                if (!string.IsNullOrEmpty(bindingOverridesJson))
                {
                    inputActions.LoadBindingOverridesFromJson(bindingOverridesJson);
                    UpdateControlLabels();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading bindings: {e.Message}");
        }
    }
    
    public void OnDestroy()
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
        {
            return 0;
        }
            
        if (actionName != "Move") 
        {
            // For normal actions, find the binding matching current control scheme
            int bindingIndex = FindBindingIndexForControlScheme(action, actionName, currentControlScheme);
            return bindingIndex;
        }
            
        // For Move action with composite bindings, we handle it differently
        
        // First determine what part we need
        string partName = buttonName == "rightButton" ? "right" : "left";
        
        // Check if we're using gamepad
        bool isGamepad = currentControlScheme.ToLower().Contains("gamepad") || 
                        currentControlScheme.ToLower().Contains("controller");
                        
        // First search specifically for the right device type
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            
            // Skip non-composite parts
            if (!binding.isPartOfComposite) continue;
            
            // Check if this is the part we want
            if (binding.name.ToLower() == partName.ToLower())
            {
                // Check if it belongs to the right device type
                string path = !string.IsNullOrEmpty(binding.overridePath) ? binding.overridePath : binding.path;
                bool isGamepadBinding = path.ToLower().Contains("gamepad");
                
                if ((isGamepad && isGamepadBinding) || (!isGamepad && !isGamepadBinding))
                {
                    return i;
                }
            }
        }
        
        // If we didn't find a device-specific match, fall back to just finding the right part
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isPartOfComposite && binding.name.ToLower() == partName.ToLower())
            {
                return i;
            }
        }
        
        // Default value if not matching
        return -1;
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

    // Helper methods to get the actual control scheme names from the asset
    private string GetKeyboardControlSchemeName()
    {
        if (inputActions == null) return "MouseKeyboard";
        
        foreach (var scheme in inputActions.controlSchemes)
        {
            // Check if this scheme contains keyboard
            if (scheme.name.ToLower().Contains("key") || 
                scheme.name.ToLower().Contains("mouse") || 
                scheme.name.ToLower().Contains("keyboard"))
            {
                return scheme.name;
            }
        }
        return "MouseKeyboard"; // Default
    }

    private string GetGamepadControlSchemeName()
    {
        if (inputActions == null) return "Gamepad";
        
        foreach (var scheme in inputActions.controlSchemes)
        {
            // Check if this scheme is for gamepad
            if (scheme.name.ToLower().Contains("gamepad") || 
                scheme.name.ToLower().Contains("controller") || 
                scheme.name.ToLower().Contains("joystick"))
            {
                return scheme.name;
            }
        }
        return "Gamepad"; // Default
    }

    // Add a helper method for case-insensitive string contains
    private bool ContainsIgnoreCase(string source, string toCheck)
    {
        return source != null && toCheck != null && 
               source.IndexOf(toCheck, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
