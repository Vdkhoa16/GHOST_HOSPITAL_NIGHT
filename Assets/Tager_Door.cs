using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Tager_Door : NetworkBehaviour
{
    public GameObject pickupButton; // Tham chiếu đến nút nhặt
    private Animator animator;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>(false); // trạng thái của cửa
    private bool isPlayerInRange = false; // vùng hiển thị button
    [SerializeField] private AudioSource SoundOpen, SoundClose;

    public bool requiresKey = false; // Cửa có yêu cầu chìa khóa không
    public int keyID; // ID của chìa khóa cần để mở cửa
    public TextMeshPro notificationText;
    private KeyManager keyManager;  // Tham chiếu đến KeyManager
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("SateD", true);
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }

        isOpen.OnValueChanged += OnDoorStateChanged;
        // Tìm KeyManager trong scene (hoặc gán từ một đối tượng khác)
        keyManager = GameObject.FindObjectOfType<KeyManager>();

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (requiresKey)
            {
                if (keyManager != null && keyManager.HasKey(keyID))
                {
                    // Người chơi có chìa khóa, mở cửa và xóa chìa khóa
                    ToggleDoorServerRpc();
                }
                else
                {
                    // Người chơi không có chìa khóa
                    ShowNotification("Cần có chìa khóa để mở cửa");
                }
            }
            else
            {
                if (isPlayerInRange)
                {
                    if (IsClient)
                    {
                        ToggleDoorServerRpc();
                    }
                }
            }


        }
    }
    void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);  // Hiển thị thông báo
        StartCoroutine(HideNotificationAfterSeconds(3));  // Ẩn sau 3 giây
    }
    IEnumerator HideNotificationAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);  // Đợi trong 3 giây
        notificationText.gameObject.SetActive(false);  // Ẩn thông báo
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            SetPickupButtonVisibilityServerRpc(clientId, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            SetPickupButtonVisibilityServerRpc(clientId, false);
        }
    }

    private void OnDoorStateChanged(bool oldState, bool newState)
    {

        UpdateDoorState(newState);
    }

    private void UpdateDoorState(bool open)
    {
        if (open)
        {
            // Nếu cửa đang mở, đóng cửa
            animator.SetBool("closeD", true);
            OnCloseDAnimationEnd();
            animator.SetBool("OpenD", false);
            /*            Debug.Log("Closing door");*/
        }
        else
        {
            // Nếu cửa đang đóng, mở cửa
            animator.SetBool("OpenD", true);
            animator.SetBool("closeD", false);
            /*            Debug.Log("Opening door");*/
        }
    }

    public void OnCloseDAnimationEnd()
    {
        animator.SetBool("SateD", true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleDoorServerRpc(ServerRpcParams rpcParams = default)
    {
        isOpen.Value = !isOpen.Value;
    }

    public void SoundOpenDoor()
    {
        SoundOpen.Play();
    }
    public void SoundCloseDoor()
    {
        SoundClose.Play();
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

        pickupButton.SetActive(visible);
        isPlayerInRange = visible;
    }


}
