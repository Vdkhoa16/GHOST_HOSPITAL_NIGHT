using UnityEngine;
using System.Collections;

public class ShowObjectOnceOnCollision : MonoBehaviour
{
    public GameObject objectToShow; // Đối tượng sẽ được hiển thị trong 10 giây
    private bool hasBeenActivated = false; // Kiểm tra đã kích hoạt hay chưa

    private void Start()
    {
        if (objectToShow != null)
        {
            objectToShow.SetActive(false); // Ẩn đối tượng từ đầu
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenActivated) // Kiểm tra nếu là người chơi và chưa kích hoạt trước đó
        {
            hasBeenActivated = true; // Đánh dấu đã kích hoạt
            if (objectToShow != null)
            {
                StartCoroutine(ShowObjectFor10Seconds()); // Gọi coroutine để hiển thị đối tượng
            }
        }
    }

    private IEnumerator ShowObjectFor10Seconds()
    {
        objectToShow.SetActive(true); // Hiển thị đối tượng
        yield return new WaitForSeconds(10); // Đợi 10 giây
        objectToShow.SetActive(false); // Ẩn đối tượng sau 10 giây
    }
}
