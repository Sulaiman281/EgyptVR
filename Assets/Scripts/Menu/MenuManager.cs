using System;
using System.Collections.Generic;
using Database;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Events")] [SerializeField] private PlayerInput.ActionEvent onSceneStart;
    [SerializeField] private PlayerInput.ActionEvent onSignUp;

    [Header("Profile Setup")] [SerializeField]
    private TMP_InputField nameInput;

    [Header("Join Class Code")] [SerializeField]
    private TMP_InputField codeInputField;

    [SerializeField] private PlayerInput.ActionEvent onRelayConnectionPass;
    [SerializeField] private PlayerInput.ActionEvent onRelayConnectionFailed;

    private void Start()
    {
        onSceneStart.Invoke(default);
        if (PlayerPrefs.HasKey(LocalDataKey.UserName))
        {
            var userName = PlayerPrefs.GetString(LocalDataKey.UserName);
            nameInput.text = userName;
        }
    }

    public async void StartClass()
    {
        PlayerPrefs.SetString("NetworkStatus", "Server");
        PlayerPrefs.Save();
        SceneManager.LoadScene("ClassRoom");
        // try
        // {
        //     var allocation = await RelayService.Instance.CreateAllocationAsync(5);
        //     var code = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        //     onRelayConnectionPass.Invoke(default);
        //     var relayServerData = new RelayServerData(allocation, "dtls");
        //     var roomScene = SceneManager.LoadSceneAsync("ClassRoom");
        //     roomScene.completed += _ =>
        //     {
        //         try
        //         {
        //             Debug.Log(code);
        //             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        //             NetworkManager.Singleton.StartHost();
        //             if (GameManager.instance != null)
        //                 GameManager.instance.joinCode = code;
        //         }
        //         catch (Exception)
        //         {
        //             SceneManager.LoadScene("Menu");
        //         }
        //
        //     };
        //     NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        // }
        // catch (RelayServiceException)
        // {
        //     onRelayConnectionFailed.Invoke(default);
        // }
    }

    public async void JoinClass()
    {
        if (string.IsNullOrEmpty(codeInputField.text)) return;
        try
        {
            var code = codeInputField.text.ToUpper();
            var relayAllocation = await RelayService.Instance.JoinAllocationAsync(code);
            if (relayAllocation == null)
            {
                onRelayConnectionFailed.Invoke(default);
                return;
            }

            onRelayConnectionPass.Invoke(default);
            PlayerPrefs.SetString("NetworkStatus", code);
            PlayerPrefs.Save();
            SceneManager.LoadScene("ClassRoom");
            // var roomScene = SceneManager.LoadSceneAsync("ClassRoom");
            // roomScene.completed += _ =>
            // {
            //     var relayServerData = new RelayServerData(relayAllocation, "dtls");
            //
            //     NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            //     NetworkManager.Singleton.StartClient();
            //     if (GameManager.instance != null)
            //         GameManager.instance.joinCode = code;
            // };
        }
        catch (RelayServiceException)
        {
            onRelayConnectionFailed.Invoke(default);
        }
    }

    public async void SaveNameInput()
    {
        await CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object>
        {
            { LocalDataKey.UserName, nameInput.text }
        });
        // saving local
        PlayerPrefs.SetString(LocalDataKey.UserName, nameInput.text);
        PlayerPrefs.Save();
    }

    public async void SignIn() // authentication automatically
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            PlayerPrefs.SetString(LocalDataKey.PlayerId, AuthenticationService.Instance.PlayerId);
            PlayerPrefs.Save();
            onSignUp.Invoke(default);
        }
        catch
        {
            // ignored
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}