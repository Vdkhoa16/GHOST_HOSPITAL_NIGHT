using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxstop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng va chạm có tag "car"
        if (other.CompareTag("car"))
        {
            // Lấy script từ đối tượng Player
            Stopcar playerScript = other.GetComponent<Stopcar>();
            if (playerScript != null)
            {
                Debug.Log("TRUE CAR");
                // Gọi hàm từ script của Player
                playerScript.StartAni();
            }
        }
    }
}
