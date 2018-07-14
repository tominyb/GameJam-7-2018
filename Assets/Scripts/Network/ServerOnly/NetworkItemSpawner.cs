using UnityEngine;
using UnityEngine.Networking;

public class NetworkItemSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject m_itemPrefab = null;
    [SerializeField] [Range(0, 1)] private float m_itemProbability = 0.05f;

    private void Start()
    {
        var possibleSpawns = Map.I.GetTilesOfType(TileType.Ground);
        int itemCount = Mathf.RoundToInt(possibleSpawns.Count * m_itemProbability);
        for (int i = 0; i < itemCount; ++i)
        {
            int possibleSpawnIndex = Random.Range(0, possibleSpawns.Count);
            Tile spawnTile = possibleSpawns[possibleSpawnIndex];
            GameObject itemObject =
                Instantiate(m_itemPrefab, spawnTile.Sprite.transform.position, Quaternion.identity);
            NetworkServer.Spawn(itemObject);
            possibleSpawns.RemoveAt(possibleSpawnIndex);
        }
    }
}
