using UnityEngine;

public static class Damage
{
    public static int GetDamage(int attack)
    {
        return attack + Random.Range(1, 7) * 2;
    }
}
