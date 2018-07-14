using System.Collections.Generic;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public readonly HashSet<int> ClientConnectionIds = new HashSet<int>();

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        ClientConnectionIds.Add(conn.connectionId);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        ClientConnectionIds.Remove(conn.connectionId);
    }
}
