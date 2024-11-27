using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyAttacking : NetworkBehaviour
{
     public GameObject weapon;
     public GameObject Audio_run;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableWeaponCollider(int isEnable)
    {
        var col = weapon.GetComponentInChildren<BoxCollider>();
        if (col != null )
        {
            if (isEnable == 1)
            {
                col.enabled = true;
            }
            else
            {
                col.enabled = false;
            }
        }
    }

    //on
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
        Audio_run.SetActive(true);
    }
    public void OnObject()
    {
        GOServerRpc();
    }

    //off
    public void outBox()
    {
        Audio_run.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OutServerRpc(ServerRpcParams rpcParams = default)
    {
        OutClientRpc();
    }
    [ClientRpc]
    public void OutClientRpc(ClientRpcParams rpcParams = default)
    {
        outBox();
    }
    public void OutObject()
    {
        OutServerRpc();
    }
}
