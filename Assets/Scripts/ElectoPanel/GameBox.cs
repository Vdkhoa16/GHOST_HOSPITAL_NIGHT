using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Gamebox : NetworkBehaviour
{
    public GameObject pickupButton;
    private NetworkVariable<bool> isOn = new NetworkVariable<bool>(false);
    private bool isPlayerInRange = false;

    public TextMeshProUGUI notificationText;
    public GameObject Audio1;

    void Start()
    {
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }

        isOn.OnValueChanged += OnElectoStateChanged;

        // Đảm bảo trạng thái ban đầu của âm thanh phù hợp với `isOn`
        UpdateElectoState(isOn.Value);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            ToggleElectoServerRpc(); // Thay đổi trạng thái
        }
    }

    void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        StartCoroutine(HideNotificationAfterSeconds(3));
    }

    IEnumerator HideNotificationAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        notificationText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
                SetPickupButtonVisibilityServerRpc(clientId, true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
                SetPickupButtonVisibilityServerRpc(clientId, false);
            }
        }
    }

    private void OnElectoStateChanged(bool oldState, bool newState)
    {
        UpdateElectoState(newState);
    }

    private void UpdateElectoState(bool on)
    {
        // Chỉ bật hoặc tắt Audio1
        Audio1.SetActive(on);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleElectoServerRpc(ServerRpcParams rpcParams = default)
    {
        isOn.Value = !isOn.Value; // Đảo trạng thái
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
        if (pickupButton != null)
        {
            pickupButton.SetActive(visible);
        }

        isPlayerInRange = visible;
    }
}
