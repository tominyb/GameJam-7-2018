using UnityEngine;

namespace ScriptableObjects {

[CreateAssetMenu(fileName = "ItemContainer", menuName = "ScriptableObjects/ItemContainer", order = 0)]
public class ItemContainer : ScriptableObject
{
    public Item[] Items;

    public int GetRandomItemIndex()
    {
        return Random.Range(0, Items.Length);
    }
}

} // namespace ScriptableObjects
