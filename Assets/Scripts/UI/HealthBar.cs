using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider m_slider = null;
    public NetworkHealth Health = null;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (Health != null)
        {
            m_slider.maxValue = Health.MaxHealth;
            m_slider.value = Health.CurrentHealth;
        }
    }
}
