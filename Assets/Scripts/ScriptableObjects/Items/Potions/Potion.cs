using UnityEngine;

namespace ScriptableObjects {

[CreateAssetMenu(fileName = "Potion", menuName = "ScriptableObjects/Potion", order = 1)]
public class Potion : ScriptableObject
{
    public Sprite sprite;
    public int healthRestoreAmount;
}

} // namespace ScriptableObjects
