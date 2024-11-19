using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Testsync1 : NetworkBehaviour
{
    public MeshRenderer meshRenderer;
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
}
