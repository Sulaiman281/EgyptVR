using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarNetworkAnimation : NetworkBehaviour
{
    [Header("LeftHand")] [SerializeField] private NetworkAnimator leftHand;
    [SerializeField] private InputAction leftGrab, leftTrigger;

    [Header("RightHand")] [SerializeField] private NetworkAnimator rightHand;
    [SerializeField] private InputAction rightGrab, rightTrigger;

    private void Start()
    {
        if (IsOwner)
        {
            ControlsSetup();
        }
    }

    public override void OnDestroy()
    {
        leftGrab.Disable();
        leftTrigger.Disable();
        rightGrab.Disable();
        rightTrigger.Disable();
        base.OnDestroy();
    }

    public void ControlsSetup()
    {
        Debug.Log("Doing Setup");
        // let's do mapping
        var mapping = GetComponent<AvatarMapping>();
        mapping.MapPlayerMap(FindObjectOfType<PlayerMapRef>());

        {
            // left hand events and animation setup
            leftGrab.Enable();
            leftTrigger.Enable();

            leftGrab.started += _ => { LeftGrabSyncServerRpc(true); };
            leftGrab.canceled += _ => { LeftGrabSyncServerRpc(false); };

            leftTrigger.started += _ => { LeftTriggerSyncServerRpc(true); };
            leftTrigger.canceled += _ => { LeftTriggerSyncServerRpc(false); };
        }

        {
            // right hand events and animation setup
            rightTrigger.Enable();
            rightGrab.Enable();

            rightGrab.started += _ => { RightGrabSyncServerRpc(true); };
            rightGrab.canceled += _ => { RightGrabSyncServerRpc(false); };
            rightTrigger.started += _ => { RightTriggerSyncServerRpc(true); };
            rightTrigger.canceled += _ => { RightTriggerSyncServerRpc(false); };
        }
    }

    [ServerRpc]
    public void LeftGrabSyncServerRpc(bool value)
    {
        leftHand.Animator.SetFloat("Grab", value ? 1 : 0);
    }

    [ServerRpc]
    public void LeftTriggerSyncServerRpc(bool value)
    {
        leftHand.Animator.SetFloat("Trigger", value ? 1 : 0);
    }

    [ServerRpc]
    public void RightGrabSyncServerRpc(bool value)
    {
        rightHand.Animator.SetFloat("Grab", value ? 1 : 0);
    }

    [ServerRpc]
    public void RightTriggerSyncServerRpc(bool value)
    {
        rightHand.Animator.SetFloat("Trigger", value ? 1 : 0);
    }
}