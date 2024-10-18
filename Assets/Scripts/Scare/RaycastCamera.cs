using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RaycastCamera : MonoBehaviour
{
    public float rayDistanse = 10f;
    public LayerMask layerMask;
    public BoxRaycast[] boxRaycast;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,rayDistanse, layerMask))
        {
            
            for(int i=0; i< boxRaycast.Length; i++)
            {
                boxRaycast[i] = hit.collider.GetComponent<BoxRaycast>();
                if (boxRaycast != null)
                {
                    boxRaycast[i].OnRayCastMirro();
                }
            }




            //BoxRaycast boxRaycast = hit.collider.GetComponent<BoxRaycast>();
            //if (boxRaycast != null)
            //{
            //    boxRaycast.OnRayCastMirro();
            //}

        }

    }

}
