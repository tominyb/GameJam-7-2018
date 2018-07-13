using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector2Int m_position;

    private readonly Dictionary<KeyCode, Vector2Int> m_movementVectors = new Dictionary<KeyCode, Vector2Int>
    {
        { KeyCode.Keypad1, Vector2Int.left + Vector2Int.down },
        { KeyCode.Keypad2, Vector2Int.down                   },
        { KeyCode.Keypad3, Vector2Int.right + Vector2Int.down},
        { KeyCode.Keypad4, Vector2Int.left                   },
        { KeyCode.Keypad6, Vector2Int.right                  },
        { KeyCode.Keypad7, Vector2Int.left + Vector2Int.up   },
        { KeyCode.Keypad8, Vector2Int.up                     },
        { KeyCode.Keypad9, Vector2Int.right + Vector2Int.up  }
    };

    private void Start()
    {
        MovePlayer(Vector2Int.zero);
    }

    private void MovePlayer(Vector2Int deltaPosition)
    {
        Vector2Int newPosition = m_position + deltaPosition;
        Tile tile = Map.I.GetTile(newPosition);
        if (tile != null && tile.Type == TileType.Ground)
        {
            transform.position = tile.Sprite.transform.position;
            m_position = newPosition;
        }
    }

    private void Update()
    {
        foreach (var vector in m_movementVectors)
        {
            if (Input.GetKeyDown(vector.Key))
            {
                MovePlayer(vector.Value);
            }
        }
    }
}
