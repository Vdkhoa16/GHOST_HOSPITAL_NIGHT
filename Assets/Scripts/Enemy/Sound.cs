using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource runsound;
   // bool isSoundPlaying = false;
   // public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
       // runsound = gameObject.AddComponent<AudioSource>();
       //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Runsound()
    {
            runsound.Play();
    }
    public void StopSound()
    {
       // if (runsound.isPlaying)
        //{
            runsound.Stop();
       // }
    }
}
