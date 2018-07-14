using UnityEngine;
using UnityEngine.Networking;

public class NetworkMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkMonster m_monsterPrefab;
    [SerializeField] private ScriptableObjects.Monster[] monsterDatas;

    private void Start()
    {
        for (int i = 0; i < 10; ++i)
        {
            int x = Random.Range(-20, 20);
            int y = Random.Range(-20, 20);
            NetworkMonster monster = Instantiate(m_monsterPrefab, new Vector3(x, y, 0), Quaternion.identity);
            NetworkServer.Spawn(monster.gameObject);
        }
    }
}
