using UnityEngine;
using UnityEngine.InputSystem;

public class TransScript : MonoBehaviour
{
    [Header("Input")] [SerializeField] private InputAction triggerInput;
    [Header("Obj Ref")] [SerializeField] private Transform scripObj;
    public bool isHovering { get; set; }

    private void Awake()
    {
        scripObj = transform.GetChild(0);
        scripObj.gameObject.SetActive(false);
        triggerInput.Enable();
        triggerInput.started += _ =>
        {
            if (!isHovering) return;
            scripObj.gameObject.SetActive(true);
        };
        triggerInput.canceled += _ =>
        {
            scripObj.gameObject.SetActive(false);
        };
    }
}
