using Avatar;
using Unity.Netcode;
using UnityEngine;

public class PlayerVisualControls : NetworkBehaviour
{
    [Header("Avatars")] public NetworkVariable<int> avatarSelected;

    private GameObject _myAvatar;
    private AvatarAnimation _avatarAnimation;

    // local variables
    private PlayerMapRef _playerMapRef;

    private void Awake()
    {
        avatarSelected = new NetworkVariable<int>(0, readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Owner);

        avatarSelected.OnValueChanged += (value, newValue) =>
        {
            // if (IsLocalPlayer)
            // {
            //     SpawnAvatarServerRpc();
            // }
        };
    }

    private void Start()
    {
        if (IsLocalPlayer)
        {
            avatarSelected.Value = PlayerPrefs.GetInt("AvatarKey");
            SpawnAvatarServerRpc();
            _playerMapRef = FindObjectOfType<PlayerMapRef>();
        }
    }

    [ServerRpc]
    public void SpawnAvatarServerRpc()
    {
        var avatars = Resources.Load<Avatars>("Avatars");
        var avatarObj = avatars.avatarsNetworkPrefab[avatarSelected.Value];
        _myAvatar = Instantiate(avatarObj);
        var avatarNetwork = _myAvatar.GetComponent<NetworkObject>();
        avatarNetwork.Spawn();
        var clientNetworkTransforms = _myAvatar.GetComponentsInChildren<ClientNetworkTransform>();
        foreach (var clientNetworkTransform in clientNetworkTransforms)
        {
            clientNetworkTransform.enabled = false;
        }
        avatarNetwork.TrySetParent(transform);
        foreach (var clientNetworkTransform in clientNetworkTransforms)
        {
            clientNetworkTransform.enabled = true;
        }
        avatarNetwork.ChangeOwnership(OwnerClientId);
    }
    //
    // [ClientRpc]
    // public void ControlAvatarClientRpc()
    // {
    //     var avatar = GetComponentInChildren<AvatarNetworkAnimation>();
    //     _myAvatar = avatar.gameObject;
    //     if (IsLocalPlayer)
    //     {
    //         avatar.ControlsSetup();
    //     }
    // }
}