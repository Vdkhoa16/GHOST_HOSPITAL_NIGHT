using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class ShowObjectOnPress : NetworkBehaviour
{
    public GameObject mainObject; // Đối tượng chính sẽ hiển thị
    //public GameObject paperObject; // Đối tượng paper sẽ hiển thị
    public GameObject pickE;
    private bool isPlayerNearby = false;
    public bool check = false;

    private void Start()
    {
        // Đảm bảo các đối tượng đều ẩn ban đầu
        mainObject.SetActive(false);
       // paperObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu người chơi tiến vào
        {
            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            SetPickupButtonVisibilityServerRpc(clientId, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu người chơi rời đi
        {
            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            SetPickupButtonVisibilityServerRpc(clientId, false);
        }
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi ở gần và nhấn phím E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            OnFool2ServerRpc(true);
            CheckServerRpc(true);
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

        pickE.SetActive(visible);
        isPlayerNearby = visible;
       // paperObject.SetActive(visible);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnFool2ServerRpc(bool visible)
    {
        OnFool2ClientRpc(visible);
    }

    [ClientRpc]
    private void OnFool2ClientRpc(bool visible, ClientRpcParams rpcParams = default)
    {
        OnFool2(visible);
    }


    private void OnFool2(bool visible)
    {
        mainObject.SetActive(visible);
       // StartCoroutine(ShowMainObjectForSeconds(2f,visible)); // Hiển thị trong 2 giây
     //   paperObject.SetActive(visible); // Giữ paperObject hiển thị
    }

    private IEnumerator ShowMainObjectForSeconds(float duration, bool visible)
    {
    
        yield return new WaitForSeconds(duration);
        mainObject.SetActive(!visible);
    }



    [ServerRpc(RequireOwnership = false)]
    private void CheckServerRpc(bool visible)
    {
        CheckClientRpc(visible);
    }

    [ClientRpc]
    private void CheckClientRpc(bool visible)
    {
        checkBool(visible);
    }

    public void checkBool(bool v)
    {
        if (v)
        {
            check = true;
        }
    }
}
