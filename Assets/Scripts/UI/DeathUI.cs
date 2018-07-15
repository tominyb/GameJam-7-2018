using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathUI : MonoBehaviour
{
    public static DeathUI I;

    [SerializeField] private GameObject m_deathText;
    [SerializeField] private GameObject m_deathParticles;

    private void Awake()
    {
        I = this;
    }

    public void Die()
    {
        m_deathText.SetActive(true);
        m_deathParticles.SetActive(true);
    }
}
