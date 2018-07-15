using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider m_slider = null;
    public NetworkHealth Health = null;
    private int m_previousMaxHealth = 0;
    private int m_previousHealth = 0;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (Health == null)
        {
            return;
        }

        m_slider.maxValue = Health.MaxHealth;
        m_slider.value = Health.CurrentHealth;
    }
}
