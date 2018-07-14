using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObjects/Monster", order = 3)]
    public class Monster : ScriptableObject
    {
        public Sprite Sprite;
        public int    StartingHealth;
        public int    Damage;
    }
}
