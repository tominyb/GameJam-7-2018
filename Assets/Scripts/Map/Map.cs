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
    [SerializeField] private Sprite     m_groundSprite;
    [SerializeField] private Sprite     m_doorSprite;
    [SerializeField] private Sprite     m_openDoorSprite;

    private List<Bounds> m_roomBounds = new List<Bounds>();
    private float m_tileWidth;
    private float m_tileHeight;

    private const int RoomMinWidth     = 6;
    private const int RoomMaxWidth     = 10;
    private const int RoomMinHeight    = 4;
    private const int RoomMaxHeight    = 7;
    private const int MinNumberOfRooms = 5;
    private const int MaxNumberOfRooms = 8;
    private const int MinX             = -25;
    private const int MaxX             = 25;
    private const int MinY             = -25;
    private const int MaxY             = 25;

    private const float MaxDistanceToAnotherRoom = 3.0f;
    
    private Dictionary<Vector2Int, Tile> m_grid = new Dictionary<Vector2Int, Tile>();
    private Dictionary<TileType, Sprite> m_tileSprites;

    private void Awake()
    {
        I = this;
        Bounds tileBounds = m_tilePrefab.GetComponent<Renderer>().bounds;
        m_tileWidth       = tileBounds.size.x;
        m_tileHeight      = tileBounds.size.y;
        m_tileSprites     = new Dictionary<TileType, Sprite> { { TileType.Ground, m_groundSprite}, { TileType.Door, m_doorSprite},
                                                               { TileType.OpenDoor, m_openDoorSprite} };
        GenerateMap();
    }

    private void GenerateMap()
    {
        Random.InitState(5);
        int numberOfRooms = Random.Range(MinNumberOfRooms, MaxNumberOfRooms);
        for (int i = 0; i < numberOfRooms; ++i)
        {
            GenerateRoom();
        }
        GenerateCorridors();
    }

    private void GenerateRoom()
    {
        Bounds room       = new Bounds();
        bool roomFound    = false;
        int width = 0, height = 0, x = 0, y = 0;
        while (!roomFound)
        {
            width    = Random.Range(RoomMinWidth, RoomMaxWidth);
            height   = Random.Range(RoomMinHeight, RoomMaxHeight);
            x        = Random.Range(MinX, MaxX);
            y        = Random.Range(MinY, MaxY);
            room.min = new Vector2((x - 0.5f) * m_tileWidth, (y - 0.5f) * m_tileHeight);
            room.max = new Vector2((width + x - 0.5f) * m_tileWidth, (height + y - 0.5f) * m_tileHeight);

            if (DoesRoomIntersectWithAnother(room))
            {
                continue;
            }

            if (m_roomBounds.Count > 0 && !IsRoomCloseEnoughToAnotherRoom(room))
            {
                continue;
            }

            roomFound = true;
        }
        m_roomBounds.Add(room);
        GameObject roomObject       = new GameObject();
        roomObject.transform.parent = transform;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                GenerateTile(roomObject.transform, new Vector2Int(x + i, y + j));
            }
        }
    }

    private Tile GenerateTile(Transform tileParent, Vector2Int tilePosition, TileType tileType = TileType.Ground)
    {
        Tile tile = null;
        if (!m_grid.ContainsKey(tilePosition))
        {
            GameObject tileObject = Instantiate(m_tilePrefab, new Vector3(tilePosition.x * m_tileWidth, tilePosition.y * m_tileHeight, 0.0f),
                                                Quaternion.identity, tileParent);
            tile = new Tile(tileObject, tileType);
            m_grid.Add(tilePosition, tile);
        }
        return tile;
    }

    private bool DoesRoomIntersectWithAnother(Bounds room)
    {
        for (int i = 0; i < m_roomBounds.Count; ++i)
        {
            if (room.Intersects(m_roomBounds[i]))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsRoomCloseEnoughToAnotherRoom(Bounds room)
    {
        Vector3 closestPointToRoomA;
        Vector3 closestPointToRoomB;

        for (int i = 0; i < m_roomBounds.Count; ++i)
        {
            closestPointToRoomA = room.ClosestPoint(m_roomBounds[i].center);
            closestPointToRoomB = m_roomBounds[i].ClosestPoint(room.center);
            if (Vector3.Distance(closestPointToRoomA, closestPointToRoomB) < MaxDistanceToAnotherRoom)
            {
                return true;
            }
        }
        return false;
    }

    private void GenerateCorridor(Vector2Int startingPosition, Vector2Int targetPosition, Transform corridorParent)
    {
        Vector2Int currentPosition = startingPosition;
        Vector2Int direction       = targetPosition - startingPosition;
        List<Tile> corridorTiles   = new List<Tile>();
        Tile tile                  = null;

        for (int i = 0; i < Mathf.Abs(direction.x); ++i)
        {
            currentPosition += Vector2Int.right * Mathf.RoundToInt(Mathf.Sign(direction.x));
            tile = GenerateTile(corridorParent, currentPosition);
            if (tile != null)
            {
                corridorTiles.Add(tile);
            }
        }

        for (int j = 0; j < Mathf.Abs(direction.y); ++j)
        {
            currentPosition += Vector2Int.up * Mathf.RoundToInt(Mathf.Sign(direction.y));
            tile = GenerateTile(corridorParent, currentPosition);
            if (tile != null)
            {
                corridorTiles.Add(tile);
            }
        }
        
        if (Random.value > 0.5f)
        {
            corridorTiles[0].ChangeType(TileType.Door);
        }

        if (Random.value > 0.5f)
        {
            corridorTiles[corridorTiles.Count - 1].ChangeType(TileType.Door);
        }
    }

    private void GenerateCorridors()
    {
        Vector3 closestPointToRoomA;
        Vector3 closestPointToRoomB;

        GameObject corridors = new GameObject();
        corridors.name = "Corridors";
        corridors.transform.parent = transform;

        for (int i = 0; i < m_roomBounds.Count - 1; ++i)
        {
            for (int j = i + 1; j < m_roomBounds.Count; ++j)
            {
                closestPointToRoomA = m_roomBounds[i].ClosestPoint(m_roomBounds[j].center);
                closestPointToRoomB = m_roomBounds[j].ClosestPoint(m_roomBounds[i].center);

                if (Vector3.Distance(closestPointToRoomA, closestPointToRoomB) < MaxDistanceToAnotherRoom)
                {
                    GenerateCorridor(GetClosestTile(closestPointToRoomA), GetClosestTile(closestPointToRoomB), corridors.transform);
                }
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

    public Sprite GetSpriteByTileType(TileType type)
    {
        return m_tileSprites[type];
    }
}
