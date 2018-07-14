using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    private readonly HashSet<int> m_clientIds = new HashSet<int>();
    public HashSet<int> ClientIds { get { return m_clientIds; } }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        m_clientIds.Add(conn.connectionId);
        Debug.Log("Client " + conn.connectionId + " connected!");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        m_clientIds.Remove(conn.connectionId);
        Debug.Log("Client " + conn.connectionId + " disconnected!");
    }
}
