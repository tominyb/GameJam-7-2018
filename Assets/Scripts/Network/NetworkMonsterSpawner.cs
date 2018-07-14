using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.MonsterContainer m_monsters;
    [SerializeField] private NetworkMonster m_monsterPrefab;

    private void Start()
    {
        List<Tile> availableTiles = Map.I.GetTilesOfType(TileType.Ground);
        int amountOfMonsters = (int) (availableTiles.Count * Random.Range(0.05f, 0.1f));

        for (int i = 0; i < amountOfMonsters; ++i)
        {
            int index = Random.Range(0, availableTiles.Count);
            Tile spawnTile = availableTiles[index];
            availableTiles.RemoveAt(index);
            NetworkMonster monster = Instantiate(m_monsterPrefab, spawnTile.Sprite.transform.position, Quaternion.identity);
            monster.MonsterIndex = m_monsters.GetRandomMonsterIndex();
            NetworkServer.Spawn(monster.gameObject);
        }
    }
}
