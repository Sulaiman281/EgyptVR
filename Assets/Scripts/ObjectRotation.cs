using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed;
    private void LateUpdate()
    {
        transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime);
    }
}
