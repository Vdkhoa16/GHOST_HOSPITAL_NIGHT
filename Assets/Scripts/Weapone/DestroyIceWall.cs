using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIceWall : MonoBehaviour
{
    public float timeDestroy = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
