using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkEvents : MonoBehaviour
{
    public PlayerInput.ActionEvent onServerStarted;
    public PlayerInput.ActionEvent onClientStarted;
    public PlayerInput.ActionEvent onConnectionJoin;
    public PlayerInput.ActionEvent onConnectionLeft;
}
