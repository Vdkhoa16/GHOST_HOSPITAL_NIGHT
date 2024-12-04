using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
public class ChucNangBanPhimhim1 : NetworkBehaviour
{
    [SerializeField] private TMP_Text Ans;
    [SerializeField] private Animator Door;
    [SerializeField] private HienKaypad hienKaypad; // Tham chiếu tới script HienBanPhimNhapMK
    [SerializeField] private string Answer = "666333";

    [SerializeField] private GameObject key;
    private BoxCollider boxItem;

     void Start()
    {

        boxItem = key.GetComponent<BoxCollider>();

            boxItem.enabled = false;
          
    }
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
            // Xóa script HienKaypad
            Destroy(hienKaypad);
            GOServerRpc();
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
        hienKaypad.HidePaper(); // Gọi hàm HidePaper từ HienBanPhimNhapMK
    }
    //Open
    [ServerRpc(RequireOwnership = false)]
    public void GOServerRpc(ServerRpcParams rpcParams = default)
    {
        GOClientRpc();
    }
    [ClientRpc]
    public void GOClientRpc(ClientRpcParams rpcParams = default)
    {
        GOBox();
    }
    public void GOBox()
    {
        boxItem.enabled = true;
        Debug.Log("box da duoc bat");
    }
    public void OnObject()
    {
        GOServerRpc();
    }


}
