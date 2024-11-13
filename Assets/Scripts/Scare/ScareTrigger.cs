using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScareTrigger : NetworkBehaviour
{
    public GameObject scareObject;
    public Transform moveTarget;
    public float moveSpeed = 5f;          
   // public GameObject scareEffect;
    public AudioSource scareAudio;
    private bool isMoving = false;

    private void Start()
    {
        scareObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            scareObject.SetActive(true);  // Hiện đối tượng hù dọa
            isMoving = true;
            //scareAudio.Play();
           
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                scareAudio.Play();
            }

        }
    }

    private void Update()
    {
        if (isMoving && moveTarget != null)
        {
            // Di chuyển đối tượng hù dọa về hướng của `moveTarget`
            Vector3 direction = (moveTarget.position - scareObject.transform.position).normalized;
            scareObject.transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // Kiểm tra va chạm với tường 
            if (Physics.Raycast(scareObject.transform.position, direction, out RaycastHit hit, 0.5f))
            {
                if (hit.collider.CompareTag("Wall")) // Giả sử tường có tag là "Wall"
                {
                    scareObject.SetActive(false);
                    isMoving = false;               // Ngừng di chuyển
                                                  // Instantiate(scareEffect, hit.point, Quaternion.identity);  // Tạo hiệu ứng va chạm
                    Destroy(gameObject);
                }
            }
        }

    }

}
