using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

public class ServerHostFinder : MonoBehaviour
{
    [SerializeField] private UnityTransport transport;

    private void Start()
    {
        // transport.ConnectionData.ListenEndPoint.
        PingHost("192.168.100.1", 7777);
    }
    
    private void PingHost(string ip, int port)
    {
        {
            Ping pingSender = new Ping();
            IPAddress ipAddress = IPAddress.Parse(ip);
            PingReply reply = pingSender.Send(ipAddress, 1000); // 1000ms timeout
            if (reply.Status == IPStatus.Success)
            {
                Debug.Log(reply.Status);
                Debug.Log("Ping successful to " + ip + ":" + port);
            }
            else
            {
                Debug.Log("Ping failed to " + ip + ":" + port);
            }
        }
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface iface in interfaces)
        {
            if (iface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && iface.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties ipProps = iface.GetIPProperties();
                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    Debug.Log(addr.Address.ToString());
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        var ipAddress = addr.Address.ToString();
                        Debug.Log(ipAddress);
                        // Use this IP address to connect to the game hosted by the other player
                        break;
                    }
                }
            }
        }

        {
            Debug.Log(transport.ConnectionData.ListenEndPoint.Family);
        }
    }

    // private void Update()
    // {
    //     Debug.Log(transport.ConnectionData.ListenEndPoint.WithPort(7777).Length);
    //     Debug.Log(transport.ConnectionData.ServerEndPoint);
    // }
}
