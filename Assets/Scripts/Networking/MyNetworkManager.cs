using Unity.Netcode;

public class MyNetworkManager : NetworkManager
{
    public bool classRoomHost;

    private NetworkEvents _events;

    private void Start()
    {
        _events = GetComponent<NetworkEvents>();
    }

    public void StartClass()
    {
        classRoomHost = true;
        StartHost();
    }

    public void JoinClass()
    {
        classRoomHost = false;
        StartClient();
    }

    private void OnServerInitialized()
    {
        _events.onServerStarted.Invoke(default);
    }

    private void OnConnectedToServer()
    {
        _events.onClientStarted.Invoke(default);
    }
    
}
