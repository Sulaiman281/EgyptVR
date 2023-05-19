using System.Linq;
using HurricaneVR.Framework.Core.UI;
using UnityEngine;

public class AllSceneCanvas : MonoBehaviour
{
    [SerializeField] private HVRInputModule hvrInputModule;
    private void OnValidate()
    {
        if (hvrInputModule == null) hvrInputModule = GetComponent<HVRInputModule>();
        hvrInputModule.UICanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None).ToList();
    }
}
