using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Check : MonoBehaviour
{
    public TMP_InputField inputFieldH; // Input Field cho chữ A
    public TMP_InputField inputFieldC; // Input Field cho chữ B
    public TMP_InputField inputFieldF; // Input Field cho chữ C
    public TMP_InputField inputFieldY; // Input Field cho chữ C
    public TextMeshProUGUI feedbackText; // Text để hiển thị phản hồi
    public Button submitButton; // Nút submit

    void Start()
    {
        // Gán hàm CheckInput cho nút khi được nhấn
        submitButton.onClick.AddListener(CheckInput);
        // Gán hàm để giới hạn ký tự cho các Input Field
        inputFieldH.onValueChanged.AddListener((value) => LimitInputLength(inputFieldH, value));
        inputFieldC.onValueChanged.AddListener((value) => LimitInputLength(inputFieldC, value));
        inputFieldF.onValueChanged.AddListener((value) => LimitInputLength(inputFieldF, value));
        inputFieldY.onValueChanged.AddListener((value) => LimitInputLength(inputFieldY, value));
    }

    void Update()
    {

    }

    private void CheckInput()
    {
        string inputH = inputFieldH.text;
        string inputC = inputFieldC.text;
        string inputF = inputFieldF.text;
        string inputY = inputFieldY.text;

        // Kiểm tra xem tất cả các ô đã được điền đúng
        if (ValidateInput(inputH, inputC, inputF, inputY))
        {
            feedbackText.text = "Đúng!";
            feedbackText.gameObject.SetActive(true); // Hiển thị phản hồi
        }
        else
        {
            feedbackText.text = "Sai!";
            feedbackText.gameObject.SetActive(true); // Hiển thị phản hồi
        }
    }

    private bool ValidateInput(string inputH, string inputC, string inputF, string inputY)
    {
        // Logic để kiểm tra input
        return inputH.Equals("H", System.StringComparison.OrdinalIgnoreCase) &&
               inputC.Equals("C", System.StringComparison.OrdinalIgnoreCase) &&
               inputF.Equals("F", System.StringComparison.OrdinalIgnoreCase) &&
               inputY.Equals("Y", System.StringComparison.OrdinalIgnoreCase);

    }
    private void LimitInputLength(TMP_InputField inputField, string value)
    {
        // Giới hạn số lượng ký tự nhập vào
        if (value.Length > 1)
        {
            inputField.text = value.Substring(0, 1); // Giữ lại ký tự đầu tiên
        }
    }
}
