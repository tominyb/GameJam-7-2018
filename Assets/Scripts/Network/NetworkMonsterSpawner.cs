using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject m_monsterPrefab;

    private void Start()
    {
        for (int i = 0; i < 10; ++i)
        {
            int x = Random.Range(-20, 20);
            int y = Random.Range(-20, 20);
            GameObject monster = Instantiate(m_monsterPrefab, new Vector3(x, y, 0), Quaternion.identity);
            NetworkServer.Spawn(monster);
        }
    }
}
