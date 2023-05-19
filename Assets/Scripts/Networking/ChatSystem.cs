using DefaultNamespace;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ChatSystem : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputMsg;

    [SerializeField] private MsgBox msgBoxPrefab;
    [SerializeField] private Transform msgBoxParent;
    

    public void SendMessage()
    {
        if (string.IsNullOrEmpty(inputMsg.text)) return;
        SendMessageServerRpc(GameManager.instance.localPlayer.data.Value.playerName, inputMsg.text);
        inputMsg.text = "";
    }

    [ServerRpc]
    public void SendMessageServerRpc(string playerName, string msg)
    {
        ReceiveMessageClientRpc(playerName, msg);
    }

    [ClientRpc]
    public void ReceiveMessageClientRpc(string playerName, string msg)
    {
        var msgBox = Instantiate(msgBoxPrefab, msgBoxParent);
        msgBox.senderName = playerName;
        msgBox.msgText = msg;
    }
}
