using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkHealth))]
public class NetworkMonster : NetworkBehaviour
{
    private NetworkHealth m_health;
    private int           m_damage;

    private void Awake()
    {

    }

    public void Initialize(ScriptableObjects.Monster data)
    {
        m_health                              = GetComponent<NetworkHealth>();
        m_health.MaxHealth                    = data.StartingHealth;
        m_damage                              = data.Damage;
        GetComponent<SpriteRenderer>().sprite = data.Sprite;
    }
}
