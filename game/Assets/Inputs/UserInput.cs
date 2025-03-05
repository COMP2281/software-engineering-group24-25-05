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
    // public bool DialogueAdvanceInput { get; private set; }
    // public bool Interact { get; private set; }


    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;
    private InputAction _menuOpenCloseAction;
    // private InputAction _dialogueAdvanceAction;
    // private InputAction _interactAction;



    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }

        _playerInput = GetComponent<PlayerInput>();

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
        // _dialogueAdvanceAction = _playerInput.actions["DialogueAdvance"];
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
        // DialogueAdvanceInput = _dialogueAdvanceAction.WasPressedThisFrame();
        // Interact = _interactAction.WasPressedThisFrame();
    }
}
