using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private BoxCollider boxCollider;
    private AudioSource effect;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        effect = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Chạm");
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                //Debug.Log("Play");
                effect.Play();
                boxCollider.enabled = false;
            }
        }
    }
}
