using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector2Int m_position;

    private Vector2Int[] m_movementVectors = new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),
                                                                new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(-1, -1),
                                                                new Vector2Int(0, -1), new Vector2Int(1, -1)};

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
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            MovePlayer(m_movementVectors[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            MovePlayer(m_movementVectors[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            MovePlayer(m_movementVectors[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            MovePlayer(m_movementVectors[3]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            MovePlayer(m_movementVectors[4]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            MovePlayer(m_movementVectors[5]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            MovePlayer(m_movementVectors[6]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            MovePlayer(m_movementVectors[7]);
        }
    }
}
