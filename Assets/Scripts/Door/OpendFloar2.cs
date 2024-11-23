using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class OpendFloar2 : NetworkBehaviour
{
    public GameObject pickupButton;
    private bool isPlayerInRange = false;

    public OnGame GameOn;
    //public GameObject Fence;
    public ElectoPanel electoPanel;
    //public ParticleSystem fenceEffect;
    void Start()
    {
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            if (electoPanel != null && electoPanel.IsOn())
            {
                pickupButton.SetActive(false); // Ẩn nút nhấn
                // TriggerFenceEffect();
                // Fence.SetActive(false); // Tắt Fence
                //if (GameOn != null)
                //{
                //    GameOn.OnObject();
                //}
               GameOn.OnObject();
            }
            else
            {
                Debug.Log("Không thể tắt Fence vì ElectoPanel chưa mở!"); // Thông báo lỗi
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                pickupButton.SetActive(true);
                isPlayerInRange = true;
            }
            else
            {
                pickupButton.SetActive(false);
                isPlayerInRange = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupButton.SetActive(false);
            isPlayerInRange = false;
        }
    }


    //private void TriggerFenceEffect()
    //{
    //    if (fenceEffect != null)
    //    {
    //        fenceEffect.Play(); 
    //    }

    //}

}
