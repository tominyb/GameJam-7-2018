using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkHealth))]
public class NetworkPlayer : NetworkBehaviour
{
    private Map m_map = Map.I;
    private NetworkHealth m_health = null;
    private Vector2Int m_position = Vector2Int.zero;

    // Local player only.
    private TurnUI m_turnUI = null;
    // Server-only.
    private NetworkTurnManager m_turnManager = null;
    private CustomNetworkManager m_networkManager = null;

    public Vector2Int Position { get { return m_position; } }

    private void Start()
    {
        m_health = GetComponent<NetworkHealth>();
        m_position = m_map.GetClosestTile(transform.position);

        if (isLocalPlayer)
        {
            m_turnUI = FindObjectOfType<TurnUI>();
            PlayerCamera.LocalPlayer = gameObject;
        }
    }

    public override void OnStartServer()
    {
        m_networkManager = FindObjectOfType<CustomNetworkManager>();
        m_networkManager.ClientPlayers.Add(connectionToClient.connectionId, this);
        m_turnManager = FindObjectOfType<NetworkTurnManager>();
    }

    [Command]
    public void CmdTryMoveBy(Vector2 delta)
    {
        int connectionId = connectionToClient.connectionId;
        if (!m_turnManager.IsActionExpectedFromClient(connectionId))
        {
            return;
        }

        Vector2Int targetPosition = m_position + Vector2Int.RoundToInt(delta);

        if (IsPositionOccupiedByAnotherPlayer(targetPosition))
        {
            return;
        }

        Tile tile = m_map.GetTile(targetPosition);
        if (tile == null)
        {
            return;
        }

        if (tile.Type != TileType.Door)
        {
            RpcSetPosition(targetPosition);
        }
        else
        {
            tile.ChangeType(TileType.OpenDoor);
            RpcOpenDoorAtPosition(targetPosition);
        }

        m_turnManager.FinishClientTurn(connectionId);
    }

    [Server]
    private bool IsPositionOccupiedByAnotherPlayer(Vector2Int position)
    {
        var players = m_networkManager.ClientPlayers;
        foreach (var entry in players)
        {
            if (entry.Value.Position == position)
            {
                return true;
            }
        }
        return false;
    }

    [ClientRpc]
    private void RpcSetPosition(Vector2 position)
    {
        m_position = Vector2Int.RoundToInt(position);
        transform.position = m_map.GetTile(Vector2Int.RoundToInt(position)).Sprite.transform.position;
        FinishTurn();
    }

    [ClientRpc]
    private void RpcOpenDoorAtPosition(Vector2 position)
    {
        m_map.GetTile(Vector2Int.RoundToInt(position)).ChangeType(TileType.OpenDoor);
        FinishTurn();
    }

    [Client]
    private void FinishTurn()
    {
        m_turnUI?.FinishOwnTurn();
    }
}
