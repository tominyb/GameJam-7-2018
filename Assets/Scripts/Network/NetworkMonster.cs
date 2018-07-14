using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(NetworkHealth))]
public class NetworkMonster : NetworkBehaviour
{
    private NetworkHealth m_health;

    private void Awake()
    {
        m_health = GetComponent<NetworkHealth>();
    }
}
