using UnityEngine;
using System.Collections;

public class ShowObjectOnPress : MonoBehaviour
{
    public GameObject mainObject; // Đối tượng chính sẽ hiển thị
    public GameObject paperObject; // Đối tượng paper sẽ hiển thị

    private bool isPlayerNearby = false;

    private void Start()
    {
        // Đảm bảo các đối tượng đều ẩn ban đầu
        mainObject.SetActive(false);
        paperObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu người chơi tiến vào
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu người chơi rời đi
        {
            isPlayerNearby = false;
        }
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi ở gần và nhấn phím F
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ShowMainObjectForSeconds(2f)); // Hiển thị trong 2 giây
            paperObject.SetActive(true); // Giữ paperObject hiển thị
        }
    }

    private IEnumerator ShowMainObjectForSeconds(float duration)
    {
        mainObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        mainObject.SetActive(false);
    }
}
