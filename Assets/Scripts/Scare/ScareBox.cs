using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareBox : MonoBehaviour
{
    public GameObject objScare;
    private BoxCollider boxCollider;
    private AudioSource audioSource;
    void Start()
    {
        objScare.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objScare.SetActive(true);
            audioSource.Play();


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boxCollider.enabled = false;
            Destroy(objScare);
        }
    }
}
