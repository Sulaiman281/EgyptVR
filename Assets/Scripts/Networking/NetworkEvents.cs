using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkEvents : MonoBehaviour
{
    public PlayerInput.ActionEvent onServerStarted;
    public PlayerInput.ActionEvent onClientStarted;
    public PlayerInput.ActionEvent onConnectionJoin;
    public PlayerInput.ActionEvent onConnectionLeft;

    private void Start()
    {
        NetworkManager.Singleton.OnTransportFailure += OnConnectionFailed;
    }

    private void OnConnectionFailed()
    {
        Debug.Log("Connection Failed");
    }
}
