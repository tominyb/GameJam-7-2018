using UnityEngine;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private TurnTimeLeftBar m_turnTimeLeftBar;
    [SerializeField] private TurnTooltip m_turnTooltip;

    private float TurnTime { set { m_turnTimeLeftBar.TurnTime = value; } }
    public float TurnTimeLeft { set { m_turnTimeLeftBar.TurnTimeLeft = value; } }

    public void StartTurn(float turnTime)
    {
        TurnTime = turnTime;
        TurnTimeLeft = turnTime;
        m_turnTooltip.StartTurn();
    }

    public void FinishOwnTurn()
    {
        m_turnTooltip.FinishOwnTurn();
    }

    public void EndTurn()
    {
        m_turnTooltip.EndTurn();
        TurnTimeLeft = 0f;
    }
}
