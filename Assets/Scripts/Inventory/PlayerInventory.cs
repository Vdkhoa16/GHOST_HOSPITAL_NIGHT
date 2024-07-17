using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class PlayerInventory : NetworkBehaviour
{
    [Header("InventorySetting")]
    public List<InventoryObject> inventoryObjects = new List<InventoryObject>();
    GameObject invPanel;
    Transform invObjectHolder;
    [SerializeField] GameObject invCanvasObject;
    [SerializeField] KeyCode inventoryButtom = KeyCode.Tab;

    [Header("PickupSetting")]
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] float pickupDistance;
    [SerializeField] KeyCode pickupButtom = KeyCode.E;

    [SerializeField] BoxCollider boxCollider;
    [SerializeField] GameObject pickUpItem_gameobject;
    private bool isPlayerInRange = false;
    private NetworkObject networkObject;
    Transform worldObjectHolder;
    public Transform playerTransform;

    void Start()
    {
        pickUpItem_gameobject.SetActive(false);
        networkObject = GetComponent<NetworkObject>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        worldObjectHolder = GameObject.FindGameObjectWithTag("WorldObjects").transform;
        invPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
        invObjectHolder = GameObject.FindGameObjectWithTag("InventoryObjectHolder").transform;
        pickUpItem_gameobject = GameObject.FindGameObjectWithTag("PickE");

        if (invPanel.activeSelf)
        {
            ToggleInventory();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & pickupLayer) != 0)
        {
            GroundItem groundItem = other.GetComponent<GroundItem>();
            if (groundItem != null)
            {
                isPlayerInRange = true;
                pickUpItem_gameobject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & pickupLayer) != 0)
        {
            GroundItem groundItem = other.GetComponent<GroundItem>();
            if (groundItem != null)
            {
                isPlayerInRange = false;
                pickUpItem_gameobject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(pickupButtom))
        {
            Debug.Log("Nhấn e");
            PickUp();

        }

        if (Input.GetKeyDown(inventoryButtom))
        {
            ToggleInventory();
        }
    }

    void PickUp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, pickupDistance, pickupLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            GroundItem groundItem = hitCollider.GetComponent<GroundItem>();
            if (groundItem != null)
            {
                AddToInventory(groundItem.itemScriptable);
                NetworkObject networkObject = hitCollider.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    Debug.Log(networkObject);
                    PickItemServerRpc(networkObject);
                    pickUpItem_gameobject.SetActive(false);
                }
                return;
            }

        }
    }

    void AddToInventory(Item newItem)
    {
        foreach (InventoryObject inventoryObject in inventoryObjects)
        {
            if (inventoryObject.item == newItem)
            {
                inventoryObject.amount++;
                return;
            }
        }
        inventoryObjects.Add(new InventoryObject() { item = newItem, amount = 1 });
    }
    [ServerRpc(RequireOwnership = false)]
    public void PickItemServerRpc(NetworkObjectReference networkObjectReference)
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            PickItemClientRpc(networkObjectReference);
            Destroy(networkObject.gameObject);
        }
    }

    [ClientRpc]
    public void PickItemClientRpc(NetworkObjectReference networkObjectReference)
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            Destroy(networkObject.gameObject);
        }
    }

    void ToggleInventory()
    {
        if (invPanel.activeSelf)
        {
            invPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            UpdateInventoryUI();
            invPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void UpdateInventoryUI()
    {
        foreach (Transform child in invObjectHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (InventoryObject invObj in inventoryObjects)
        {
            GameObject obj = Instantiate(invCanvasObject, invObjectHolder);
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = invObj.item.itemName + " - " + invObj.amount;
            obj.transform.GetChild(1).GetComponent<Image>().sprite = invObj.item.itemImage;
            obj.GetComponent<Button>().onClick.AddListener(delegate { DropItem(invObj.item); });
        }
    }

    void DropItem(Item item)
    {
        foreach (InventoryObject invObj in inventoryObjects)
        {
            if (invObj.item != item)
            {
                continue;
            }
            if (invObj.amount > 1)
            {
                invObj.amount--;
                DropItemServerRpc(invObj.item.prefab.name, boxCollider.transform.position + boxCollider.transform.forward);
                Debug.Log(invObj.item.prefab.name);
                UpdateInventoryUI();
                return;
            }
            if (invObj.amount <= 1)
            {

                inventoryObjects.Remove(invObj);
                DropItemServerRpc(invObj.item.prefab.name, boxCollider.transform.position + boxCollider.transform.forward);
                Debug.Log(invObj.item.prefab.name);
                UpdateInventoryUI();
                return;
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void DropItemServerRpc(string prefabName, Vector3 position)
    {
        DropItemClientRpc(prefabName, position);
    }

    [ClientRpc]
    void DropItemClientRpc(string prefabName, Vector3 position)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null)
        {
            Debug.LogError("Failed to load prefab: " + prefabName);
            return;
        }

        GameObject drop = Instantiate(prefab, position, Quaternion.identity, worldObjectHolder);
        drop.GetComponent<NetworkObject>().Spawn();

    }


    [System.Serializable]
    public class InventoryObject
    {
        public Item item;
        public int amount;
    }
}
