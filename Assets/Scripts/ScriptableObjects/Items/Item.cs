using UnityEngine;

namespace ScriptableObjects {

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public Sprite Sprite;
    public int HealthRestoreAmount;
}

} // namespace ScriptableObjects
