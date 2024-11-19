using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPhim1 : MonoBehaviour
{
    [SerializeField] private TMP_Text Ans;
    [SerializeField] private Animator Door;
    [SerializeField] private HienBanPhimNhapMK hienBanPhimNhapMK; // Tham chiếu tới script HienBanPhimNhapMK
    [SerializeField] private string Answer = "666333";

    public void Number(int number)
    {
        Ans.text += number.ToString();
    }
    public void Enter()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "Chính xác!";
            Door.SetBool("isOpen", true);
            Invoke("CloseButtonAfterDelay", 2f); // Gọi hàm CloseButtonAfterDelay sau 2 giây
        }
        else
        {
            Ans.text = "Sai!";
            Invoke("ClearText", 1f); // Chờ 1 giây trước khi xóa
        }
    }
    private void ClearText()
    {
        Ans.text = ""; // Xóa nội dung
    }
    private void CloseButtonAfterDelay()
    {
        hienBanPhimNhapMK.HidePaper(); // Gọi hàm HidePaper từ HienBanPhimNhapMK
    }
}
