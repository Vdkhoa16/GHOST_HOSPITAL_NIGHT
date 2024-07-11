using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Keycode_Set : NetworkBehaviour
{
    [SerializeField] private GameObject door;
    private Tager_Door currentDoor; // Cửa hiện tại mà người chơi đang ở gần
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím E
        if (Input.GetKeyDown(KeyCode.E) && currentDoor != null)
        {
          //  currentDoor.ToggleDoor(); // Gọi hàm ToggleDoor từ script của cửa
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Door")
        {
            door.SetActive(true);
            currentDoor = other.GetComponent<Tager_Door>(); // Lấy script Target_Door từ cửa
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Door"))
        {
            door.SetActive(false);
            currentDoor = null; // Người chơi đã ra khỏi vùng cửa
        }
    }
}
