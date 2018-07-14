using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    private readonly HashSet<int> m_clientConnectionIds = new HashSet<int>();
    public HashSet<int> ClientConnectionIds { get { return m_clientConnectionIds; } }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        m_clientConnectionIds.Add(conn.connectionId);
        Debug.Log("Client " + conn.connectionId + " connected!");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        m_clientConnectionIds.Remove(conn.connectionId);
        Debug.Log("Client " + conn.connectionId + " disconnected!");
    }
}
