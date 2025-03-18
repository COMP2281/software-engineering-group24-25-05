using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

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
    private Button interactButton;
    private Button deviceToggleButton;
    
    // Button callback
    public delegate void ButtonCallback();
    private ButtonCallback onRebindComplete;

    private bool rebindInProgress = false;
    private bool isDestroying = false;
    
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
        interactButton = root.Q<Button>("interactButton");
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
            SetupControlButton(interactButton, "Interact", GetCorrectBindingIndex("Interact", interactButton.name));
            
            // Update the button labels to show current bindings
            UpdateControlLabels();
        }

        // Listen for scene changes to cancel rebinding
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Cancel any rebinding in progress when scene unloads
        if (rebindInProgress)
        {
            CancelRebinding();
        }
    }
    
    private void OnDisable()
    {
        // Cancel any rebinding in progress when object is disabled
        if (rebindInProgress)
        {
            CancelRebinding();
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
        SetupControlButton(interactButton, "Interact", GetCorrectBindingIndex("Interact", interactButton.name));
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
            UpdateButtonLabel(jumpButton, "Jump", GetCorrectBindingIndex("Jump", "jumpButton"));
            UpdateButtonLabel(crouchButton, "Crouch", GetCorrectBindingIndex("Crouch", "crouchButton"));
            UpdateButtonLabel(rightButton, "Move", GetCorrectBindingIndex("Move", "rightButton"));
            UpdateButtonLabel(leftButton, "Move", GetCorrectBindingIndex("Move", "leftButton"));
            UpdateButtonLabel(interactButton, "Interact", GetCorrectBindingIndex("Interact", interactButton.name));
        }
    }
    
    private void UpdateButtonLabel(Button button, string actionName, int bindingIndex)
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
                    // Use the binding at the specified index
                    if (bindingIndex >= 0 && bindingIndex < action.bindings.Count)
                    {
                        var binding = action.bindings[bindingIndex];
                        bindingDisplayString = !string.IsNullOrEmpty(binding.overridePath) 
                            ? binding.overridePath 
                            : binding.path;
                    }
                    else
                    {
                        // Fallback to getting any binding that matches the control scheme
                        bindingDisplayString = GetBindingForControlScheme(action, currentControlScheme);
                    }
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
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (!binding.isComposite && MatchesControlScheme(binding.groups, controlScheme))
            {
                return !string.IsNullOrEmpty(binding.overridePath)
                    ? binding.overridePath
                    : binding.path;
            }
        }
        throw new System.Exception($"No valid binding found for control scheme '{controlScheme}'.");
    }
    
    // Helper to get binding display for composite bindings with case-insensitive comparisons
    private string GetCompositeBindingDisplayString(InputAction action, string compositePart, string controlScheme)
    {
        // Remove fallback and avoid matching if binding.groups is empty
        // Must strictly match the scheme
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
                bool matchesScheme = MatchesControlScheme(binding.groups, controlScheme);
                
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
        
        throw new System.Exception($"No valid composite binding found for '{compositePart}' in scheme '{controlScheme}'.");
    }

    private void StartRebinding(string actionName, int bindingIndex = 0)
    {
        // Prevent starting a new rebind if one is already in progress
        if (rebindInProgress)
        {
            Debug.LogWarning("Attempt to start rebinding while another is in progress");
            return;
        }

        // Check if we've reached the maximum rebind attempts for this scene
        if (UserInput.Instance != null && !UserInput.Instance.RegisterRebindAttempt())
        {
            Debug.LogWarning("Too many rebind attempts. Resetting input system.");
            UserInput.Instance.ResetInputSystem();
            return;
        }

        // Make sure we have valid input actions before attempting to rebind
        if (inputActions == null)
        {
            Debug.LogError("Input actions asset is null. Cannot start rebinding.");
            return;
        }

        InputAction action = inputActions.FindAction(actionName);
        if (action != null)
        {
            // Store info about the binding we're rebinding
            bool isGamepadScheme = currentControlScheme.ToLower().Contains("gamepad") || 
                                  currentControlScheme.ToLower().Contains("controller");
            
            Debug.Log($"Starting rebind for {actionName} using {(isGamepadScheme ? "gamepad" : "keyboard")} scheme.");
            
            // For Move action or composite parts, use the provided binding index
            if (actionName == "Move" || 
                (bindingIndex > 0 && bindingIndex < action.bindings.Count && action.bindings[bindingIndex].isPartOfComposite))
            {
                // Use the supplied bindingIndex for composite parts
                if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
                {
                    Debug.LogWarning($"Invalid binding index {bindingIndex} for action {actionName}");
                    return;
                }
            }
            else
            {
                // For standard actions, explicitly find the correct binding for the current device type
                bindingIndex = FindBindingIndexForDeviceType(action, isGamepadScheme);
                if (bindingIndex < 0)
                {
                    Debug.LogWarning($"Could not find a binding for action {actionName} for {(isGamepadScheme ? "gamepad" : "keyboard")}");
                    return;
                }
                Debug.Log($"Selected binding index {bindingIndex} for {actionName}");
            }
            
            // For composite parts we need special handling
            var bindingIsCompositePart = bindingIndex > 0 && bindingIndex < action.bindings.Count && 
                                        action.bindings[bindingIndex].isPartOfComposite;
            
            // Store the action and binding index for use in callbacks
            actionToRebind = action;
            this.bindingIndex = bindingIndex;
            
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
                    string partName = action.bindings[bindingIndex].name;
                    actionText = $"{actionName} {partName}";
                }
                
                if (rebindText != null)
                {
                    string bindPath = action.bindings[bindingIndex].path;
                    string overridePath = action.bindings[bindingIndex].overridePath;
                    
                    Debug.Log($"Rebinding: Current path = {bindPath}, Override = {overridePath}");
                    rebindText.text = $"Press any {(isGamepadScheme ? "button" : "key")} for {actionText}...";
                }
            }

            rebindInProgress = true;
            
            try
            {
                // Configure the rebinding operation
                var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                    .WithCancelingThrough("<Keyboard>/escape")
                    .OnMatchWaitForAnother(0.1f)
                    .WithoutGeneralizingPathOfSelectedControl();
                    
                // Add stronger device constraints based on the current control scheme
                if (!isGamepadScheme) // Keyboard/Mouse
                {
                    rebindOperation = rebindOperation
                        .WithControlsHavingToMatchPath("<Keyboard>")
                        .WithControlsExcluding("<Gamepad>")
                        .WithControlsExcluding("<Joystick>");
                }
                else // Gamepad
                {
                    rebindOperation = rebindOperation
                        .WithControlsHavingToMatchPath("<Gamepad>")
                        .WithControlsExcluding("<Keyboard>")
                        .WithControlsExcluding("<Mouse>");
                }
                    
                // Complete the rebinding operation setup with clean error handling
                this.rebindOperation = rebindOperation
                    .OnComplete(operation => {
                        if (isDestroying) return; // Don't process if being destroyed
                        
                        try {
                            // Make a local copy of the path to avoid issues with the operation being disposed
                            string newBindingPath = null;
                            
                            if (operation != null && operation.selectedControl != null) {
                                newBindingPath = operation.selectedControl.path;
                                Debug.Log($"Selected control path: {newBindingPath}");
                            }
                            
                            // Store binding details locally before disposing the operation
                            InputAction localAction = actionToRebind;
                            int localBindingIndex = this.bindingIndex;
                            bool isLocalGamepad = currentControlScheme.ToLower().Contains("gamepad") || 
                                               currentControlScheme.ToLower().Contains("controller");
                            bool isLocalCompositePart = bindingIsCompositePart;
                            
                            // Dispose the operation before applying the binding to avoid race conditions
                            if (rebindOperation != null)
                            {
                                rebindOperation.Dispose();
                                rebindOperation = null;
                            }
                            
                            // Double-check the binding path matches the expected device type
                            bool isBindingForGamepad = IsGamepadPath(newBindingPath);
                            if (isLocalGamepad != isBindingForGamepad) {
                                Debug.LogWarning($"Selected control type mismatch: Expected {(isLocalGamepad ? "gamepad" : "keyboard")} " +
                                              $"but got {(isBindingForGamepad ? "gamepad" : "keyboard")} control. Skipping.");
                                CompleteRebind();
                                return;
                            }
                            
                            // Only then apply the binding
                            if (newBindingPath != null && localAction != null)
                            {
                                if (!isLocalCompositePart && actionName != "Move") {
                                    // For regular bindings, use our scheme-specific binding method
                                    ApplyControlSchemeSpecificBinding(localAction, newBindingPath, isLocalGamepad);
                                }
                                else if (isLocalCompositePart || actionName == "Move") {
                                    // For composite bindings, directly apply to the specific part
                                    localAction.ApplyBindingOverride(localBindingIndex, newBindingPath);
                                    // Save after applying
                                    var bindingOverridesSaved = inputActions.SaveBindingOverridesAsJson();
                                    PlayerPrefs.SetString("InputBindings", bindingOverridesSaved);
                                    PlayerPrefs.Save();
                                    Debug.Log($"Applied composite binding override: {newBindingPath} to index {localBindingIndex}");
                                }
                            }
                            
                            CompleteRebind();
                        } catch (System.Exception e) {
                            Debug.LogError($"Error during rebind completion: {e.Message}\n{e.StackTrace}");
                            SafeCleanup();
                        }
                    })
                    .OnCancel(operation => {
                        if (isDestroying) return; // Don't process if being destroyed
                        
                        try {
                            CancelRebinding();
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
    
    // Helper method to find the binding index for the current device type
    private int FindBindingIndexForDeviceType(InputAction action, bool isGamepad)
    {
        // First try to find a binding that explicitly matches the device type
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite || binding.isPartOfComposite)
                continue;
            
            string path = !string.IsNullOrEmpty(binding.overridePath) ? binding.overridePath : binding.path;
            bool bindingIsGamepad = IsGamepadPath(path);
            
            if (isGamepad == bindingIsGamepad) {
                Debug.Log($"Found matching binding at index {i}: {path} (isGamepad: {bindingIsGamepad})");
                return i;
            }
        }
        
        // If no explicit match, try using control scheme groups
        string schemeName = isGamepad ? GetGamepadControlSchemeName() : GetKeyboardControlSchemeName();
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite || binding.isPartOfComposite)
                continue;
                
            if (MatchesControlScheme(binding.groups, schemeName)) {
                Debug.Log($"Found scheme-matched binding at index {i}: {binding.path} (scheme: {binding.groups})");
                return i;
            }
        }
        
        // If still no match, return the first non-composite binding as fallback
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!action.bindings[i].isComposite && !action.bindings[i].isPartOfComposite)
                return i;
        }
        
        return -1; // No suitable binding found
    }

    // Improve the scheme-matching logic to be more exact
    private bool MatchesControlScheme(string bindingGroups, string schemeName)
    {
        if (string.IsNullOrEmpty(bindingGroups))
            return false;
        
        // Split groups by commas and check for exact scheme match
        string[] groups = bindingGroups.Split(',');
        foreach (string group in groups)
        {
            // Trim and compare case-insensitive
            if (string.Equals(group.Trim(), schemeName, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    // Replace the ApplyControlSchemeSpecificBinding method with a more robust approach
    private void ApplyControlSchemeSpecificBinding(InputAction action, string newBindingPath, bool isGamepad)
    {
        Debug.Log($"Applying new binding for {action.name}, path: {newBindingPath}, isGamepad: {isGamepad}");
        
        try
        {
            // Get the scheme name based on device type
            string currentScheme = isGamepad ? GetGamepadControlSchemeName() : GetKeyboardControlSchemeName();
            
            // Validate that the binding path is appropriate for the target control scheme
            bool isBindingPathForGamepad = IsGamepadPath(newBindingPath);
            
            // If we're trying to bind a gamepad input to keyboard or vice versa, block it
            if (isGamepad != isBindingPathForGamepad)
            {
                Debug.LogWarning($"Binding path {newBindingPath} doesn't match scheme type (gamepad: {isGamepad}). Skipping.");
                return;
            }
            
            // Use our enhanced method to find the exact binding index for this device type
            int bindingIndex = FindExactBindingIndexForScheme(action, isGamepad);
            
            if (bindingIndex >= 0)
            {
                // Apply the new binding override only to this specific binding
                action.ApplyBindingOverride(bindingIndex, newBindingPath);
                
                // Now, save all binding overrides
                var bindingOverridesSaved = inputActions.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString("InputBindings", bindingOverridesSaved);
                PlayerPrefs.Save();
                
                Debug.Log($"Successfully applied binding override at index {bindingIndex}");
                Debug.Log($"New binding overrides: {bindingOverridesSaved}");
            }
            else
            {
                Debug.LogError($"Couldn't find appropriate binding for {action.name} with scheme {currentScheme}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error applying control scheme binding: {e.Message}");
        }
    }
    
    // Add a method to find the exact binding index for a specific scheme/device type
    private int FindExactBindingIndexForScheme(InputAction action, bool isGamepad)
    {
        string schemeName = isGamepad ? GetGamepadControlSchemeName() : GetKeyboardControlSchemeName();
        
        // First try to find a binding that explicitly matches the scheme name
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite || binding.isPartOfComposite)
                continue;
                
            if (MatchesControlScheme(binding.groups, schemeName))
            {
                Debug.Log($"Found exact scheme match at binding index {i}: {binding.path} for scheme {schemeName}");
                return i;
            }
        }
        
        // Next, try to find a binding based on the device type path
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite || binding.isPartOfComposite)
                continue;
                
            string path = !string.IsNullOrEmpty(binding.overridePath) ? binding.overridePath : binding.path;
            bool bindingIsGamepad = IsGamepadPath(path);
            
            // If it's a keyboard binding without a specific scheme, only match it when we're looking for keyboard
            if (!isGamepad && !bindingIsGamepad && 
                (string.IsNullOrEmpty(binding.groups) || !binding.groups.Contains(GetGamepadControlSchemeName())))
            {
                Debug.Log($"Found keyboard-specific binding at index {i}: {path}");
                return i;
            }
            
            // If it's a gamepad binding without a specific scheme, only match it when we're looking for gamepad
            if (isGamepad && bindingIsGamepad &&
                (string.IsNullOrEmpty(binding.groups) || !binding.groups.Contains(GetKeyboardControlSchemeName())))
            {
                Debug.Log($"Found gamepad-specific binding at index {i}: {path}");
                return i;
            }
        }
        
        // If nothing specific found, look for first binding of the right device type
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite || binding.isPartOfComposite)
                continue;
                
            string path = !string.IsNullOrEmpty(binding.overridePath) ? binding.overridePath : binding.path;
            bool bindingIsGamepad = IsGamepadPath(path);
            
            if (isGamepad == bindingIsGamepad)
            {
                Debug.Log($"Found device-type match (fallback) at binding index {i}: {path}");
                return i;
            }
        }
        
        // Last resort - first non-composite binding
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!action.bindings[i].isComposite && !action.bindings[i].isPartOfComposite)
            {
                Debug.Log($"Using first available binding (last resort) at index {i}");
                return i;
            }
        }
        
        return -1; // No suitable binding found
    }
    
    // Enhance the IsGamepadPath method to be more comprehensive
    private bool IsGamepadPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return false;
        
        string lowerPath = path.ToLower();
        
        // Check for various gamepad-related strings
        return lowerPath.Contains("gamepad") || 
               lowerPath.Contains("joystick") || 
               lowerPath.Contains("dualshock") || 
               lowerPath.Contains("dualsense") || 
               lowerPath.Contains("xbox") ||
               lowerPath.Contains("ps4") || 
               lowerPath.Contains("ps5") ||
               lowerPath.Contains("nintendo") ||
               lowerPath.Contains("controller");
    }

    // Add a keyboard-specific path detection method
    private bool IsKeyboardPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return false;
        
        string lowerPath = path.ToLower();
        
        return lowerPath.Contains("keyboard") || 
               lowerPath.Contains("<key>") ||
               lowerPath.Contains("mouse") || 
               (!IsGamepadPath(lowerPath) && !lowerPath.Contains("touch"));
    }

    // A safe cleanup method to be called whenever we encounter an exception
    private void SafeCleanup()
    {
        CancelRebinding();
    }

    private int FindBindingIndexForControlScheme(InputAction action, string actionName, string controlScheme)
    {
        if (actionName == "Move")
        {
            // For move action, we need to find the composite binding for this control scheme
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].isComposite && MatchesControlScheme(action.bindings[i].groups, controlScheme))
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
                    MatchesControlScheme(action.bindings[i].groups, controlScheme))
                {
                    return i;
                }
            }
        }
        
        // If no binding found with exact scheme match, fall back to device type detection
        bool isGamepad = controlScheme.ToLower().Contains("gamepad") || 
                         controlScheme.ToLower().Contains("controller");
                         
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].isComposite || action.bindings[i].isPartOfComposite)
                continue;
                
            bool isGamepadBinding = action.bindings[i].path.ToLower().Contains("gamepad");
            if ((isGamepad && isGamepadBinding) || (!isGamepad && !isGamepadBinding))
            {
                return i;
            }
        }
        
        throw new System.Exception($"No binding found for action '{actionName}' in scheme '{controlScheme}'");
    }
    
    private void CompleteRebind()
    {
        // Re-enable the action
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
        
        // Hide the overlay
        if (rebindOverlay != null)
        {
            rebindOverlay.style.display = DisplayStyle.None;
        }
        
        // Reset state
        rebindInProgress = false;
        actionToRebind = null;
        
        // Update the control labels
        UpdateControlLabels();
        
        // Save the bindings
        SaveBindings();
        
        // Call the callback
        if (onRebindComplete != null)
        {
            try
            {
                onRebindComplete.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error in rebind complete callback: {e.Message}");
            }
        }
    }
    
    private void CancelRebinding()
    {
        if (rebindOperation != null)
        {
            try
            {
                // Properly cancel the operation
                rebindOperation.Cancel();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error cancelling rebind operation: {e.Message}");
            }
            
            try
            {
                // Then dispose it
                rebindOperation.Dispose();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error disposing rebind operation: {e.Message}");
            }
            rebindOperation = null;
        }
        
        // Re-enable the action
        if (actionToRebind != null && !isDestroying)
        {
            try
            {
                actionToRebind.Enable();
                actionToRebind = null;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error enabling action: {e.Message}");
            }
        }
        
        // Hide the overlay
        if (rebindOverlay != null)
        {
            rebindOverlay.style.display = DisplayStyle.None;
        }
        
        rebindInProgress = false;
    }

    private void RebindComplete()
    {
        // This method is now replaced by CompleteRebind which is called after
        // safely disposing the rebinding operation
        CompleteRebind();
    }
    
    private void RebindCancelled()
    {
        // This method is now replaced by CancelRebinding for better safety
        CancelRebinding();
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
                    Debug.Log($"Saved bindings: {bindingOverridesJson}");
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
                    Debug.Log($"Loading bindings: {bindingOverridesJson}");
                    // Load the saved binding overrides
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
        isDestroying = true;
        
        // Remove scene unloaded listener
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        
        // Cancel any ongoing rebinding when destroyed
        if (rebindInProgress)
        {
            CancelRebinding();
        }

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
        if (inputActions == null) throw new System.Exception("Input actions asset is null.");
        
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
        throw new System.Exception("No keyboard/mouse scheme found.");
    }

    private string GetGamepadControlSchemeName()
    {
        if (inputActions == null) throw new System.Exception("Input actions asset is null.");
        
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
        throw new System.Exception("No gamepad/controller scheme found.");
    }

    // Add this new method to improve binding management
    public void ResetBindingsForCurrentControlScheme()
    {
        if (inputActions == null) return;
        
        bool isGamepad = currentControlScheme.ToLower().Contains("gamepad") || 
                         currentControlScheme.ToLower().Contains("controller");
        
        // Log what we're about to reset
        Debug.Log($"Resetting bindings for {(isGamepad ? "gamepad" : "keyboard/mouse")} control scheme");
        
        foreach (var actionMap in inputActions.actionMaps)
        {
            foreach (var action in actionMap.actions)
            {
                if (action.bindings.Count == 0) continue;
                
                // Keep track of composite bindings we're processing
                int? currentCompositeIndex = null;
                bool currentCompositeIsForCurrentScheme = false;
                
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    
                    // Start of a new composite binding
                    if (binding.isComposite)
                    {
                        currentCompositeIndex = i;
                        // Check if this composite is for our current scheme
                        bool matchesControlScheme = MatchesControlScheme(binding.groups, currentControlScheme);
                        bool isGamepadBinding = binding.path.ToLower().Contains("gamepad");
                        
                        // Determine if this composite binding belongs to our current control scheme
                        currentCompositeIsForCurrentScheme = matchesControlScheme || 
                            ((isGamepad && isGamepadBinding) || (!isGamepad && !isGamepadBinding));
                        
                        // Reset the composite binding if it matches the current scheme
                        if (currentCompositeIsForCurrentScheme)
                        {
                            action.RemoveBindingOverride(i);
                        }
                        continue;
                    }
                    
                    // Handle composite parts
                    if (binding.isPartOfComposite && currentCompositeIndex.HasValue)
                    {
                        // Only reset composite parts if the parent composite matches our scheme
                        if (currentCompositeIsForCurrentScheme)
                        {
                            action.RemoveBindingOverride(i);
                        }
                        continue;
                    }
                    
                    // Handle regular (non-composite) bindings
                    currentCompositeIndex = null;
                    
                    // Check if this binding belongs to our current device type
                    bool matchesScheme = MatchesControlScheme(binding.groups, currentControlScheme);
                    bool isForGamepad = binding.path.ToLower().Contains("gamepad");
                    
                    if (matchesScheme || ((isGamepad && isForGamepad) || (!isGamepad && !isForGamepad)))
                    {
                        action.RemoveBindingOverride(i);
                        Debug.Log($"Reset binding for action: {action.name}, binding: {binding.path}");
                    }
                }
            }
        }
        
        // Save the updated bindings
        SaveBindings();
        UpdateControlLabels();
        Debug.Log($"Completed resetting bindings for {(isGamepad ? "gamepad" : "keyboard/mouse")} control scheme");
    }
}
