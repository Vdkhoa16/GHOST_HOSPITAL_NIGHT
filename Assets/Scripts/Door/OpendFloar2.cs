using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OpendFloar2 : MonoBehaviour
{
    public GameObject pickupButton;
    private bool isPlayerInRange = false;
    public GameObject Fence;
    // Start is called before the first frame update
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
            pickupButton.SetActive(false);
            Fence.SetActive(false);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           if(other.GetComponent<NetworkObject>().IsOwner)
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

   


}
