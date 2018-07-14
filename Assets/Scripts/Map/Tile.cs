using UnityEngine;
using System.Collections;

public class Tile
{
    public GameObject Sprite;
    public TileType   Type;

    public Tile(GameObject sprite, TileType type) { Sprite = sprite; Type = type; }

    public void ChangeType(TileType type, Sprite sprite)
    {
        Type                                         = type;
        Sprite.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
