using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SceneDoor : MonoBehaviour
{
    [SerializeField] private Transform[] teleportationPoints;
    [SerializeField] private PlayerInput.ActionEvent onSceneDoorEnter;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.transform.position = teleportationPoints[Random.Range(0, teleportationPoints.Length)].position;
        onSceneDoorEnter.Invoke(default);
#if UNITY_EDITOR
        Debug.Log("Player Entered In Scene Door");
#endif
    }
}
