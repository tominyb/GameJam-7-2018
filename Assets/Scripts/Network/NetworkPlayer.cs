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

    private void Start()
    {
        m_health = GetComponent<NetworkHealth>();
        m_position = m_map.GetClosestTile(transform.position);

        if (isLocalPlayer)
        {
            m_turnUI = FindObjectOfType<TurnUI>();
        }

        if (isServer)
        {
            m_turnManager = FindObjectOfType<NetworkTurnManager>();
        }
    }

    [Command]
    public void CmdTryMoveBy(Vector2 delta)
    {
        if (!m_turnManager.IsActionExpectedFromClient(connectionToClient.connectionId))
        {
            return;
        }

        Vector2Int newPosition = m_position + Vector2Int.RoundToInt(delta);

        Tile targetTile = m_map.GetTile(newPosition);
        if (targetTile == null)
        {
            return;
        }

        m_turnManager.FinishClientTurn(connectionToClient.connectionId);
        RpcSetPosition(newPosition);
    }

    [ClientRpc]
    private void RpcSetPosition(Vector2 position)
    {
        m_position = Vector2Int.RoundToInt(position);
        transform.position = m_map.GetTile(Vector2Int.RoundToInt(position)).Sprite.transform.position;
        m_turnUI?.FinishOwnTurn();
    }
}
