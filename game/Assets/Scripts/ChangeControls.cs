using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
            
            // Store info about the binding we're rebinding
            bool isGamepadScheme = currentControlScheme.ToLower().Contains("gamepad") || 
                                  currentControlScheme.ToLower().Contains("controller");
            
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
                    rebindText.text = $"Press any {(isGamepadScheme ? "button" : "key")} for {actionText}...";
                }
            }

            rebindInProgress = true;
            
            try
            {
                // Configure the rebinding operation
                var rebindOperation = action.PerformInteractiveRebinding(correctBindingIndex)
                    .WithCancelingThrough("<Keyboard>/escape")
                    .OnMatchWaitForAnother(0.1f)
                    .WithoutGeneralizingPathOfSelectedControl();
                    
                // Add device constraints based on the current control scheme
                if (!isGamepadScheme) // Keyboard/Mouse
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
                            
                            // Only then apply the binding
                            if (newBindingPath != null && localAction != null && !isLocalCompositePart && actionName != "Move") {
                                ApplyControlSchemeSpecificBinding(localAction, newBindingPath, isLocalGamepad);
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

    // Replace the ApplyControlSchemeSpecificBinding method with a more fundamental approach
    private void ApplyControlSchemeSpecificBinding(InputAction action, string newBindingPath, bool isGamepad)
    {
        Debug.Log($"Applying new binding for {action.name}, path: {newBindingPath}, isGamepad: {isGamepad}");
        
        try
        {
            // Create a totally separate path for keyboard vs gamepad bindings
            // This ensures they're completely independent
            string currentScheme = isGamepad ? GetGamepadControlSchemeName() : GetKeyboardControlSchemeName();
            
            // Find the specific control path that matches our scheme AND action
            int bindingIndex = -1;
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (binding.isComposite || binding.isPartOfComposite)
                    continue;
                    
                // Check if this is a binding for our current device type
                bool isBindingForGamepad = binding.path.ToLower().Contains("gamepad");
                if ((isGamepad && isBindingForGamepad) || (!isGamepad && !isBindingForGamepad))
                {
                    bindingIndex = i;
                    break;
                }
            }
            
            if (bindingIndex >= 0)
            {
                // IMPORTANT: Don't clear ALL binding overrides, just apply the new one
                // This ensures we keep the other control scheme's bindings
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
        
        foreach (var actionMap in inputActions.actionMaps)
        {
            foreach (var action in actionMap.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    if (binding.isComposite || binding.isPartOfComposite)
                        continue;
                    
                    bool isGamepadBinding = binding.path.ToLower().Contains("gamepad");
                    // Only reset bindings for the current control scheme
                    if ((isGamepad && isGamepadBinding) || (!isGamepad && !isGamepadBinding))
                    {
                        action.RemoveBindingOverride(i);
                    }
                }
            }
        }
        
        // Save the updated bindings
        SaveBindings();
        UpdateControlLabels();
        Debug.Log($"Reset bindings for {(isGamepad ? "gamepad" : "keyboard/mouse")} control scheme");
    }
}
