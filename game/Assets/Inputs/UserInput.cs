using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance;

    // Input system management properties
    [Tooltip("Maximum rebinding operations allowed per scene")]
    public int maxRebindOperationsPerScene = 3;
    private int rebindAttempts = 0;

    public Vector2 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool CrouchHold { get; private set; }
    public bool MenuOpenCloseInput { get; private set; }
    public bool SubmitInput { get; private set; }
    public Vector2 AimInput { get; private set; }
    public bool UsingController { get; private set; }
    public bool InteractPressed { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;
    private InputAction _menuOpenCloseAction;
    private InputAction _SubmitAction;
    private InputAction _aimAction;
    private InputAction _interactAction;

    private bool isQuitting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the input manager across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        // Register scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Load any saved bindings
        LoadSavedBindings();

        SetupInputActions();
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
        
        // Release all input actions explicitly
        _moveAction = null;
        _jumpAction = null;
        _crouchAction = null;
        _menuOpenCloseAction = null;
        _SubmitAction = null;
        _aimAction = null;
        _interactAction = null;
        
        Instance = null;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset counters when scenes change
        rebindAttempts = 0;
        
        // Force garbage collection to clean up any dangling input references
        System.GC.Collect();
        
        // Re-enable actions when a new scene loads
        if (_playerInput != null && _playerInput.actions != null)
        {
            try
            {
                _playerInput.actions.Enable();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error enabling actions on scene load: {e.Message}");
            }
        }
    }

    private void OnEnable()
    {
        // Ensure actions are enabled when this component is enabled
        if (_playerInput != null && _playerInput.actions != null)
        {
            _playerInput.actions.Enable();
        }
    }

    private void OnDisable()
    {
        if (isQuitting)
            return;
            
        // Ensure actions are disabled when this component is disabled
        if (_playerInput != null && _playerInput.actions != null)
        {
            try
            {
                _playerInput.actions.Disable();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error disabling actions: {e.Message}");
            }
        }
    }

    private void OnDestroy()
    {
        if (isQuitting)
            return;
            
        // Unregister scene event listener
        SceneManager.sceneLoaded -= OnSceneLoaded;
            
        // Explicitly null out all input actions to avoid ObjectDisposedException
        _moveAction = null;
        _jumpAction = null;
        _crouchAction = null;
        _menuOpenCloseAction = null;
        _SubmitAction = null;
        _aimAction = null;
        _interactAction = null;
        
        // Clear the static instance when this object is destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        if (_playerInput != null && _playerInput.actions != null)
        {
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _crouchAction = _playerInput.actions["Crouch"];
            _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
            _SubmitAction = _playerInput.actions["Submit"];
            _aimAction = _playerInput.actions["Look"];
            _interactAction = _playerInput.actions["Interact"];
        }
    }

    private void UpdateInputs()
    {
        if (_playerInput == null || !_playerInput.enabled)
            return;

        // Safely read input values with null checks
        MovementInput = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        JumpPressed = _jumpAction != null && _jumpAction.WasPressedThisFrame();
        JumpHeld = _jumpAction != null && _jumpAction.IsPressed();
        JumpReleased = _jumpAction != null && _jumpAction.WasReleasedThisFrame();
        CrouchHold = _crouchAction != null && _crouchAction.IsPressed();
        MenuOpenCloseInput = _menuOpenCloseAction != null && _menuOpenCloseAction.WasPressedThisFrame();
        SubmitInput = _SubmitAction != null && _SubmitAction.WasPressedThisFrame();
        AimInput = _aimAction != null ? _aimAction.ReadValue<Vector2>() : Vector2.zero;
        InteractPressed = _interactAction != null && _interactAction.WasPressedThisFrame();

        if (_playerInput.currentControlScheme != null)
        {
            UsingController = _playerInput.currentControlScheme.ToLower().Contains("gamepad");
        }
    }

    private void LoadSavedBindings()
    {
        if (_playerInput != null && _playerInput.actions != null && PlayerPrefs.HasKey("InputBindings"))
        {
            string rebinds = PlayerPrefs.GetString("InputBindings");
            Debug.Log("Loading saved input bindings: " + rebinds);
            
            try
            {
                _playerInput.actions.LoadBindingOverridesFromJson(rebinds);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load input bindings: {e.Message}");
                // If loading fails, don't reset - this could cause control scheme issues
            }
        }
    }
    
    /// <summary>
    /// Register a rebind attempt - returns false if too many have been made
    /// </summary>
    public bool RegisterRebindAttempt()
    {
        rebindAttempts++;
        if (rebindAttempts > maxRebindOperationsPerScene)
        {
            Debug.LogWarning($"Too many rebind attempts in this scene ({rebindAttempts}). Consider reloading the scene.");
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Reinitializes the input system - call this when having issues with input
    /// </summary>
    public void ResetInputSystem()
    {
        if (_playerInput != null)
        {
            // Disable actions first
            if (_playerInput.actions != null)
            {
                _playerInput.actions.Disable();
            }
            
            // Null out references
            _moveAction = null;
            _jumpAction = null;
            _crouchAction = null;
            _menuOpenCloseAction = null;
            _SubmitAction = null;
            _aimAction = null;
            _interactAction = null;
            
            // Setup again
            SetupInputActions();
            
            // Re-enable
            if (_playerInput.actions != null)
            {
                _playerInput.actions.Enable();
            }
            
            // Reset counters
            rebindAttempts = 0;
            
            // Force garbage collection
            System.GC.Collect();
            
            Debug.Log("Input system reset");
        }
    }
}
