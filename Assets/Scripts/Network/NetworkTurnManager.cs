using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTurnManager : NetworkBehaviour
{
    [SerializeField] private TurnTimeLeftBar m_turnTimeLeftBar = null;
    [SerializeField] private float m_turnTime = 5f;
    [SerializeField] private float m_timeBetweenTurns = 1f;
    private IEnumerator m_clientTimeLeftUpdate = null;

    // Server-only.
    private NetworkTurnManagerServerData m_serverData = null;

    // TODO: Re-start whenever a connection is initiated. Stalls currently if player exits game and re-joins.
    private void Start()
    {
        if (isServer)
        {
            m_serverData = gameObject.AddComponent<NetworkTurnManagerServerData>();
            StartCoroutine(HandleTurns());
        }
    }

    [Server]
    private IEnumerator HandleTurns()
    {
        for (;;)
        {
            yield return new WaitForSeconds(m_timeBetweenTurns);
            StartTurn();
            for (float time = 0f; (time < m_turnTime) && !HaveAllClientsFinishedTheirTurn(); time += Time.deltaTime)
            {
                yield return null;
            }
            EndTurn();
        }
    }

    [Server]
    private bool HaveAllClientsFinishedTheirTurn()
    {
        return m_serverData.HaveAllClientsFinishedTheirTurn();
    }

    [Server]
    private void StartTurn()
    {
        RpcStartTurn(m_turnTime);
        m_serverData.StartTurn();
    }

    [ClientRpc]
    private void RpcStartTurn(float turnTime)
    {
        m_turnTime = turnTime;
        m_turnTimeLeftBar.TurnTime = turnTime;
        m_clientTimeLeftUpdate = HandleTurnTimeLeftUpdates();
        StartCoroutine(m_clientTimeLeftUpdate);
    }

    [Server]
    private void EndTurn()
    {
        m_serverData.EndTurn();
        RpcEndTurn();
    }

    [ClientRpc]
    private void RpcEndTurn()
    {
        m_turnTimeLeftBar.TurnTimeLeft = 0f;
        if (m_clientTimeLeftUpdate == null)
        {
            return;
        }
        StopCoroutine(m_clientTimeLeftUpdate);
        m_clientTimeLeftUpdate = null;
    }

    [Client]
    private IEnumerator HandleTurnTimeLeftUpdates()
    {
        for (float turnTimeLeft = m_turnTime;;)
        {
            turnTimeLeft = Mathf.Max(turnTimeLeft - Time.deltaTime, 0.0f);
            m_turnTimeLeftBar.TurnTimeLeft = turnTimeLeft;
            yield return null;
        }
    }

    [Server]
    public bool IsActionExpectedFromClient(int clientConnectionId)
    {
        return m_serverData.IsActionExpectedFromClient(clientConnectionId);
    }

    [Server]
    public void FinishClientTurn(int clientConnectionId)
    {
        m_serverData.FinishClientTurn(clientConnectionId);
    }
}
