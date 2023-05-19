using UnityEngine;
using UnityEngine.InputSystem;

public class SceneDoor : MonoBehaviour
{
    [SerializeField] private Transform teleportationPoint;
    [SerializeField] private PlayerInput.ActionEvent onSceneDoorEnter;
    [SerializeField] private Transform playerMovement;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerMovement.gameObject.SetActive(false);
        
        other.transform.position = teleportationPoint.position;
        onSceneDoorEnter.Invoke(default);

        StartCoroutine(GameManager.instance.DelayAction(()=>
            {
                playerMovement.gameObject.SetActive(true);
            },.5f));
// #if UNITY_EDITOR
        Debug.Log("Player Entered In Scene Door");
// #endif
    }
}
