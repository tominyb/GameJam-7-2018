using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    private void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            Notify();
        }
    }

    private void Notify()
    {
        if (isServer)
        {
            Debug.Log("Notifying clients...");
            RpcNotifyClients();
        }
        else
        {
            Debug.Log("Notifying server...");
            CmdNotifyServer();
        }
    }

    [ClientRpc]
    private void RpcNotifyClients()
    {
        Debug.Log("Server notify received");
    }

    [Command]
    private void CmdNotifyServer()
    {
        Debug.Log("Client notify received");
    }
}
