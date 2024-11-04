using UnityEngine;

public class GhostAppear : MonoBehaviour
{
    public GameObject ghost; // Tham chiếu tới đối tượng con ma

    private void Start()
    {
        ghost.SetActive(false); // Ẩn con ma lúc ban đầu
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu người chơi tiến vào
        {
            ghost.SetActive(true); // Hiện con ma
        }
    }

}
