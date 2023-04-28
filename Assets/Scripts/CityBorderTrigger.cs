using UnityEngine;
using UnityEngine.InputSystem;

public class CityBorderTrigger : MonoBehaviour
{
    [SerializeField] private PlayerInput.ActionEvent onBorderExit;
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        onBorderExit.Invoke(default);
#if UNITY_EDITOR
        Debug.Log("Player Exit City Border");
#endif
    }
}
