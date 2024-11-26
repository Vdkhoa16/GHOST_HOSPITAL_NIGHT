using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ShowPromptOnTrigger : MonoBehaviour
{
    public GameObject pressFText; // Tham chiếu đến GameObject hiển thị thông báo

    private void Start()
    {
        pressFText.SetActive(false); // Ẩn thông báo ban đầu
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu người chơi vào vùng Trigger
        if (other.CompareTag("Player"))
        {
            //pressFText.SetActive(true); // Hiển thị thông báo
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                pressFText.SetActive(true);
            }
            else
            {
                pressFText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu người chơi rời khỏi vùng Trigger
        if (other.CompareTag("Player"))
        {
            pressFText.SetActive(false);
        }
    }

}
