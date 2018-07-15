using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkTurnManagerServerData : MonoBehaviour
{
    public enum MonsterAction { Attack, Move, Wait }

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
        List<NetworkPlayer> players = m_networkManager.ClientPlayers.Values.ToList();

        foreach (KeyValuePair<Vector2Int, NetworkMonster> entry in monsters)
        {
            Vector2Int currentPosition = entry.Key;
            NetworkMonster monster = entry.Value;
            List<Vector2Int> targetTiles = new List<Vector2Int>();
            MonsterAction action = MonsterAction.Wait;

            for (int i = 0; i < m_directions.Length; ++i)
            {
                Vector2Int targetTile = currentPosition + m_directions[i];
                MonsterAction tempAction = GetMonsterAction(targetTile, newMonsterPositions, players);
   
                if (tempAction == MonsterAction.Move)
                {
                    action = tempAction;
                    targetTiles.Add(targetTile);
                }
                else if (tempAction == MonsterAction.Attack)
                {
                    action = tempAction;
                    targetTiles.Clear();
                    targetTiles.Add(targetTile);
                    break;
                }
            }

            if (action == MonsterAction.Wait)
            {
                continue;
            }

            if (action == MonsterAction.Attack)
            {
                PerformMonsterAttack(monster, targetTiles[0], players);
            }

            else
            {
                Vector2Int targetTile = targetTiles[Random.Range(0, targetTiles.Count)];
                newMonsterPositions.Add(targetTile, monster);
                monster.RpcMove(targetTile);
                newMonsterPositions.Remove(currentPosition);
            }
        }

        Map.I.Monsters = newMonsterPositions;
    }

    private MonsterAction GetMonsterAction(Vector2Int tilePosition, Dictionary<Vector2Int, NetworkMonster> monsters,
        List<NetworkPlayer> players)
    {
        Tile tile = Map.I.GetTile(tilePosition);

        if (tile == null)
        {
            return MonsterAction.Wait;
        }

        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].Position == tilePosition)
            {
                return MonsterAction.Attack;
            }
        }

        if (tile.Type != TileType.Door && !monsters.ContainsKey(tilePosition))
        { 
            return MonsterAction.Move;
        }

        return MonsterAction.Wait;
    }

    private void PerformMonsterAttack(NetworkMonster monster, Vector2Int targetPosition, List<NetworkPlayer> players)
    {
        foreach (NetworkPlayer player in players)
        {
            if (player.Position == targetPosition)
            {
                InflictDamageOnPlayer(Damage.GetDamage(monster.Damage), player);
                return;
            }
        }
    }

    private void InflictDamageOnPlayer(int damage, NetworkPlayer player)
    {
        player.Health.TakeDamage(damage);
        if (player.Health.IsDead())
        {
            player.RpcDie();
        }
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
