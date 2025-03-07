using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{

    public static UserInput Instace;

    public Vector2 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool CrouchHold { get; private set; }
    public bool MenuOpenCloseInput { get; private set; }
    public bool SubmitInput { get; private set; }


    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;
    private InputAction _menuOpenCloseAction;
    private InputAction _SubmitAction;
    



    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        // Load any saved bindings
        LoadSavedBindings();

        SetupInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _crouchAction = _playerInput.actions["Crouch"];
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
        _SubmitAction = _playerInput.actions["Submit"];
    
        // _interactAction = _playerInput.actions["Interact"];
    }

    private void UpdateInputs()
    {
        MovementInput = _moveAction.ReadValue<Vector2>();
        JumpPressed = _jumpAction.WasPressedThisFrame();
        JumpHeld = _jumpAction.IsPressed();
        JumpReleased = _jumpAction.WasReleasedThisFrame();
        CrouchHold = _crouchAction.IsPressed();
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
        SubmitInput = _SubmitAction.WasPressedThisFrame();
        // DialogueAdvanceInput = _dialogueAdvanceAction.WasPressedThisFrame();
        // Interact = _interactAction.WasPressedThisFrame();
    }

    private void LoadSavedBindings()
    {
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            string rebinds = PlayerPrefs.GetString("InputBindings");
            Debug.Log("Loading saved input bindings: " + rebinds);
            _playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }
    }
}
