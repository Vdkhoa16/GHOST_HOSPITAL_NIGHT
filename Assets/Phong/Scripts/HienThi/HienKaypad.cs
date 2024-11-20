using TMPro;
using UnityEngine;
using UnityEngine.UI; // Nhập không gian tên UnityEngine.UI để sử dụng các thành phần UI như Button

public class HienKaypad : MonoBehaviour
{
    [SerializeField] private GameObject canvas; // Canvas chứa đoạn văn bản
    public GameObject btn;
    public GameObject pressEUI;
    public Button closeButton;

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
        canvas.SetActive(true); // Hiện canvas        
        pressEUI.gameObject.SetActive(false);

        // Mở khóa và hiển thị con trỏ chuột
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePaper()
    {
        canvas.SetActive(false); // Ẩn Canvas chứa văn bản

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
