using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SpriteRenderer))]
public class NetworkItem : NetworkBehaviour
{
    public void Initialize(ScriptableObjects.Item data)
    {
        GetComponent<SpriteRenderer>().sprite = data.Sprite;
    }
}
