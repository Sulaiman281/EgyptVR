using UnityEngine;

public class FixCamera : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private void OnValidate()
    {
        if(canvas == null)
            if (TryGetComponent(out canvas))
            {
                if (canvas.worldCamera == null)
                {
                    canvas.worldCamera = Camera.main;
                }
            }
    }
}
