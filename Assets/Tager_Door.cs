using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Tager_Door : NetworkBehaviour
{
    public GameObject pickupButton; // Tham chiếu đến nút nhặt
    private Animator animator;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>(false); // trạng thái của cửa
    private bool isPlayerInRange = false; // vùng hiển thị button
    [SerializeField] private AudioSource SoundOpen,SoundClose;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("SateD", true);
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }

        isOpen.OnValueChanged += OnDoorStateChanged;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (IsClient)
            {
/*                Debug.Log("Nhận được nút E");*/
                ToggleDoorServerRpc();
            }
        }
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
