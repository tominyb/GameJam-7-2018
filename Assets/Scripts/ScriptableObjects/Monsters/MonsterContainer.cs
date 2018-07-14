using UnityEngine;

namespace ScriptableObjects {

[CreateAssetMenu(fileName = "MonsterContainer", menuName = "ScriptableObjects/MonsterContainer", order = 2)]
public class MonsterContainer : ScriptableObject
{
    public Monster[] Monsters;

    public int GetRandomMonsterIndex()
    {
        return Random.Range(0, Monsters.Length);
    }
}

} // namespace ScriptableObjects
