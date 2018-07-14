using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public static Map I;

    [SerializeField] private GameObject m_tilePrefab;
    [SerializeField] private Vector2Int m_gridSize;

    private float m_tileWidth;
    private float m_tileHeight;

    private const int RoomMinWidth  = 6;
    private const int RoomMaxWidth  = 10;
    private const int RoomMinHeight = 4;
    private const int RoomMaxHeight = 7;
    
    private Dictionary<Vector2Int, Tile> m_grid = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        I = this;
        Bounds tileBounds = m_tilePrefab.GetComponent<Renderer>().bounds;
        m_tileWidth       = tileBounds.size.x;
        m_tileHeight      = tileBounds.size.y;
        GenerateMap();
    }

    private void GenerateMap()
    {
        int x = Random.Range(-20, 20);
        int y = Random.Range(-20, 20);
        GenerateRoom(new Vector2Int(x, y));
    }

    private void GenerateRoom(Vector2Int position)
    {
        int width             = Random.Range(RoomMinWidth, RoomMaxWidth);
        int height            = Random.Range(RoomMinHeight, RoomMaxHeight);
        GameObject room       = new GameObject();
        room.transform.parent = transform;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                GameObject tileObject = Instantiate(m_tilePrefab, new Vector3((position.x + i) * m_tileWidth, 
                                                   (position.y + j) * m_tileHeight, 0.0f),
                                                    Quaternion.identity, room.transform);
                m_grid.Add(new Vector2Int(i + position.x, j + position.y), new Tile(tileObject, TileType.Ground));
            }
        }
    }

    public Tile GetTile(Vector2Int position)
    {
        Tile tile = null;
        m_grid.TryGetValue(position, out tile);
        return tile;
    }

    public Vector2Int GetClosestTile(Vector3 position)
    {
        float distance = Mathf.Infinity;
        Vector2Int closestTile = Vector2Int.zero;
        foreach (var entry in m_grid)
        {
            float newDistance = Vector3.Distance(position, entry.Value.Sprite.transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                closestTile = entry.Key;
            }
        }
        return closestTile;
    }
}
