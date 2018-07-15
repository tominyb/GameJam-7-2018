using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkHealth))]
public class NetworkPlayer : NetworkBehaviour
{
    private Map m_map = Map.I;
    private Vector2Int m_position = Vector2Int.zero;

    private NetworkHealth m_health = null;
    [SyncVar] private int m_attack = 6;

    // Local player only.
    private TurnUI m_turnUI = null;
    private Transform m_canvasTransform = null;
    [SerializeField] private DamageUI m_damageUI = null;
    // Server-only.
    private NetworkTurnManager m_turnManager = null;
    private CustomNetworkManager m_networkManager = null;

    public Vector2Int Position { get { return m_position; } }

    private void Start()
    {
        m_position = m_map.GetClosestTile(transform.position);
        m_health = GetComponent<NetworkHealth>();

        if (isLocalPlayer)
        {
            m_turnUI = FindObjectOfType<TurnUI>();
            m_canvasTransform = FindObjectOfType<Canvas>().transform;
            FindObjectOfType<HealthBar>().Health = m_health;
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

        HandleTileAtTargetPosition(tile, targetPosition);
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

    [Server]
    private void HandleTileAtTargetPosition(Tile tile, Vector2Int targetPosition)
    {
        if (tile.Type == TileType.Door)
        {
            tile.ChangeType(TileType.OpenDoor);
            RpcOpenDoorAtPosition(targetPosition);
            return;
        }

        HandlePossibleItemAtTargetPosition(targetPosition);
        if (!HandlePossibleMonsterAtTargetPosition(targetPosition))
        {
            RpcSetPosition(targetPosition);
        }
    }

    [Server]
    private void HandlePossibleItemAtTargetPosition(Vector2Int targetPosition)
    {
        NetworkItem item = m_map.GetItemAtPosition(targetPosition);
        if (item == null)
        {
            return;
        }
        m_health.RestoreHealth(item.HealthRestoreAmount);
        m_map.RemoveItemAtPosition(targetPosition);
        NetworkServer.Destroy(item.gameObject);
    }

    [Server]
    private bool HandlePossibleMonsterAtTargetPosition(Vector2Int targetPosition)
    {
        NetworkMonster monster = m_map.GetMonsterAtPosition(targetPosition);
        if (monster == null)
        {
            return false;
        }
        NetworkHealth monsterHealth = monster.Health;
        int damage = Damage.GetDamage(m_attack);
        monsterHealth.TakeDamage(damage);
        RpcDisplayDamage(damage, monster.transform.position);
        if (monsterHealth.IsDead())
        {
            m_map.RemoveMonsterAtPosition(targetPosition);
            NetworkServer.Destroy(monster.gameObject);
        }
        return true;
    }

    [ClientRpc]
    private void RpcDisplayDamage(int damage, Vector3 monsterPosition)
    {
        Instantiate(m_damageUI, m_canvasTransform).InitEffect(damage, monsterPosition);
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
