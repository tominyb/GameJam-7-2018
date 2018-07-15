using UnityEngine;
using UnityEngine.Networking;

public class NetworkItemSpawner : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.ItemContainer m_items = null;
    [SerializeField] private GameObject m_itemPrefab = null;
    [SerializeField] [Range(0, 1)] private float m_itemProbability = 0.05f;
    private Map m_map = null;

    private void Start()
    {
        m_map = Map.I;
        SpawnItems();
    }

    private void SpawnItems()
    {
        var possibleSpawns = m_map.GetPositionTilePairsOfType(TileType.Ground);
        int itemCount = Mathf.RoundToInt(possibleSpawns.Count * m_itemProbability);
        for (int i = 0; i < itemCount; ++i)
        {
            int possibleSpawnIndex = Random.Range(0, possibleSpawns.Count);
            var spawn = possibleSpawns[possibleSpawnIndex];
            SpawnItem(spawn.Key, spawn.Value);
            possibleSpawns.RemoveAt(possibleSpawnIndex);
        }
    }

    private void SpawnItem(Vector2Int position, Tile tile)
    {
        GameObject itemObject = Instantiate(m_itemPrefab, tile.Sprite.transform.position, Quaternion.identity);
        NetworkItem item = itemObject.GetComponent<NetworkItem>();
        item.ItemIndex = m_items.GetRandomItemIndex();
        NetworkServer.Spawn(itemObject);
        m_map.AddItemAtPosition(item, position);
    }
}
