using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSize : MonoBehaviour
{
    [Header("Input")] [SerializeField] private InputAction leftGrabAction;
    [SerializeField] private InputAction rightGrabAction;

    [Header("PlayerReference")] [SerializeField]
    private Transform playerObj;

    [SerializeField] private int maxScale;

    public bool holdingLeftGrab;
    public bool holdingRightGrab;

    private bool _hasScaled;

    private void Awake()
    {
        {
            // Enabling Left Hand Action Input and reading when start action and cancel action.
            leftGrabAction.Enable();

            leftGrabAction.started += _ => { holdingLeftGrab = true; VerifyScale();};

            leftGrabAction.canceled += _ => { holdingLeftGrab = false; VerifyScale();};
        }
        {
            // Enabling Right Hand Action Input and reading when start action and cancel action.
            rightGrabAction.Enable();

            rightGrabAction.started += _ => { holdingRightGrab = true; VerifyScale();};

            rightGrabAction.canceled += _ => { holdingRightGrab = false; VerifyScale();};
        }
    }

    /**
     * This Method checks if player has holding both controllers grab buttons it grow large, otherwise the normal size
     */
    private void VerifyScale()
    {
        playerObj.transform.localScale = Vector3.one * (holdingLeftGrab && holdingRightGrab ? maxScale : 1);
    }
}