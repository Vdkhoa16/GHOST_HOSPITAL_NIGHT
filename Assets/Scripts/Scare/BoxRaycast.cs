﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRaycast : MonoBehaviour
{
    [SerializeField] private GameObject scare;
    [SerializeField] private AudioSource clip;
    [SerializeField] private BoxCollider boxCollider;

    private void Start()
    {


        scare.SetActive(false);
  
    }

    private void Update()
    {
        scare.SetActive(false);
      
    }

    public void OnRayCastMirro()
    {       
        scare.SetActive(true);
        clip.Play();
        boxCollider.enabled = false;
    }

}
