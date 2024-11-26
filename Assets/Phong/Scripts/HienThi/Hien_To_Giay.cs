using UnityEngine; 
using TMPro; 
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;

public class Hien_To_Giay : NetworkBehaviour
{
    [SerializeField] private GameObject canvas; // Canvas chứa đoạn văn bản
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
            //if (other.GetComponent<NetworkObject>().IsOwner)
            //{
            //    isNearObject = true;
            //    showPressEUI(true);
            //}
            //else
            //{
            //    isNearObject = false;
            //    showPressEUI(false);
            //}

            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            SetPickupButtonVisibilityServerRpc(clientId, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //    isNearObject = false;
            //    showPressEUI(false);
            //}
            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            SetPickupButtonVisibilityServerRpc(clientId, false);
        }
    }

    //void showPressEUI(bool show)
    //{
    //    pressEUI.SetActive(show);
    //}


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
