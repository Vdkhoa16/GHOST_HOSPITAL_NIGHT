using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Testsync : NetworkBehaviour
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
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
    }
}
