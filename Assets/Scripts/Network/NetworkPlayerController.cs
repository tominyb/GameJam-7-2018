using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class NetworkPlayerController : NetworkBehaviour
{
    private readonly Dictionary<KeyCode, Vector2Int> m_moveDeltas = new Dictionary<KeyCode, Vector2Int>
    {
        { KeyCode.Keypad1, Vector2Int.left + Vector2Int.down },
        { KeyCode.Keypad2, Vector2Int.down },
        { KeyCode.Keypad3, Vector2Int.right + Vector2Int.down },
        { KeyCode.Keypad4, Vector2Int.left },
        { KeyCode.Keypad6, Vector2Int.right },
        { KeyCode.Keypad7, Vector2Int.left + Vector2Int.up },
        { KeyCode.Keypad8, Vector2Int.up },
        { KeyCode.Keypad9, Vector2Int.right + Vector2Int.up }
    };

    private Player m_player = null;

    // Server-only.
    private NetworkTurnManager m_turnManager = null;

    private void Start()
    {
        if (isServer)
        {
            m_turnManager = FindObjectOfType<NetworkTurnManager>();
        }

        m_player = GetComponent<Player>();
        if (isLocalPlayer)
        {
            PlayerCamera.LocalPlayer = gameObject;
        }
    }

    private void Update()
    {
        if (!isLocalPlayer || !isClient)
        {
            return;
        }

        foreach (var entry in m_moveDeltas)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                CmdTryMoveBy(entry.Value);
                return; // Only one action per turn is allowed, so further checks would only cause unnecessary traffic.
            }
        }
    }

    // Vector2Int is not supported as an argument to Remote Actions (ClientRpc, Command).
    // Therefore, directions are sent as Vector2's with proper conversions done in both ends.

    [Command]
    private void CmdTryMoveBy(Vector2 delta)
    {
        int clientConnectionId = connectionToClient.connectionId;
        if (!m_turnManager.IsActionExpectedFromClient(clientConnectionId))
        {
            return;
        }
        RpcMoveBy(delta);
        m_turnManager.FinishClientTurn(connectionToClient.connectionId);
    }

    [ClientRpc]
    private void RpcMoveBy(Vector2 delta)
    {
        m_player.Move(Vector2Int.RoundToInt(delta));
    }
}
