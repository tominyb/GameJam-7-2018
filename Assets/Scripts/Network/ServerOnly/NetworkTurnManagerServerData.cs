using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkTurnManagerServerData : MonoBehaviour
{
    [SerializeField] private CustomNetworkManager m_networkManager = null;
    private HashSet<int> m_clientConnectionIds = null;
    private readonly HashSet<int> m_finishedClientConnectionIds = new HashSet<int>();
    private bool m_turnActive = false;

    private void Start()
    {
        m_networkManager = FindObjectOfType<CustomNetworkManager>();
        m_clientConnectionIds = m_networkManager.ClientConnectionIds;
    }

    public void StartTurn()
    {
        m_finishedClientConnectionIds.Clear();
        m_turnActive = true;
    }

    public void EndTurn()
    {
        m_turnActive = false;
    }

    public void FinishClientTurn(int clientConnectionId)
    {
        m_finishedClientConnectionIds.Add(clientConnectionId);
    }

    public bool IsActionExpectedFromClient(int clientConnectionId)
    {
        return m_turnActive && !m_finishedClientConnectionIds.Contains(clientConnectionId);
    }

    public bool HaveAllClientsFinishedTheirTurn()
    {
        return !m_clientConnectionIds.Except(m_finishedClientConnectionIds).Any();
    }
}
