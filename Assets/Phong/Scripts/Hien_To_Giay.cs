using UnityEngine; 
using TMPro; 
using UnityEngine.UI;

public class Hien_To_Giay : MonoBehaviour
{
    public GameObject pressEUI;
    public GameObject paperUI;
    public TMP_InputField inputFieldH;
    public TMP_InputField inputFieldC;
    public TMP_InputField inputFieldF;
    public TMP_InputField inputFieldY;
    public TextMeshProUGUI feedbackText;
    public Button closeButton;
    public Button submitButton;
    public Button submitButton1;

    private bool isNearObject = false;

    void Start()
    {
        closeButton.onClick.AddListener(HidePaper);
    }

    void Update()
    {
        if (isNearObject && Input.GetKeyDown(KeyCode.E))
        {
            ActivateInputFields();
        }
    }

    public void ActivateInputFields()
    {
        paperUI.SetActive(true);
        inputFieldH.gameObject.SetActive(true);
        inputFieldC.gameObject.SetActive(true);
        inputFieldF.gameObject.SetActive(true);
        inputFieldY.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        submitButton1.gameObject.SetActive(true);
        pressEUI.gameObject.SetActive(false);
        inputFieldH.ActivateInputField();
        // Mở khóa và hiển thị con trỏ chuột
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HidePaper()
    {
        paperUI.SetActive(false);
        inputFieldH.gameObject.SetActive(false);
        inputFieldC.gameObject.SetActive(false);
        inputFieldF.gameObject.SetActive(false);
        inputFieldY.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        submitButton1.gameObject.SetActive(false);
        // Khóa và ẩn con trỏ chuột lại
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearObject = true;
            showPressEUI(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearObject = false;
            showPressEUI(false);
        }
    }

    void showPressEUI(bool show)
    {
        pressEUI.SetActive(show);
    }
}
