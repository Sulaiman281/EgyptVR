using UnityEngine;
using UnityEngine.InputSystem;

public class LeftHandCanvas : MonoBehaviour
{
    // [SerializeField] private InputAction menuKeyAction;
    // [SerializeField] private Transform canvasTrans;
    // private void Awake()
    // {
    //     menuKeyAction.Enable();
    //     menuKeyAction.started += _ =>
    //     {
    //         if (!GameManager.instance.hasPressedTheMenu)
    //         {
    //             GameManager.instance.hasPressedTheMenu = true;
    //             GameManager.instance.robotScript.SetRobotInstructionText("");
    //             // pause and play
    //             GameManager.instance.robotScript.Pause(false);
    //         }
    //         canvasTrans.gameObject.SetActive(true);
    //         transform.LookAt(Camera.main.transform);
    //     };
    //     menuKeyAction.canceled += _ =>
    //     {
    //         canvasTrans.gameObject.SetActive(false);
    //     };
    // }
}
