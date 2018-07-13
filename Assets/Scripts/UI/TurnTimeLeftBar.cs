using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimeLeftBar : MonoBehaviour
{
    [SerializeField] private Gradient m_sliderColorGradient;
    [SerializeField] private Slider m_slider;
    private Image m_sliderFillImage;

    public float TurnTime { set { m_slider.maxValue = value; } }
    public float TurnTimeLeft { set { m_slider.value = value; } }

    private void Start()
    {
        m_slider = GetComponent<Slider>();
        m_slider.onValueChanged.AddListener(delegate { UpdateSliderColorAccordingToValuePercentage(); });
        m_sliderFillImage = m_slider.fillRect.GetComponent<Image>();
    }

    private void UpdateSliderColorAccordingToValuePercentage()
    {
        m_sliderFillImage.color = m_sliderColorGradient.Evaluate(GetSliderValuePercentage());
    }

    private float GetSliderValuePercentage()
    {
        return (m_slider.value - m_slider.minValue) / (m_slider.maxValue - m_slider.minValue);
    }
}
