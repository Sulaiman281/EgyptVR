using UnityEngine;
using UnityEngine.InputSystem;

public class TabelBoxTrigger : MonoBehaviour
{
    [SerializeField] private PlayerInput.ActionEvent onPlayerCanDoQuizzes;
    [SerializeField] private PriorityState playerState;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.instance.localPlayer == null) return;
        if (GameManager.instance.localPlayer.data.Value.priorityState == playerState)
        {
            onPlayerCanDoQuizzes.Invoke(default);
        }
    }
}
