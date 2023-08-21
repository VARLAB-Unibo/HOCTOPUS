using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] public List<GameObject> listClientPrefabs;


    [ServerRpc(RequireOwnership = false)]
    public void JoinServerRpc(ulong clientId, int platform)
    {
        GameObject tempGO = (GameObject)Instantiate(listClientPrefabs[platform]);
        NetworkObject netObj = tempGO.GetComponent<NetworkObject>();

        netObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    public override void OnNetworkSpawn()
    {
        if (Application.platform == RuntimePlatform.Android)
            JoinServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        else
        {
            Debug.Log("Spawna hololens");
            JoinServerRpc(NetworkManager.Singleton.LocalClientId, 1);
        }
    }
}