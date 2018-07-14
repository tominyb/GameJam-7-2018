using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTurnManager : NetworkBehaviour
{
    [SerializeField] private TurnTimeLeftBar m_turnTimeLeftBar = null;
    [SerializeField] private float m_turnTime = 5f;
    [SerializeField] private float m_timeBetweenTurns = 1f;
    private IEnumerator m_clientTimeLeftUpdate = null;

    // Server-only. Consider moving to a separate class.
    [SerializeField] private CustomNetworkManager m_networkManager = null;
    private HashSet<int> m_clientConnectionIds = null;
    private HashSet<int> m_finishedClientConnectionIds = new HashSet<int>();

    private void Start()
    {
        if (isServer)
        {
            m_clientConnectionIds = m_networkManager.ClientIds;
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
    private void StartTurn()
    {
        m_finishedClientConnectionIds.Clear();
        RpcStartTurn(m_turnTime);
    }

    [ClientRpc]
    private void RpcStartTurn(float turnTime)
    {
        Debug.Log("Turn started.");
        m_turnTime = turnTime;
        m_turnTimeLeftBar.TurnTime = turnTime;
        m_clientTimeLeftUpdate = HandleTurnTimeUpdates();
        StartCoroutine(m_clientTimeLeftUpdate);
    }

    [Server]
    private bool HaveAllClientsFinishedTheirTurn()
    {
        return !m_clientConnectionIds.Except(m_finishedClientConnectionIds).Any();
    }

    [Server]
    private void EndTurn()
    {
        RpcEndTurn();
    }

    [ClientRpc]
    private void RpcEndTurn()
    {
        Debug.Log("Turn ended.");
        m_turnTimeLeftBar.TurnTimeLeft = 0f;
        if (m_clientTimeLeftUpdate != null)
        {
            StopCoroutine(m_clientTimeLeftUpdate);
            m_clientTimeLeftUpdate = null;
        }
    }

    [Client]
    private IEnumerator HandleTurnTimeUpdates()
    {
        for (float turnTimeLeft = m_turnTime;;)
        {
            turnTimeLeft = Mathf.Max(turnTimeLeft - Time.deltaTime, 0.0f);
            m_turnTimeLeftBar.TurnTimeLeft = turnTimeLeft;
            yield return null;
        }
    }

    [Server]
    public void FinishClientTurn(int clientId)
    {
        Debug.Log(m_clientConnectionIds.Count);
        Debug.Log("Client (" + clientId + ") finished their turn.");
        m_finishedClientConnectionIds.Add(clientId);
    }
}
