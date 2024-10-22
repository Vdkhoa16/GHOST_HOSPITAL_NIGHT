using UnityEngine;
using TMPro;
using System.Collections;

public class LetterInteraction : MonoBehaviour
{
    public GameObject letterUI; // UI để hiển thị lá thư
    public TextMeshProUGUI letterText; // Text hiển thị nội dung lá thư ban đầu
    public TextMeshProUGUI interactionText; // Text hiển thị "Nhấn E để đọc"
    public TextMeshProUGUI newLetterContent; // Nội dung mới sẽ hiển thị
    public GameObject firstPanel; // Panel đầu tiên sẽ hiện sau 2 giây
    public GameObject secondPanel; // Panel thứ hai sẽ hiện sau khi firstPanel ẩn đi

    public AudioSource audioSource1; // Âm thanh đầu tiên phát sau 2 giây
    public AudioSource audioSource2; // Âm thanh thứ hai phát sau 2 giây

    private bool isNearLetter = false;
    private bool isReadingLetter = false;
    private bool hasReadLetter = false; // Biến kiểm soát việc đã đọc lá thư
    private bool isLetterCompleted = false; // Biến kiểm soát việc đọc xong thư
    private GameObject currentCharacter = null; // Lưu trữ nhân vật hiện tại đang ở gần

    void Start()
    {
        letterUI.SetActive(false); // Ẩn UI lá thư ban đầu
        interactionText.gameObject.SetActive(false); // Ẩn thông báo ban đầu
        newLetterContent.gameObject.SetActive(false); // Ẩn nội dung mới khi chưa đọc
        firstPanel.SetActive(false); // Ẩn panel đầu tiên khi chưa đọc
        secondPanel.SetActive(false); // Ẩn panel thứ hai khi chưa đọc

        audioSource1.Stop(); // Đảm bảo âm thanh ban đầu bị tắt
        audioSource2.Stop(); // Đảm bảo âm thanh ban đầu bị tắt
    }

    void Update()
    {
        if (isNearLetter && Input.GetKeyDown(KeyCode.E) && !hasReadLetter) // Nếu có nhân vật ở gần, nhấn E và chưa đọc lá thư
        {
            letterUI.SetActive(!letterUI.activeSelf); // Bật/tắt UI lá thư

            if (letterUI.activeSelf)
            {
                interactionText.gameObject.SetActive(false); // Ẩn thông báo khi UI mở
                isReadingLetter = true; // Đánh dấu đang đọc lá thư
                StartCoroutine(ChangeLetterContentAndShowFirstPanel(2)); // Gọi Coroutine để đổi nội dung và hiển thị panel đầu tiên sau 2 giây
            }
            else
            {
                interactionText.gameObject.SetActive(true); // Hiện lại thông báo khi UI đóng
                isReadingLetter = false; // Đánh dấu không đọc lá thư nữa
                StopAllCoroutines(); // Ngừng tất cả Coroutine khi UI đóng
                letterText.gameObject.SetActive(true); // Hiện lại nội dung lá thư ban đầu khi đóng UI
                newLetterContent.gameObject.SetActive(false); // Ẩn nội dung mới khi đóng UI
                firstPanel.SetActive(false); // Ẩn panel đầu tiên khi đóng UI
                secondPanel.SetActive(false); // Ẩn panel thứ hai khi đóng UI
                audioSource1.Stop(); // Tắt âm thanh khi UI đóng
                audioSource2.Stop(); // Tắt âm thanh khi UI đóng
            }
        }
    }

    IEnumerator ChangeLetterContentAndShowFirstPanel(float delay)
    {
        yield return new WaitForSeconds(2); // Thời gian chờ trước khi hiển thị panel đầu tiên
        if (isReadingLetter)
        {
            if (!audioSource1.isPlaying) // Phát âm thanh đầu tiên nếu nó chưa được phát
            {
                audioSource1.Play();
            }

            yield return StartCoroutine(FadeOut(letterText)); // Fade out nội dung ban đầu
            letterText.gameObject.SetActive(false); // Ẩn nội dung ban đầu
            newLetterContent.gameObject.SetActive(true); // Hiện nội dung mới sau khi đổi
            firstPanel.SetActive(true); // Hiện panel đầu tiên sau khi đổi nội dung

            // Gọi Coroutine để ẩn firstPanel sau 2 giây và hiển thị secondPanel
            StartCoroutine(HideFirstPanelAndShowSecondPanel(2));
        }
    }

    IEnumerator HideFirstPanelAndShowSecondPanel(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi thêm 2 giây
        if (isReadingLetter)
        {
            firstPanel.SetActive(false); // Ẩn panel đầu tiên
            if (!audioSource2.isPlaying) // Phát âm thanh thứ hai nếu nó chưa được phát
            {
                audioSource2.Play();
            }
            secondPanel.SetActive(true); // Hiển thị panel thứ hai
            isLetterCompleted = true; // Đánh dấu lá thư đã đọc xong
        }
    }

    IEnumerator FadeOut(TextMeshProUGUI text, float fadeDuration = 1f)
    {
        Color originalColor = text.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - normalizedTime); // Giảm alpha từ 1 đến 0
            yield return null; // Chờ một frame
        }
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // Đảm bảo alpha là 0 ở cuối
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasReadLetter) // Chỉ hiện thông báo nếu chưa đọc lá thư
        {
            if (!letterUI.activeSelf) // Chỉ hiện thông báo khi UI lá thư chưa mở
            {
                interactionText.gameObject.SetActive(true); // Hiện thông báo "Nhấn E để đọc"
            }
            isNearLetter = true;
            currentCharacter = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentCharacter)
        {
            if (!isLetterCompleted) // Nếu chưa đọc xong thư thì không cho rời đi
            {
                return; // Không cho phép thoát khỏi Trigger nếu thư chưa đọc xong
            }

            interactionText.gameObject.SetActive(false); // Ẩn thông báo khi nhân vật đi xa
            isNearLetter = false;
            currentCharacter = null;

            if (letterUI.activeSelf)
            {
                letterUI.SetActive(false); // Đóng UI lá thư khi rời xa
                isReadingLetter = false; // Đánh dấu không còn đọc lá thư
                StopAllCoroutines(); // Ngừng tất cả Coroutine khi UI đóng
                letterText.gameObject.SetActive(true); // Hiện lại nội dung ban đầu khi rời xa
                newLetterContent.gameObject.SetActive(false); // Ẩn nội dung mới khi rời xa
                firstPanel.SetActive(false); // Ẩn panel đầu tiên khi rời xa
                secondPanel.SetActive(false); // Ẩn panel thứ hai khi rời xa

                // Tắt cả hai âm thanh khi người chơi rời xa
                if (audioSource1.isPlaying)
                {
                    audioSource1.Stop();
                }
                if (audioSource2.isPlaying)
                {
                    audioSource2.Stop();
                }
            }
        }
    }
}
