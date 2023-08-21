using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SendInfoClient : NetworkBehaviour
{
    [ClientRpc]
    public void RemoveNotificationClientRpc(ClientRpcParams clientRpcParams = default)
    {
        GameObject student = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        if (student != null)
            student.GetComponent<ClientHandler>().CallRaiseArm(false);
    }

    [ClientRpc]
    public void EnableDisableClientRpc(bool enable, ClientRpcParams clientRpcParams = default)
    {
        GameObject student = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        if (student != null)
            student.GetComponent<ClientHandler>().EnableDisableNotificationButton(enable);
    }

    [ClientRpc]
    public void ClientDisconnectionClientRpc()
    {
        GameObject student = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        if (student != null)
            student.GetComponent<ClientHandler>().Exit();
    }

    [ClientRpc]
    public void GrantPermissionClientRpc(bool enable, ClientRpcParams clientRpcParams = default)
    {
        GameObject student = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        if (student != null)
            student.GetComponent<ClientHandler>().Permission(enable);

    }

}