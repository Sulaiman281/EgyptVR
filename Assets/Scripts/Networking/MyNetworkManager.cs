using Unity.Netcode;

public class MyNetworkManager : NetworkManager
{
    public bool classRoomHost;

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
}
