using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarAnimation : MonoBehaviour
{
    [Header("LeftHand")] [SerializeField]
    private Animator leftHandAnimator;
    [SerializeField] private InputAction triggerInputL;
    [SerializeField] private InputAction grabInputL;
    
    [Header("RightHand")]
    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private InputAction triggerInputR;
    [SerializeField] private InputAction grabInputR;

    private void Start()
    {
        { // left hand animation setup
            triggerInputL.Enable();
            grabInputL.Enable();
            triggerInputL.started += _ => { leftHandAnimator.SetFloat("Trigger", 1); };
            triggerInputL.canceled += _ => { leftHandAnimator.SetFloat("Trigger", 0); };
            grabInputL.started += _ => { leftHandAnimator.SetFloat("Grab", 1); };
            grabInputL.canceled += _ => { leftHandAnimator.SetFloat("Grab", 0); };
        }
        { // right hand animation setup
            triggerInputR.Enable();
            grabInputR.Enable();
            triggerInputR.started += _ => { rightHandAnimator.SetFloat("Trigger", 1); };
            triggerInputR.canceled += _ => { rightHandAnimator.SetFloat("Trigger", 0); };
            grabInputR.started += _ => { rightHandAnimator.SetFloat("Grab", 1); };
            grabInputR.canceled += _ => { rightHandAnimator.SetFloat("Grab", 0); };
        }
    }
}