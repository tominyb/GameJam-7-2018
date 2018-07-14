using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTooltip : MonoBehaviour
{
    [SerializeField] private Text m_tooltipText;
    private string m_waitingForTurnText = null;
    [SerializeField] private string m_waitingForActionText = "Input your action.";
    [SerializeField] private string m_waitingForOthersText = "Wait as other players input their action.";

    private void Start()
    {
        m_waitingForTurnText = m_tooltipText.text;
    }

    public void StartTurn()
    {
        SetTooltipText(m_waitingForActionText);
    }

    public void FinishOwnTurn()
    {
        SetTooltipText(m_waitingForOthersText);
    }

    public void EndTurn()
    {
        SetTooltipText(m_waitingForTurnText);
    }

    private void SetTooltipText(string text)
    {
        m_tooltipText.text = text;
    }
}
