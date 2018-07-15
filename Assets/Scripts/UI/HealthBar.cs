using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Text m_valueText = null;
    private Slider m_slider = null;
    [HideInInspector] public NetworkHealth Health = null;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
        m_slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    private void Update()
    {
        if (Health != null)
        {
            m_slider.maxValue = Health.MaxHealth;
            m_slider.value = Health.CurrentHealth;
        }
    }

    private void OnValueChanged()
    {
        m_valueText.text = Mathf.RoundToInt(m_slider.value) + "/" + Mathf.RoundToInt(m_slider.maxValue);
    }
}
