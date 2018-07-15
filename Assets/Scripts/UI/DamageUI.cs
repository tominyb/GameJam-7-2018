using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DamageUI : MonoBehaviour
{
    [SerializeField] private float m_effectDuration = 3f;
    [SerializeField] private float m_effectOffset = 40f;
    private float m_effectTime = 0f;
    private Text m_text = null;
    private Vector3 m_initialWorldPosition = Vector3.zero;

    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

    public void InitEffect(int damage, Vector3 damagedEntityWorldPosition)
    {
        m_text.text = (-damage).ToString();
        m_text.CrossFadeAlpha(0f, m_effectDuration, false);
        m_initialWorldPosition = damagedEntityWorldPosition;
        UpdatePosition();
    }

    private void Update()
    {
        m_effectTime += Time.deltaTime;
        if (m_effectTime >= m_effectDuration)
        {
            Destroy(gameObject);
            return;
        }
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position =
            Camera.main.WorldToScreenPoint(m_initialWorldPosition)
            + ((m_effectTime / m_effectDuration) * m_effectOffset * Vector3.up);
    }
}
