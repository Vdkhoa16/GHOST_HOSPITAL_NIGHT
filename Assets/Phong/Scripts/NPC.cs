using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : NetworkBehaviour
{
    [SerializeField] private GameObject canvas; // Canvas chứa đoạn văn bản
    [SerializeField] private GameObject pressEUI; // Tạo một biến công khai để lưu trữ GameObject hiển thị chữ "E"
    [SerializeField] private Button closeButton; 
    [SerializeField] private TMP_Text textDisplay; // TextMeshPro để hiển thị văn bản
    [SerializeField] private AudioSource audioSource; // AudioSource để phát âm thanh
    [SerializeField] private AudioClip typingSound; // Âm thanh khi văn bản xuất hiện

    private bool isNearObject = false; // Biến riêng tư để kiểm tra người chơi có ở gần vật thể không


   // public NetworkObject networkObject;
    void Start()
    {
        closeButton.onClick.AddListener(HideText);
    }

    void Update()
    {
        if (isNearObject && Input.GetKeyDown(KeyCode.E))
        {
            //ShowText(); // Hiển thị văn bản khi nhấn E
            //if (networkObject != null)
            //{
          
            //}
            ShowText();
        }

    }

    private void ShowText()
    {

        canvas.SetActive(true); // Hiện canvas
        pressEUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Phát âm thanh khi văn bản xuất hiện
        audioSource.PlayOneShot(typingSound);
    }
    private void HideText()
    {
        canvas.SetActive(false); // Ẩn Canvas chứa văn bản
        pressEUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu đối tượng va chạm có tag "VaCham"
        {

            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            SetPickupButtonVisibilityServerRpc(clientId, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu đối tượng rời khỏi vùng va chạm
        {
                                        //    isNearObject = false; // Đánh dấu người chơi không còn ở gần vật thể
                                        //    showPressEUI(false);  // Ẩn chữ "E"
        ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
        SetPickupButtonVisibilityServerRpc(clientId, false);
    }

    }


    [ServerRpc(RequireOwnership = false)]
    private void SetPickupButtonVisibilityServerRpc(ulong clientId, bool visible, ServerRpcParams rpcParams = default)
    {
        SetPickupButtonVisibilityClientRpc(visible, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { clientId }
            }
        });
    }


    [ClientRpc]
    private void SetPickupButtonVisibilityClientRpc(bool visible, ClientRpcParams rpcParams = default)
    {

        pressEUI.SetActive(visible);
        isNearObject = visible;
    }
}
