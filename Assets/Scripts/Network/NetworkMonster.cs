using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkHealth))]
public class NetworkMonster : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.MonsterContainer m_monsters = null;
    [HideInInspector] [SyncVar] public int MonsterIndex = -1;

    [HideInInspector] public NetworkHealth Health = null;
    private int m_damage = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();
        ScriptableObjects.Monster data        = m_monsters.Monsters[MonsterIndex];
        Health                                = GetComponent<NetworkHealth>();
        Health.MaxHealth                      = data.StartingHealth;
        m_damage                              = data.Damage;
        GetComponent<SpriteRenderer>().sprite = data.Sprite;
    }
}
