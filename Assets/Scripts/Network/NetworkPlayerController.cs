using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    private readonly Dictionary<KeyCode, Vector2Int> m_commandDirections = new Dictionary<KeyCode, Vector2Int>
    {
        { KeyCode.A, Vector2Int.left },
        { KeyCode.D, Vector2Int.right },
        { KeyCode.S, Vector2Int.down },
        { KeyCode.W, Vector2Int.up }
    };

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        foreach (var entry in m_commandDirections)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                NotifyDirection(entry.Value);
            }
        }
    }

    private void NotifyDirection(Vector2Int direction)
    {
        // Vector2Int is not supported as an argument to Remote Actions (ClientRpc, Command).
        // Therefore, directions are sent as Vector2's with proper conversions done in both ends.
        if (isServer)
        {
            Debug.Log("Notifying clients: " + direction);
            RpcNotifyDirectionToClients(direction);
        }
        else
        {
            Debug.Log("Notifying server: " + direction);
            CmdNotifyDirectionToServer(direction);
        }
    }

    [ClientRpc]
    private void RpcNotifyDirectionToClients(Vector2 directionFloat)
    {
        Debug.Log("Server notify received: " + Vector2Int.RoundToInt(directionFloat));
    }

    [Command]
    private void CmdNotifyDirectionToServer(Vector2 directionFloat)
    {
        Debug.Log("Client notify received: " + Vector2Int.RoundToInt(directionFloat));
    }
}
