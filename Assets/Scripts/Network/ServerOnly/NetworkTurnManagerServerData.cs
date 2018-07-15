using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkTurnManagerServerData : MonoBehaviour
{
    [SerializeField] private CustomNetworkManager m_networkManager = null;
    private HashSet<int> m_clientConnectionIds = null;
    private readonly HashSet<int> m_finishedClientConnectionIds = new HashSet<int>();
    private bool m_turnActive = false;

    private readonly Vector2Int[] m_directions = new Vector2Int[]
        { 
            Vector2Int.left + Vector2Int.down,
            Vector2Int.down,
            Vector2Int.right + Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.left + Vector2Int.up,
            Vector2Int.up,
            Vector2Int.right + Vector2Int.up
        };

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
        MoveMonsters();
        m_turnActive = false;
    }

    private void MoveMonsters()
    {
        Dictionary<Vector2Int, NetworkMonster> monsters = Map.I.Monsters;
        Dictionary<Vector2Int, NetworkMonster> newMonsterPositions = new Dictionary<Vector2Int, NetworkMonster>(monsters);
        List<NetworkPlayer> players                     = m_networkManager.ClientPlayers.Values.ToList();
        foreach (KeyValuePair<Vector2Int, NetworkMonster> entry in monsters)
        {
            Vector2Int randomDirection = m_directions[Random.Range(0, m_directions.Length)];
            Vector2Int targetTile = entry.Key + randomDirection;
            if (IsTilePassable(targetTile, newMonsterPositions))
            {
                newMonsterPositions.Add(targetTile, entry.Value);
                entry.Value.RpcMove(targetTile);
                newMonsterPositions.Remove(entry.Key);
            }
        }
        Map.I.Monsters = newMonsterPositions;
    }

    private bool IsTilePassable(Vector2Int tilePosition, Dictionary<Vector2Int, NetworkMonster> monsters)
    {
        Tile tile = Map.I.GetTile(tilePosition);
        return tile != null && tile.Type != TileType.Door && !monsters.ContainsKey(tilePosition);
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
