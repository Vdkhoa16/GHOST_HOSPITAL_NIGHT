using UnityEngine;
using TMPro;

public class Paper : MonoBehaviour
{
    public GameObject letterUI; // UI để hiển thị lá thư
    public TextMeshProUGUI letterText; // Text hiển thị nội dung lá thư
    public TextMeshProUGUI interactionText; // Text hiển thị "Nhấn E để đọc"

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
    }

    void Update()
    {
        if (isNearLetter && Input.GetKeyDown(KeyCode.E) && !isReadingLetter) // Nếu có nhân vật ở gần và nhấn E
        {
            letterUI.SetActive(true); // Mở UI lá thư
            interactionText.gameObject.SetActive(false); // Ẩn thông báo khi UI mở
            isReadingLetter = true; // Đánh dấu đang đọc lá thư
        }
        else if (isReadingLetter && Input.GetKeyDown(KeyCode.E))
        {
            letterUI.SetActive(false); // Đóng UI lá thư
            interactionText.gameObject.SetActive(true); // Hiện lại thông báo khi UI đóng
            isReadingLetter = false; // Đánh dấu không còn đọc lá thư
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
