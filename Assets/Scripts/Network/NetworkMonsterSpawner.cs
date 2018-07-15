using UnityEngine;
using UnityEngine.Networking;

// TODO: Refactor to share a common base class or interface with NetworkItemSpawner? Currently contains duplicate code.
public class NetworkMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.MonsterContainer m_monsters;
    [SerializeField] private GameObject m_monsterPrefab;
    [SerializeField] [Range(0, 1)] private float m_monsterProbability = 0.05f;
    private Map m_map = null;

    private void Start()
    {
        m_map = Map.I;
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        var possibleSpawns = m_map.GetPositionTilePairsOfType(TileType.Ground);
        int monsterCount = Mathf.RoundToInt(possibleSpawns.Count * m_monsterProbability);
        for (int i = 0; i < monsterCount; ++i)
        {
            int possibleSpawnIndex = Random.Range(0, possibleSpawns.Count);
            var spawn = possibleSpawns[possibleSpawnIndex];
            SpawnMonster(spawn.Key, spawn.Value);
            possibleSpawns.RemoveAt(possibleSpawnIndex);
        }
    }

    private void SpawnMonster(Vector2Int position, Tile tile)
    {
        GameObject monsterObject = Instantiate(m_monsterPrefab, tile.Sprite.transform.position, Quaternion.identity);
        NetworkMonster monster = monsterObject.GetComponent<NetworkMonster>();
        monster.MonsterIndex = m_monsters.GetRandomMonsterIndex();
        NetworkServer.Spawn(monsterObject);
        m_map.AddMonsterAtPosition(monster, position);
    }
}
