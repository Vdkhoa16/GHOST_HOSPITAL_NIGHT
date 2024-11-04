using UnityEngine;
using TMPro;
using System.Collections;

public class Paper : MonoBehaviour
{
    public GameObject letterUI;
    public TextMeshProUGUI letterText;
    public TextMeshProUGUI interactionText;
    public GameObject extraObject; // Đối tượng thêm sẽ xuất hiện
    private bool isNearLetter = false;
    private bool isReadingLetter = false; // Biến kiểm soát việc đang đọc lá thư

    void Start()
    {
        if (letterUI != null)
        {
            letterUI.SetActive(false); // Ẩn UI lá thư ban đầu
        }

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Ẩn thông báo ban đầu
        }

        if (extraObject != null)
        {
            extraObject.SetActive(false); // Ẩn đối tượng thêm ban đầu
        }
    }

    void Update()
    {
        if (isNearLetter && Input.GetKeyDown(KeyCode.E) && !isReadingLetter) // Nếu có nhân vật ở gần và nhấn E
        {
            letterUI.SetActive(true); // Mở UI lá thư
            interactionText.gameObject.SetActive(false); // Ẩn thông báo khi UI mở
            isReadingLetter = true; // Đánh dấu đang đọc lá thư

            StartCoroutine(ShowExtraObjectAfterDelay(5f, 6f)); // Bắt đầu Coroutine hiển thị extraObject
        }
        else if (isReadingLetter && Input.GetKeyDown(KeyCode.E))
        {
            letterUI.SetActive(false); // Đóng UI lá thư
            interactionText.gameObject.SetActive(true); // Hiện lại thông báo khi UI đóng
            isReadingLetter = false; // Đánh dấu không còn đọc lá thư
        }
    }

    private IEnumerator ShowExtraObjectAfterDelay(float delay, float duration)
    {
        yield return new WaitForSeconds(delay); // Chờ 5 giây
        if (extraObject != null)
        {
            extraObject.SetActive(true); // Hiển thị extraObject
            yield return new WaitForSeconds(duration); // Chờ 6 giây
            extraObject.SetActive(false); // Ẩn extraObject
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu là người chơi
        {
            if (!isReadingLetter) // Chỉ hiện thông báo khi chưa đọc lá thư
            {
                interactionText.gameObject.SetActive(true); // Hiện thông báo "Nhấn E để đọc"
            }
            isNearLetter = true; // Đánh dấu đã ở gần lá thư
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionText.gameObject.SetActive(false); // Ẩn thông báo khi nhân vật đi xa
            isNearLetter = false;

            if (isReadingLetter) // Nếu đang đọc lá thư thì đóng nó khi rời xa
            {
                letterUI.SetActive(false); // Đóng UI lá thư
                isReadingLetter = false; // Đánh dấu không còn đọc lá thư
            }
        }
    }
}
