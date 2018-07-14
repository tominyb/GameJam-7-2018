using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkHealth))]
public class NetworkMonster : NetworkBehaviour
{
    private NetworkHealth m_health;

    private void Awake()
    {
        m_health = GetComponent<NetworkHealth>();
    }
}
