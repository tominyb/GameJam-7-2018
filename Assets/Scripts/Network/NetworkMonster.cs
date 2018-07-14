using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkHealth))]
public class NetworkMonster : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.MonsterContainer m_monsters = null;
    [SyncVar] public int MonsterIndex;

    private NetworkHealth m_health;
    private int           m_damage;

    public override void OnStartClient()
    {
        base.OnStartClient();
        ScriptableObjects.Monster data        = m_monsters.Monsters[MonsterIndex];
        m_health                              = GetComponent<NetworkHealth>();
        m_health.MaxHealth                    = data.StartingHealth;
        m_damage                              = data.Damage;
        GetComponent<SpriteRenderer>().sprite = data.Sprite;
    }
}
