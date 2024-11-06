using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Button veryLowButton;
    public Button lowButton;
    public Button mediumButton;
    public Button highButton;
    public Button veryHighButton;
    public Button ultraButton;

    private void Start()
    {
        // Lấy và áp dụng mức chất lượng đã lưu khi khởi động
        int savedQualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
        QualitySettings.SetQualityLevel(savedQualityLevel);

        // Thiết lập nút được chọn ban đầu
        SetSelectedButton(savedQualityLevel);
       
        // Ẩn panel sau khi thiết lập ban đầu
        this.gameObject.SetActive(false);
    }

    // Hàm này sẽ được gọi mỗi khi mở lại panel
    private void OnEnable()
    {
        // Đặt lại nút được chọn dựa trên mức chất lượng đã lưu
        int savedQualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
        SetSelectedButton(savedQualityLevel);
    }

    public void VeryLow()
    {
        SetQualityLevel(0);
    }

    public void Low()
    {
        SetQualityLevel(1);
    }

    public void Medium()
    {
        SetQualityLevel(2);
    }

    public void High()
    {
        SetQualityLevel(3);
    }

    public void VeryHigh()
    {
        SetQualityLevel(4);
    }

    public void Ultra()
    {
        SetQualityLevel(5);
    }

    private void SetQualityLevel(int level)
    {
        // Đặt mức chất lượng và lưu nó vào bộ nhớ
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("QualityLevel", level);
        PlayerPrefs.Save();
    }

    private void SetSelectedButton(int qualityLevel)
    {
        // Đặt nút được chọn dựa trên mức chất lượng
        switch (qualityLevel)
        {
            case 0:
                EventSystem.current.SetSelectedGameObject(veryLowButton.gameObject);
                break;
            case 1:
                EventSystem.current.SetSelectedGameObject(lowButton.gameObject);
                break;
            case 2:
                EventSystem.current.SetSelectedGameObject(mediumButton.gameObject);
                break;
            case 3:
                EventSystem.current.SetSelectedGameObject(highButton.gameObject);
                break;
            case 4:
                EventSystem.current.SetSelectedGameObject(veryHighButton.gameObject);
                break;
            case 5:
                EventSystem.current.SetSelectedGameObject(ultraButton.gameObject);
                break;
            default:
                // Nếu không có giá trị phù hợp, chọn mức chất lượng "Medium" làm mặc định
                EventSystem.current.SetSelectedGameObject(mediumButton.gameObject);
                break;
        }
    }
}
