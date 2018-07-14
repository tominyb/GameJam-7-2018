using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Potion : MonoBehaviour
{
    [SerializeField] private ScriptableObjects.PotionContainer m_potions = null;
    private ScriptableObjects.Potion m_potion = null;

    private void Start()
    {
        m_potion = m_potions.GetRandomPotion();
        GetComponent<SpriteRenderer>().sprite = m_potion.sprite;
    }
}
