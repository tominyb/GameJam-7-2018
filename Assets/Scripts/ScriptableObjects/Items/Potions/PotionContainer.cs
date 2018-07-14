using UnityEngine;

namespace ScriptableObjects {

[CreateAssetMenu(fileName = "PotionContainer", menuName = "ScriptableObjects/PotionContainer", order = 0)]
public class PotionContainer : ScriptableObject
{
    public Potion[] potions;

    public Potion GetRandomPotion()
    {
        return potions[Random.Range(0, potions.Length)];
    }
}

} // namespace ScriptableObjects
