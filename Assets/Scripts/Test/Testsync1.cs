using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Testsync1 : NetworkBehaviour
{
    public MeshRenderer meshRenderer;

    public BoxCollider boxCollider;

   // public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MeshServerRpc();
            BoxSeverRpc();
            GOSeverRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void MeshServerRpc(ServerRpcParams rpcParams = default)
    {
        MeshClientRpc();
    }

    [ClientRpc]
    public void MeshClientRpc(ClientRpcParams rpcParams = default)
    {
        OnMesh();
    }

    public void OnMesh()
    {
        meshRenderer.enabled = true;
    }




    public void BoxSeverRpc(ServerRpcParams rpcParams = default)
    {
        BoxClientRpc();
    }
    [ClientRpc]
    public void BoxClientRpc(ClientRpcParams rpcParams = default)
    {
        OnBox();
    }
    public void OnBox()
    {
        boxCollider.enabled = false;
    }

    public void GOSeverRpc(ServerRpcParams rpcParams = default)
    {
        GOClientRpc();
    }
    [ClientRpc]
    public void GOClientRpc(ClientRpcParams rpcParams = default)
    {
      //  GOBox();
    }
    //public void GOBox()
    //{
    //    gameObject.SetActive(true);
    //}
}
