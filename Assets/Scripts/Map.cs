using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public static Map I;

    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private GameObject m_tilePrefab;
    [SerializeField] private Vector2Int m_gridSize;
    
    private Dictionary<Vector2Int, Tile> m_grid = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        I = this;
        GenerateMap();
        Instantiate(m_playerPrefab);
    }

    private void GenerateMap()
    {
        Bounds tileBounds = m_tilePrefab.GetComponent<Renderer>().bounds;
        for (int i = 0; i < m_gridSize.x; ++i)
        {
            for (int j = 0; j < m_gridSize.y; ++j)
            {
                GameObject tileObject = Instantiate(m_tilePrefab, new Vector3(i * tileBounds.size.x, j * tileBounds.size.y, 0.0f ),
                                                    Quaternion.identity, transform);
                m_grid.Add(new Vector2Int(i, j), new Tile(tileObject, TileType.Ground));
            }
        }
    }

    public Tile GetTile(Vector2Int position)
    {
        Tile tile = null;
        m_grid.TryGetValue(position, out tile);
        return tile;
    }
}
