using UnityEngine;
using UnityEngine.Networking;

public class NetworkHealth : NetworkBehaviour
{
    [SyncVar] private int m_maxHealth = 100;
    [SyncVar] private int m_currentHealth = 50;

    public int MaxHealth
    {
        get { return m_maxHealth; }
        set
        {
            m_maxHealth = value;
            m_currentHealth = Mathf.Min(m_currentHealth, m_maxHealth);
        }
    }

    public int CurrentHealth
    {
        get { return m_currentHealth; }
        private set { m_currentHealth = Mathf.Clamp(value, 0, m_maxHealth); }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
    }

    public void RestoreHealth(int amount)
    {
        CurrentHealth += amount;
    }

    public bool IsDead()
    {
        return m_currentHealth <= 0;
    }
}
