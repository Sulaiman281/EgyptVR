using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInput : MonoBehaviour
{
    [Header("Left Hand")] [SerializeField] private InputAction leftGrabInput;
    [SerializeField] private InputAction leftTriggerInput;

    public PlayerInput.ActionEvent leftGrabStart;
    public PlayerInput.ActionEvent leftGrabCancel;
    
    public PlayerInput.ActionEvent leftTriggerStart;
    public PlayerInput.ActionEvent leftTriggerCancel;

    [Header("RightHand")] [SerializeField] private InputAction rightGrabInput;
    [SerializeField] private InputAction rightTriggerInput;
    
    public PlayerInput.ActionEvent rightGrabStart;
    public PlayerInput.ActionEvent rightGrabCancel;
    
    public PlayerInput.ActionEvent rightTriggerStart;
    public PlayerInput.ActionEvent rightTriggerCancel;

    private void Start()
    {
        {
            // left hand input action setup
            leftGrabInput.Enable();
            leftTriggerInput.Enable();

            leftGrabInput.started += _ =>
            {
                leftGrabStart.Invoke(default);
            };

            leftGrabInput.canceled += _ =>
            {
                leftGrabCancel.Invoke(default);
            };

            leftTriggerInput.started += _ =>
            {
                leftTriggerStart.Invoke(default);
            };
            leftTriggerInput.canceled += _ =>
            {
                leftTriggerCancel.Invoke(default);
            };
        }
        {
            // right hand input action setup
            rightGrabInput.Enable();
            rightTriggerInput.Enable();

            leftGrabInput.started += _ =>
            {
                rightGrabStart.Invoke(default);
            };

            rightGrabInput.canceled += _ =>
            {
                rightGrabCancel.Invoke(default);
            };

            rightTriggerInput.started += _ =>
            {
                rightTriggerStart.Invoke(default);
            };
            rightTriggerInput.canceled += _ =>
            {
                rightTriggerCancel.Invoke(default);
            };
        }
    }
}
