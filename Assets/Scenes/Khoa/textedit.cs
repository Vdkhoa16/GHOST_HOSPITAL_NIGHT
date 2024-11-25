using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEdit : MonoBehaviour
{
    public float scrollSpeed = 50f; // Tốc độ di chuyển khi không nhấn chuột
    public string[] textLines; // Mảng chứa các dòng chữ
    private RectTransform rectTransform;
    private float startPositionY;
    private float endPositionY;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI textMeshPro; // TextMeshProUGUI để hiển thị chữ
    private bool isMouseDown = false; // Biến theo dõi trạng thái nhấn chuột
    private bool fadeStarted = false; // Biến theo dõi khi nào bắt đầu hiệu ứng mờ dần
    private float fadeStartPositionY; // Biến để xác định vị trí bắt đầu hiệu ứng mờ
    private float initialMousePositionY; // Vị trí chuột ban đầu khi nhấn chuột

    // Các đối tượng Image cần thay đổi
    public List<Image> images = new List<Image>(); // List chứa các ảnh
    private int currentImageIndex = 0; // Chỉ số của ảnh hiện tại
    private float imageFadeTime = 5f; // Thời gian cho mỗi ảnh mờ đi và xuất hiện
    private float imageTimer = 0f; // Bộ đếm thời gian cho ảnh

    void Start()
    {
        // Khởi tạo các dòng chữ
        textLines = new string[] {
            "PROJECT GHOST HOSPITAL", "Trưởng nhóm: Nguyễn quốc kiệt", "Thành viên phối hợp thực hiện", "Võ Đăng Khoa", "Đặng Vũ Phong",
            "Huỳnh Bảo Long ", "Tuấn Bình", "Kịch bản: ", "Nguyễn Quốc Kiệt - Võ Đăng Khoa", "Thiết kế map: Võ Đăng Khoa",
            "Mini game: Tất cả các thành viên", "Nhiệm vụ và chức năng đã làm", "Nguyễn Quốc Kiệt: ", "Netcode: Xây dựng hệ thống đồng bộ ", "Item: xây dựng hệ hống Item",
            "Võ Đăng Khoa: ", "hệ thống cửa ", "AI ENEMY: quái vật và AI Navmesh", "Âm thanh: vùng âm thanh", "Dòng chữ 20",
            "Dòng chữ 21", "Dòng chữ 22", "Dòng chữ 23", "Dòng chữ 24", "Dòng chữ 25",
            "Dòng chữ 26", "Dòng chữ 27", "Dòng chữ 28", "Dòng chữ 29", "Dòng chữ 30",
            "Dòng chữ 31", "Dòng chữ 32", "Dòng chữ 33", "Dòng chữ 34", "Dòng chữ 35",
            "Dòng chữ 36", "Dòng chữ 37", "Dòng chữ 38", "Dòng chữ 39", "Dòng chữ 40",
            "Dòng chữ 41", "Dòng chữ 42", "Dòng chữ 43", "Dòng chữ 44", "Dòng chữ 45",
            "Dòng chữ 46", "Dòng chữ 47", "Dòng chữ 48", "Dòng chữ 49", "Dòng chữ 50"
        };

        // Lấy TextMeshProUGUI và RectTransform
        textMeshPro = GetComponent<TextMeshProUGUI>();
        rectTransform = textMeshPro.GetComponent<RectTransform>();

        // Lấy thành phần CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup không được gắn vào Text! Vui lòng thêm CanvasGroup.");
        }

        // Lưu vị trí ban đầu của chữ
        startPositionY = rectTransform.anchoredPosition.y;

        // Tính toán vị trí kết thúc (trên màn hình)
        Canvas canvas = GetComponentInParent<Canvas>();
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        endPositionY = canvasHeight + rectTransform.rect.height; // Vượt qua phần trên của màn hình

        // Xác định vị trí bắt đầu mờ (ví dụ: khi dòng chữ 50 xuất hiện)
        fadeStartPositionY = endPositionY - 200f; // Ví dụ, khi dòng cuối gần đến vị trí này sẽ bắt đầu mờ

        // Thiết lập các dòng text từ mảng
        textMeshPro.text = string.Join("\n\n", textLines); // Gộp các dòng lại thành một chuỗi, cách nhau bởi dấu xuống dòng

        // Khởi tạo alpha cho tất cả các ảnh là 0 (ẩn)
        foreach (Image img in images)
        {
            img.canvasRenderer.SetAlpha(0f);
        }
    }

    void Update()
    {
        // Kiểm tra trạng thái chuột
        if (Input.GetMouseButtonDown(0)) // Khi nhấn chuột
        {
            isMouseDown = true;
            fadeStarted = false; // Ngừng mờ khi nhấn chuột
            initialMousePositionY = Input.mousePosition.y; // Lưu vị trí chuột khi bắt đầu nhấn
        }

        if (Input.GetMouseButtonUp(0)) // Khi thả chuột
        {
            isMouseDown = false;
            fadeStarted = true; // Bắt đầu mờ khi thả chuột
        }

        // Khi nhấn chuột, di chuyển text dựa trên sự thay đổi vị trí chuột
        if (isMouseDown)
        {
            float mouseDeltaY = Input.mousePosition.y - initialMousePositionY; // Sự thay đổi vị trí chuột
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + mouseDeltaY); // Di chuyển text
            initialMousePositionY = Input.mousePosition.y; // Cập nhật vị trí chuột hiện tại

            // Tăng alpha (làm rõ chữ) khi kéo chuột lên trên
            if (rectTransform.anchoredPosition.y < fadeStartPositionY)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime * 0.5f); // Tăng alpha dần
            }
        }
        else
        {
            // Di chuyển chữ lên trên khi không nhấn chuột (sử dụng tốc độ cố định)
            rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }

        // Hiệu ứng mờ dần khi gần kết thúc và khi thả chuột, và đạt tới vị trí mờ
        if (fadeStarted && canvasGroup != null && rectTransform.anchoredPosition.y > fadeStartPositionY)
        {
            // Làm chậm hiệu ứng mờ
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * 0.5f); // Chậm lại bằng cách nhân với 0.5
        }

        // Kiểm tra nếu vượt qua điểm cuối
        if (rectTransform.anchoredPosition.y > endPositionY)
        {
            Debug.Log("Credits Finished!");
        }

        // Logic điều khiển thay đổi ảnh với hiệu ứng mờ đi và xuất hiện
        imageTimer += Time.deltaTime;

        if (imageTimer >= imageFadeTime)
        {
            // Làm mờ ảnh hiện tại
            images[currentImageIndex].CrossFadeAlpha(0f, 1f, false);

            // Chuyển sang ảnh tiếp theo
            currentImageIndex = (currentImageIndex + 1) % images.Count;

            // Làm rõ ảnh mới
            images[currentImageIndex].CrossFadeAlpha(1f, 1f, false);

            // Reset bộ đếm thời gian
            imageTimer = 0f;
        }
    }
}
