using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkPlayer))]
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

    private NetworkPlayer m_player = null;

    private void Start()
    {
        m_player = GetComponent<NetworkPlayer>();
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
                m_player.CmdTryMoveBy(entry.Value);
                return; // Only one action per turn is allowed, so further checks would be unnecessary.
            }
        }
    }
}
