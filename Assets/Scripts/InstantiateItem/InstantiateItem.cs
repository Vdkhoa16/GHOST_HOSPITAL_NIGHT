using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
public class InstantiateItem : NetworkBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemTransform;


    void Update()
    {
        // Handle input in the Update method
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsClient)
            {
                InstantiateItemServerRpc();
            }
        }
    }

    public void Item()
    {
        Vector3 swap = itemTransform.position;
        Quaternion spawnRotation = Quaternion.identity; 
        GameObject itemInstance = Instantiate(itemPrefab, swap, spawnRotation);
    }

    [ServerRpc(RequireOwnership = false)]
    public void InstantiateItemServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            var playerNetworkObject = client.PlayerObject;
            if (playerNetworkObject != null)
            {
                Item();
                InstantiateItemClientRpc(playerNetworkObject.NetworkObjectId);
            }
        }
    }

    [ClientRpc]
    void InstantiateItemClientRpc(ulong playerNetworkObjectId, ClientRpcParams rpcParams = default)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerNetworkObjectId, out var playerNetworkObject))
        {
            Item();
        }
    }
}
