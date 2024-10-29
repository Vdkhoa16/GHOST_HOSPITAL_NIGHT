﻿using TMPro;
using UnityEngine;
using UnityEngine.UI; // Nhập không gian tên UnityEngine.UI để sử dụng các thành phần UI như Button

public class HienKaypad : MonoBehaviour
{
    public GameObject pressEUI; // Tạo một biến công khai để lưu trữ GameObject hiển thị chữ "E"
    public GameObject KeypadStandard; // Tạo một biến công khai để lưu trữ GameObject tờ giấy UI
    public Image image;
    public Button closeButton; // Tạo biến công khai cho nút đóng


    private bool isNearObject = false; // Biến riêng tư để kiểm tra người chơi có ở gần vật thể không

    void Start()
    {
        // Gán hàm HidePaper cho nút đóng khi được nhấn
        closeButton.onClick.AddListener(HidePaper);
    }

    void Update()
    {
        // Kiểm tra khi nhấn phím E và người chơi đang ở gần vật thể
        if (isNearObject && Input.GetKeyDown(KeyCode.E))
        {
            // Hiện InputField và thiết lập bắt đầu nhập liệu
            ActivateInputFields(); // Gọi hàm để hiển thị các InputField
        }
    }

    public void ActivateInputFields()
    {
        pressEUI.gameObject.SetActive(false);
        KeypadStandard.SetActive(true); // Hiện tờ giấy  
        closeButton.gameObject.SetActive(true); // Hiện nút đóng
        image.gameObject.SetActive(true);
        // Mở khóa và hiển thị con trỏ chuột
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePaper()
    {
        KeypadStandard.SetActive(false); // Ẩn tờ giấy
        closeButton.gameObject.SetActive(false); // Ẩn nút đóng
        image.gameObject.SetActive(false);
        // Khóa và ẩn con trỏ chuột lại
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu đối tượng va chạm có tag "VaCham"
        {
            isNearObject = true; // Đánh dấu người chơi ở gần vật thể
            showPressEUI(true);  // Hiển thị chữ "E"
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu đối tượng rời khỏi vùng va chạm
        {
            isNearObject = false; // Đánh dấu người chơi không còn ở gần vật thể
            showPressEUI(false);  // Ẩn chữ "E"
        }
    }

    void showPressEUI(bool show) // Hàm để hiển thị/ẩn chữ "E"
    {
        pressEUI.SetActive(show); // Chuyển đổi trạng thái hiển thị của pressEUI
    }
}
