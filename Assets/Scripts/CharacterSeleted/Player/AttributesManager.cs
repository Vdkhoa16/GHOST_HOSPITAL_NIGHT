using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AttributesManager : NetworkBehaviour
{

    public bool isTesting = false; // Tạo biến bool để kiểm soát từ Inspector
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
        health = 5;
        vThirdPersonController = GetComponent<vThirdPersonController>();
    }

    public void Atacking()
    {
        health -= 1;
        sprint_Speed -= 1;
        walk_Speed -= 1;
        running_Speed -= 1;
       // Update();
    }

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu vật thể có tag "Collectible"
        if (other.CompareTag("test"))
        {
            Atacking();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTesting)
        {
           Atacking();
            isTesting = false; // Đặt về false để chỉ gọi một lần khi bật từ Inspector
        }

        vThirdPersonController.UpdateSpeed(walk_Speed, running_Speed, sprint_Speed);
    }


}