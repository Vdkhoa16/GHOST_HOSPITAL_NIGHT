using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Tager_Door : NetworkBehaviour
{
    public GameObject pickupButton;
    private Animator animator;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>(true); // trạng thái của cửa
    private bool isPlayerInRange = false; // vùng hiển thị button
    [SerializeField] private AudioSource SoundOpen, SoundClose;

    public bool requiresKey = false; // Cửa có yêu cầu chìa khóa không
    public int keyID; // ID của chìa khóa cần để mở cửa

    public TextMeshProUGUI notificationText;

    public PlayerInventory playerInventory;

    public NavMeshObstacle navMeshObstacle;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("SateD", true);
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }
        isOpen.OnValueChanged += OnDoorStateChanged;

        //navMeshObstacle.carving = true;


    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            

            if (requiresKey)
            {
                // Check if the player has the correct key
                if (playerInventory != null && playerInventory.HasKey(keyID))
                {
                    ToggleDoorServerRpc();

                    //remove
                    requiresKey = false;
                }
                else
                {
                    ShowNotification("Không có chìa khóa");
                }
            }
            else
            {
                ToggleDoorServerRpc();
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
            //playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                playerInventory = other.GetComponent<PlayerInventory>();
            }
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
            // Cập nhật NavMeshObstacle khi cửa đóng
            if (navMeshObstacle != null)
            {
                navMeshObstacle.carving = true; // Tạo chướng ngại vật cho AI
            }
        }
        else
        {
            // Nếu cửa đang đóng, mở cửa
            animator.SetBool("OpenD", true);
            animator.SetBool("closeD", false);
            /*            Debug.Log("Opening door");*/
            if (navMeshObstacle != null)
            {
                navMeshObstacle.carving = false; // Cho phép AI đi qua
            }
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
        Debug.Log("Toggled Door. New state: " + isOpen.Value);  // In ra trạng thái mới để kiểm tra
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
