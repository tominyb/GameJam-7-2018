using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2Int m_position;

    private void Start()
    {
        m_position = Map.I.GetClosestTile(transform.position);
    }

    public void Move(Vector2Int deltaPosition)
    {
        Vector2Int newPosition = m_position + deltaPosition;
        Tile tile = Map.I.GetTile(newPosition);
        if (tile != null && tile.Type == TileType.Ground)
        {
            transform.position = tile.Sprite.transform.position;
            m_position         = newPosition;
        }
    }
}
