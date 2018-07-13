using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTurnManager : NetworkBehaviour
{
    [SerializeField] private TurnTimeLeftBar m_turnTimeLeftBar;
    [SerializeField] private float m_turnTime = 5f;
    [SerializeField] private float m_timeBetweenTurns = 1f;
    private float m_turnTimeLeft;
    private IEnumerator m_clientTimeLeftUpdate;

    private void Start()
    {
        if (isServer)
        {
            StartCoroutine(HandleTurns());
        }
    }

    [Server]
    private IEnumerator HandleTurns()
    {
        for (;;)
        {
            yield return new WaitForSeconds(m_timeBetweenTurns);
            RpcStartTurn(m_turnTime);
            yield return new WaitForSeconds(m_turnTime);
            RpcEndTurn();
        }
    }

    [ClientRpc]
    private void RpcStartTurn(float turnTime)
    {
        Debug.Log("Turn started.");
        m_turnTime = turnTime;
        m_turnTimeLeftBar.TurnTime = m_turnTime;
        m_turnTimeLeft = turnTime;
        m_clientTimeLeftUpdate = HandleTurnTimeUpdates();
        StartCoroutine(m_clientTimeLeftUpdate);
    }

    [ClientRpc]
    private void RpcEndTurn()
    {
        Debug.Log("Turn ended.");
        m_turnTimeLeft = 0f;
        StopCoroutine(m_clientTimeLeftUpdate);
        m_clientTimeLeftUpdate = null;
    }

    [Client]
    private IEnumerator HandleTurnTimeUpdates()
    {
        for (;;)
        {
            m_turnTimeLeft = Mathf.Max(m_turnTimeLeft - Time.deltaTime, 0.0f);
            m_turnTimeLeftBar.TurnTimeLeft = m_turnTimeLeft;
            yield return null;
        }
    }
}
