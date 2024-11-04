using UnityEngine;
using UnityEngine.UI;

public class CodeHandler : MonoBehaviour
{
    public GameObject CodeCanvas;
    public InputField Digit1, Digit2, Digit3, Digit4;
    public Button confirmButton, exitButton;
    private string correctCode = "12345";

    void Start()
    {
        CodeCanvas.SetActive(false); // Ẩn canvas lúc đầu
        confirmButton.onClick.AddListener(CheckCode); // Gán hàm kiểm tra mã cho nút xác nhận
        exitButton.onClick.AddListener(CloseCanvas);  // Gán hàm đóng cho nút thoát
    }

    void Update()
    {
        // Khi nhấn F, bật hoặc tắt canvas
        if (Input.GetKeyDown(KeyCode.F))
        {
            CodeCanvas.SetActive(!CodeCanvas.activeSelf);
        }
    }

    void CheckCode()
    {
        string enteredCode = Digit1.text + Digit2.text + Digit3.text + Digit4.text;

        if (enteredCode == correctCode)
        {
            Debug.Log("Đúng!");
            // Thực hiện hành động khi nhập đúng, ví dụ mở cửa hoặc tiếp tục game
        }
        else
        {
            Debug.Log("Sai, vui lòng thử lại.");
            // Xóa các ô nhập nếu nhập sai
            Digit1.text = "";
            Digit2.text = "";
            Digit3.text = "";
            Digit4.text = "";
        }
    }

    void CloseCanvas()
    {
        CodeCanvas.SetActive(false); // Ẩn canvas khi nhấn thoát
    }
}
