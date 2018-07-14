using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public readonly HashSet<int> ClientConnectionIds = new HashSet<int>();
    public readonly Dictionary<int, NetworkPlayer> ClientPlayers = new Dictionary<int, NetworkPlayer>();

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        ClientConnectionIds.Add(conn.connectionId);
        // ClientPlayers is updated in OnStartServer of NetworkPlayer.
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        int connectionId = conn.connectionId;
        ClientConnectionIds.Remove(connectionId);
        ClientPlayers.Remove(connectionId);
    }
}
