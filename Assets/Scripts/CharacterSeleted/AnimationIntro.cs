using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationIntro : MonoBehaviour
{
    private Animator amin;
    void Start()
    {
        amin = GetComponent<Animator>();
        amin.SetTrigger("Dance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
