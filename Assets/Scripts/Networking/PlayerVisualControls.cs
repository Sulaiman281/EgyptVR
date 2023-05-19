using System;
using Avatar;
using Database;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerVisualControls : NetworkBehaviour
{

    [SerializeField] private TabletPhone tablet;
    
    [Header("Avatars")] public NetworkVariable<int> avatarSelected;
    [SerializeField] private int avatarIgnoreLayer;
    public NetworkVariable<PlayerData> data;
    [SerializeField] private TMP_Text playerLabel;

    public int lessonsToTake;
    public int lessonsTaken;
    
    private GameObject _myAvatar;
    // local variables
    private PlayerMapRef _playerMapRef;
    
    private void Awake()
    {
        avatarSelected = new NetworkVariable<int>(0, readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Owner);
        data = new NetworkVariable<PlayerData>(new PlayerData
        {
            playerName = PlayerPrefs.GetString(LocalDataKey.UserName),
            points = 0,
            priorityState = PriorityState.Lessons
        });

        data.OnValueChanged += (oldValue, newValue) =>
        {
            playerLabel.text = $"({newValue.points}) {newValue.playerName}";
        };

        avatarSelected.OnValueChanged += (value, newValue) =>
        {
            if (IsLocalPlayer)
            {
                // SpawnAvatarServerRpc();
            }
        };
    }

    private void Start()
    {
        if (IsLocalPlayer)
        {
            GameManager.instance.localPlayer = this;
            avatarSelected.Value = PlayerPrefs.GetInt("AvatarKey");
            SpawnAvatarServerRpc();
            _playerMapRef = FindObjectOfType<PlayerMapRef>();
            SpawnTabletServerRpc();
            
            var pData = data.Value;
            pData.playerName = PlayerPrefs.GetString(LocalDataKey.UserName);
            data.Value = pData;
        }
    }

    private bool _hasAvatar;
    private AvatarMapping _avatarMapping;

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            var trans = _playerMapRef.transform;
            transform.SetPositionAndRotation(trans.position, trans.rotation);
        }
    }

    private void LateUpdate()
    {
        if (!_hasAvatar && IsOwner)
        {
            _avatarMapping = transform.GetComponentInChildren<AvatarMapping>();
            if (_avatarMapping == null) return;
            _avatarMapping.MapPlayerMap(_playerMapRef);
            _avatarMapping.transform.GetChild(0).GetChild(1).gameObject.layer = avatarIgnoreLayer;
            _hasAvatar = true;
        }
    }

    public void LessonTaken()
    {
        lessonsTaken++;

        var pData = data.Value;
        pData.points += 5;

        GameManager.instance.playerMapRef.phone.objectives[0] = $"Take Lesson ({lessonsTaken}/{lessonsToTake})";
        if (lessonsTaken >= lessonsToTake)
        {
            Debug.Log($"Lesson Updates {lessonsTaken} and {lessonsToTake}");
            GameManager.instance.playerMapRef.phone.UpdateObjectives(data.Value.priorityState);
            pData.priorityState = PriorityState.Quiz;
            data.Value = pData;
            return;
        }
        data.Value = pData;
        GameManager.instance.playerMapRef.phone.UpdateObjectives(data.Value.priorityState);
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
    
    [ServerRpc]
    private void SpawnTabletServerRpc()
    {
        var netObj = Instantiate(tablet).GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(OwnerClientId);
    }

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
    public void AddQuizPoints(int countPoints)
    {
        var pData = data.Value;
        pData.points = countPoints;
        pData.priorityState = PriorityState.Modeling;
        data.Value = pData;
        GameManager.instance.playerMapRef.phone.UpdateObjectives(data.Value.priorityState);
    }
}

[Serializable]
public struct PlayerData : INetworkSerializable
{
    public string playerName;
    public int points;
    public PriorityState priorityState;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref points);
        serializer.SerializeValue(ref priorityState);
    }
}

public enum PriorityState
{
    Lessons = 0,
    Quiz = 1,
    Modeling = 2,
}