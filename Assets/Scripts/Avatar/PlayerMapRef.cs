using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMapRef : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    
    #region Tablet
    [Header("Tablet")]
    [SerializeField] private XRSocketInteractor phoneSocket;
    public TabletPhone phone;

    public void PutTabletIntoSocket()
    {
        phone.transform.SetPositionAndRotation(phoneSocket.transform.position, Quaternion.identity);
    }


    #endregion
}
