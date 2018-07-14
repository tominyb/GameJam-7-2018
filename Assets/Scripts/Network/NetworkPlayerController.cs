using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    private readonly Dictionary<KeyCode, Vector2Int> m_commandDirections = new Dictionary<KeyCode, Vector2Int>
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
    }

    private void Update()
    {
        if (!isLocalPlayer || !isClient)
        {
            return;
        }

        foreach (var entry in m_commandDirections)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                NotifyDirection(entry.Value);
                return;
            }
        }
    }

    [Client]
    private void NotifyDirection(Vector2Int direction)
    {
        CmdNotifyDirectionToServer(direction);
    }

    // Vector2Int is not supported as an argument to Remote Actions (ClientRpc, Command).
    // Therefore, directions are sent as Vector2's with proper conversions done in both ends.

    [ClientRpc]
    private void RpcNotifyDirectionToClients(Vector2 directionFloat)
    {
        Debug.Log("Server notify received: " + Vector2Int.RoundToInt(directionFloat));
        Move(Vector2Int.RoundToInt(directionFloat));
    }

    [Command]
    private void CmdNotifyDirectionToServer(Vector2 directionFloat)
    {
        Debug.Log("Client notify received: " + Vector2Int.RoundToInt(directionFloat));
        int clientConnectionId = connectionToClient.connectionId;
        if (m_turnManager.IsActionExpectedFromClient(clientConnectionId))
        {
            RpcNotifyDirectionToClients(directionFloat);
            m_turnManager.FinishClientTurn(connectionToClient.connectionId);
        }
        else
        {
            Debug.Log("Notify dismissed. No action is being expected from client.");
        }
    }

    private void Move(Vector2Int direction)
    {
        Debug.Log("Moving " + name + " (" + gameObject.GetInstanceID() + ") by: " + direction);
        m_player.Move(direction);
    }
}
