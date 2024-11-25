using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class ElectoPanel : NetworkBehaviour
{
    public GameObject pickupButton;
    private NetworkVariable<bool> isOn = new NetworkVariable<bool>(false);
    private bool isPlayerInRange = false;

    public bool requiresKey = false;
    public int keyID;
    public TextMeshProUGUI notificationText;

    public GameObject gameObject1; // The GameObject to activate/deactivate
    public GameObject Audio1;
    public GameObject Audio2;
    public PlayerInventory playerInventory;

    void Start()
    {
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }
        isOn.OnValueChanged += OnElectoStateChanged;

        // Ensure the GameObject and sound reflect the initial state
        UpdateElectoState(isOn.Value);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            if (requiresKey)
            {
                if (playerInventory != null && playerInventory.HasKey(keyID))
                {
                    ToggleElectoServerRpc();
                    requiresKey = false;
                }
                else
                {
                    ShowNotification("không có ' Máy kích điện '");
                }
            }
            else
            {
                ToggleElectoServerRpc();
            }
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
            //if (other.GetComponent<NetworkObject>().IsOwner)
            //{
            //    playerInventory = other.GetComponent<PlayerInventory>();
            //    pickupButton.SetActive(true);
            //    isPlayerInRange = true;
            //}
            //else
            //{
            //    pickupButton.SetActive(false);
            //}
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

    private void OnElectoStateChanged(bool oldState, bool newState)
    {
        UpdateElectoState(newState);
    }
    public bool IsOn()
    {
        return isOn.Value;
    }

    private void UpdateElectoState(bool on)
    {
        if (on)
        {
            // State: "Mở"
            gameObject1.SetActive(false);
            //if (Sound1.isPlaying) Sound1.Stop(); // Stop Sound1
            //Sound2.Play(); // Play Sound2
            Audio2.gameObject.SetActive(true);
            Audio1.gameObject.SetActive(false);
        }
        else
        {
            // State: "Chưa mở"
            gameObject1.SetActive(true);
            //if (!Sound1.isPlaying) Sound1.Play(); // Play Sound1 
            //if (Sound2.isPlaying) Sound2.Stop(); // Stop Sound2
            Audio2.gameObject.SetActive(false);
            Audio1.gameObject.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleElectoServerRpc(ServerRpcParams rpcParams = default)
    {
        if (!isOn.Value) // Chỉ cho phép bật nếu trạng thái hiện tại là "tắt"
        {
            isOn.Value = true;
        }
        //if (isOn.Value)
        //{
        //    ShowNotification("Máy kích điện đã được bật, không thể tắt!");
        //}
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
