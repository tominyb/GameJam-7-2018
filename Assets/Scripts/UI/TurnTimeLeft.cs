using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimeLeft : MonoBehaviour
{
    [SerializeField] private Gradient m_sliderColorGradient;
    [SerializeField] private Slider m_slider;
    private Image m_sliderFillImage;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
        m_slider.onValueChanged.AddListener(delegate { UpdateSliderColorAccordingToValuePercentage(); });
        m_sliderFillImage = m_slider.fillRect.GetComponent<Image>();
        StartCoroutine(TestSliderColorChangeAtInterval(1f));
    }

    private IEnumerator TestSliderColorChangeAtInterval(float interval)
    {
        while (true)
        {
            RandomizeSliderValue();
            yield return new WaitForSeconds(interval);
        }
    }

    private void RandomizeSliderValue()
    {
        m_slider.value = Random.Range(m_slider.minValue, m_slider.maxValue);
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
