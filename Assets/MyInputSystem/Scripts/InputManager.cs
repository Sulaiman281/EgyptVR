using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public HandController leftController;
    public HandController rightController;

    private void Start()
    {
        leftController.EnableHand();
        rightController.EnableHand();
        // DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        leftController.EnableHand();
        rightController.EnableHand();
    }

    private void LateUpdate()
    {
        leftController.UpdateInputs();
        rightController.UpdateInputs();
    }

    private void OnDisable()
    {
        leftController.DisableHand();
        rightController.DisableHand();
    }
}

[Serializable]
public struct HandController
{
    [Header("InputActions")] [SerializeField]
    private Hand hand;

    [SerializeField] private InputAction triggerPressedInput;
    [SerializeField] private InputAction gripPressedInput;
    [SerializeField] private InputAction primaryPressedInput;
    [SerializeField] private InputAction secondaryPressedInput;
    [SerializeField] private InputAction joyStickMoveInput;
    [SerializeField] private InputAction menuPressInput;
    
    [Header("InputValues")] public Vector2 joystickMove;
    public bool triggerPressed;
    public bool gripPressed;
    public bool primaryPressed;
    public bool secondaryPressed;
    public bool menuPress;

    [Header("Events")] public PlayerInput.ActionEvent onTriggerPressStart;
    public PlayerInput.ActionEvent onGripPressStart;
    public PlayerInput.ActionEvent onPrimaryPressStart;
    public PlayerInput.ActionEvent onSecondaryPressStart;
    public PlayerInput.ActionEvent onMenuPressStart;
    
    public PlayerInput.ActionEvent onTriggerPressEnd;
    public PlayerInput.ActionEvent onGripPressEnd;
    public PlayerInput.ActionEvent onPrimaryPressEnd;
    public PlayerInput.ActionEvent onSecondaryPressEnd;
    public PlayerInput.ActionEvent onMenuPressEnd;

    public void EnableHand()
    {
        triggerPressedInput.Enable();
        gripPressedInput.Enable();
        primaryPressedInput.Enable();
        secondaryPressedInput.Enable();
        joyStickMoveInput.Enable();
        menuPressInput.Enable();

        triggerPressedInput.started += OnTriggerStart;
        gripPressedInput.started += OnGripStart;
        primaryPressedInput.started += OnPrimaryStart;
        secondaryPressedInput.started += OnSecondaryStart;
        menuPressInput.started += OnMenuStart;
        
        triggerPressedInput.canceled += OnTriggerEnd;
        gripPressedInput.canceled += OnGripEnd;
        primaryPressedInput.canceled += OnPrimaryEnd;
        secondaryPressedInput.canceled += OnSecondaryEnd;
        menuPressInput.canceled += OnMenuEnd;
    }

    private void OnMenuEnd(InputAction.CallbackContext obj)
    {
        onMenuPressEnd.Invoke(default);
    }

    private void OnSecondaryEnd(InputAction.CallbackContext obj)
    {
        onSecondaryPressEnd.Invoke(default);
    }

    private void OnPrimaryEnd(InputAction.CallbackContext obj)
    {
        onPrimaryPressEnd.Invoke(default);
    }

    private void OnGripEnd(InputAction.CallbackContext obj)
    {
        onGripPressEnd.Invoke(default);
    }

    private void OnTriggerEnd(InputAction.CallbackContext obj)
    {
        onTriggerPressEnd.Invoke(default);
    }

    private void OnMenuStart(InputAction.CallbackContext obj)
    {
        onMenuPressStart.Invoke(default);
    }

    private void OnSecondaryStart(InputAction.CallbackContext obj)
    {
        onSecondaryPressStart.Invoke(default);
    }

    private void OnPrimaryStart(InputAction.CallbackContext obj)
    {
        onPrimaryPressStart.Invoke(default);
    }

    private void OnGripStart(InputAction.CallbackContext obj)
    {
        onGripPressStart.Invoke(default);
    }

    private void OnTriggerStart(InputAction.CallbackContext obj)
    {
        onTriggerPressStart.Invoke(default);
    }

    public void DisableHand()
    {
        triggerPressedInput.Disable();
        gripPressedInput.Disable();
        primaryPressedInput.Disable();
        secondaryPressedInput.Disable();
        joyStickMoveInput.Disable();
        menuPressInput.Disable();
        
        triggerPressedInput.started -= OnTriggerStart;
        gripPressedInput.started -= OnGripStart;
        primaryPressedInput.started -= OnPrimaryStart;
        secondaryPressedInput.started -= OnSecondaryStart;
        menuPressInput.started -= OnMenuStart;
        
        triggerPressedInput.canceled -= OnTriggerEnd;
        gripPressedInput.canceled -= OnGripEnd;
        primaryPressedInput.canceled -= OnPrimaryEnd;
        secondaryPressedInput.canceled -= OnSecondaryEnd;
        menuPressInput.canceled -= OnMenuEnd;
    }

    public void UpdateInputs()
    {
        triggerPressed = triggerPressedInput.IsPressed();
        gripPressed = gripPressedInput.IsPressed();
        primaryPressed = primaryPressedInput.IsPressed();
        secondaryPressed = secondaryPressedInput.IsPressed();
        joystickMove = joyStickMoveInput.ReadValue<Vector2>();
        menuPress = menuPressInput.IsPressed();
    }
}

public enum Hand
{
    LeftHand = 0,
    RightHand = 1
}