using Nakama;
using UnityEngine;

namespace Game.Backend
{
    public class NakamaConnection : MonoBehaviour
    {
        private string _scheme = "http";
        private string _host = "127.0.0.1";
        private int _port = 7350;               // Got from Docker Compose yml file
        private string _serverKey = "defaultkey";
        
        private IClient _client;
        private ISession _session;
        private ISocket _socket;

        private async void Start()
        {
            _client = new Client(_scheme, _host, _port, _serverKey, UnityWebRequestAdapter.Instance);
            Debug.Log("[Nakama Backend] Starting Nakama Client");

            _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            Debug.Log($"[Nakama Backend] Client authenticated! Session is {_session.UserId}");

            _socket = _client.NewSocket();
            await _socket.ConnectAsync(_session, true);
            Debug.Log($"[Nakama Backend] Socket Created and Connected!");
            Debug.Log($"[Nakama Backend] Socket: {_socket}");
            Debug.Log($"[Nakama Backend] Session: {_session}");
        }
    }
}