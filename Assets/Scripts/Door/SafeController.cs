using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class SafeController : NetworkBehaviour
{
    public GameObject pickupButton;
    private Animator animator;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>(true); // trạng thái của cửa
    private bool isPlayerInRange = false; // vùng hiển thị button
   // [SerializeField] private AudioSource SoundOpen, SoundClose;

    public bool requiresKey = false; // Cửa có yêu cầu chìa khóa không
    public int keyID; // ID của chìa khóa cần để mở cửa
    public TextMeshProUGUI notificationText;
    [SerializeField] private GameObject PassPanel;

    [SerializeField] private GameObject letter;
    [SerializeField] private Transform transformletterl;
    [SerializeField] private BoxCollider boxCollider;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("SateD", true);
        if (pickupButton != null)
        {
            pickupButton.SetActive(false);
        }
        isOpen.OnValueChanged += OnDoorStateChanged;
        letter.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            PassPanel.SetActive(true);
            ONMose();

      
        }
    }




    public void ONMose()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
       // letter = Instantiate(letter);
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
        letter.SetActive(true);
        boxCollider.enabled = false;
        
    }

    //public void SoundOpenDoor()
    //{
    //    SoundOpen.Play();
    //}
    //public void SoundCloseDoor()
    //{
    //    SoundClose.Play();
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

        pickupButton.SetActive(visible);
        isPlayerInRange = visible;
    }
}
