﻿using UnityEngine;
using UnityEngine.Networking;

public class NetworkItemSpawner : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.ItemContainer m_items = null;
    [SerializeField] private GameObject m_itemPrefab = null;
    [SerializeField] [Range(0, 1)] private float m_itemProbability = 0.05f;

    private void Start()
    {
        var possibleSpawns = Map.I.GetTilesOfType(TileType.Ground);
        int itemCount = Mathf.RoundToInt(possibleSpawns.Count * m_itemProbability);
        for (int i = 0; i < itemCount; ++i)
        {
            int possibleSpawnIndex = Random.Range(0, possibleSpawns.Count);
            SpawnItemOnTile(possibleSpawns[possibleSpawnIndex]);
            possibleSpawns.RemoveAt(possibleSpawnIndex);
        }
    }

    private void SpawnItemOnTile(Tile tile)
    {
        GameObject itemObject =
            Instantiate(m_itemPrefab, tile.Sprite.transform.position, Quaternion.identity);
        itemObject.GetComponent<NetworkItem>().ItemIndex = m_items.GetRandomItemIndex();
        NetworkServer.Spawn(itemObject);
    }
}
