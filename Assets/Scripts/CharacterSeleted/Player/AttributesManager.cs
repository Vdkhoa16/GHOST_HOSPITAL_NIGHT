using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AttributesManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    public int health;
    public float walk_Speed = 2f;
    public float running_Speed = 4f;
    public float sprint_Speed = 6f;

    private vThirdPersonController vThirdPersonController;



    
    void Start()
    {
        vThirdPersonController = GetComponent<vThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        vThirdPersonController.UpdateSpeed(walk_Speed,running_Speed,sprint_Speed);
    }


}
