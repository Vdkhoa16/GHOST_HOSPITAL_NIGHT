using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class RaycastCamera : MonoBehaviour
{
    public float rayDistanse = 10f;
    public LayerMask layerMask;
    public LayerMask layerMaskDefault;
    public BoxRaycast[] boxRaycast;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = new Ray(transform.position, transform.forward);
        //RaycastHit hit;


        //if(Physics.Raycast(ray, out hit,rayDistanse, layerMask))
        //{
            
        //    for(int i=0; i< boxRaycast.Length; i++)
        //    {
        //        boxRaycast[i] = hit.collider.GetComponent<BoxRaycast>();
        //        if (boxRaycast != null)
        //        {
        //            boxRaycast[i].OnRayCastMirro();
        //        }
        //    }
        //}
 
        //if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f,layerMaskDefault))
        //{
        //    Debug.Log("Hit");
        //    Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.forward)*hitInfo.distance,Color.red);
        //    //if(Physics.Raycast(ray, out hit,rayDistanse, layerMask))

        //}
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 20f))
        {
            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);


            // Kiểm tra layer của đối tượng
            int layer = hitInfo.collider.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Mirror"))
            {
                Debug.Log("Va chạm với đối tượng trong layer Mirror");

                    for (int i = 0; i < boxRaycast.Length; i++)
                {
                    boxRaycast[i] = hitInfo.collider.GetComponent<BoxRaycast>();
                    if (boxRaycast != null)
                    {
                        boxRaycast[i].OnRayCastMirro();
                    }
                }
            }


            
        }
        else
        {
            //Debug.Log("Không có va chạm");
        }


    }

}
