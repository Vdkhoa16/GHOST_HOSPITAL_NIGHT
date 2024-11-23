using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OnGame : NetworkBehaviour
{
    public GameObject fences;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
    }

    [ServerRpc(RequireOwnership = false)]
    public void GOServerRpc(ServerRpcParams rpcParams = default)
    {
        GOClientRpc();
    }
    [ClientRpc]
    public void GOClientRpc(ClientRpcParams rpcParams = default)
    {
        GOBox();
    }
    public void GOBox()
    {
        fences.SetActive(false);
    }

    public void OnObject()
    {
        GOServerRpc();
    }


    //public void BoxSeverRpc(ServerRpcParams rpcParams = default)
    //{
    //    BoxClientRpc();
    //}
    //[ClientRpc]
    //public void BoxClientRpc(ClientRpcParams rpcParams = default)
    //{
    //    OnBox();
    //}
    //public void OnBox()
    //{
    //    boxCollider.enabled = false;
    //}


}
