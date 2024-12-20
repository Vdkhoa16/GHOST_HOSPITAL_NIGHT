﻿using System.Collections.Generic;
using TMPro;
using Unity.Netcode;

using UnityEngine;
using UnityEngine.UI;
using static PlayerInventory;

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
    private Transform playerCamera;
    private Image crosshairImage;  // Tham chiếu đến UI crosshair
    public float pickupRange = 5f;
    private GameObject pickUpItem_gameobject;

    [Header("playerHandTransform")]
    [SerializeField] private Transform playerHandTransform;
    public GameObject currentItemInHand;

    [Header("Animation")]
    private PlayerAnimation playerAnimation;

    [Header("AttributesManager")]
    private AttributesManager playerAttributes;
    [Header("ItemIceWall")]
    public Item iceWall;
    //public GameObject previewPrefab; // Prefab tạm thời
    public Transform previewObject; // Đối tượng tạm thời
    [System.Serializable]
    public struct ItemData : INetworkSerializable
    {
        public string itemName;
        public string prefabName;
        public ItemType itemType;
        public int keyID;

        public ItemData(Item item)
        {
            itemName = item.itemName;
            prefabName = item.prefab.name;
            keyID = item.keyID;
            itemType = item.itemType;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref itemName);
            serializer.SerializeValue(ref prefabName);
            serializer.SerializeValue(ref keyID);
            serializer.SerializeValue(ref itemType);
        }
    }


    //private bool isPlayerInRange = false;
    private NetworkObject networkObject;
    //Transform worldObjectHolder;
    //public Transform playerTransform;

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        pickUpItem_gameobject.SetActive(false);
        networkObject = GetComponent<NetworkObject>();
        playerAttributes = GetComponent<AttributesManager>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        // worldObjectHolder = GameObject.FindGameObjectWithTag("WorldObjects").transform;
        invPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
        invObjectHolder = GameObject.FindGameObjectWithTag("InventoryObjectHolder").transform;
        playerCamera = GameObject.FindWithTag("MainCamera").transform;
        pickUpItem_gameobject = GameObject.FindGameObjectWithTag("PickE");
        crosshairImage = GameObject.FindWithTag("pickupUI").GetComponent<Image>();

        if (invPanel.activeSelf)
        {
            ToggleInventory();
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(pickupButtom))
        {
            Debug.Log("Nhấn e");
            PickUpButton();

        }
        PickUp();
        if (Input.GetKeyDown(inventoryButtom))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            RemoveItemHandServerRpc();
        }

        //if (previewObject.childCount > 0)
        //{
        //    // Dùng vòng lặp để xóa tất cả các con của playerHandTransform
        //    for (int i = previewObject.childCount - 1; i >= 0; i--)
        //    {
        //        // Lấy con và phá hủy nó
        //        Destroy(previewObject.GetChild(i).gameObject);
        //        // playerHandTransform.GetChild(i).gameObject.SetActive(false);

        //    }
        //}
        bool check = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UseItemIceWall(check);
        }
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    if (currentItemInHand.GetComponentInChildren<Grenade>())
        //    {
        //        Grenade grenade = currentItemInHand.GetComponentInChildren<Grenade>();
        //        PrepareGrenade();
        //    }
        //}
    }

    public void PickUpButton()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        // Debug.DrawRay(playerCamera.position, ray.direction * pickupRange, Color.green);

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                GroundItem groundItem = hit.collider.GetComponent<GroundItem>();
                AddToInventory(groundItem.itemScriptable);
                NetworkObject networkObject = hit.collider.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    // gọi aniamtion
                    playerAnimation.PickUp();
                    // Debug.Log(networkObject);
                    PickItemServerRpc(networkObject);
                    pickUpItem_gameobject.SetActive(false);
                }
                return;

            }
        }
    }
    void PickUp()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        // Debug.DrawRay(playerCamera.position, ray.direction * pickupRange, Color.red);

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            Outline outline = hit.collider.GetComponent<Outline>();
            if (hit.collider.CompareTag("Pickup"))
            {
                //Debug.Log("Tìm thấy vật phẩm");
                crosshairImage.color = Color.red;
                pickUpItem_gameobject.SetActive(true);

                // outline.UpdateMaterialProperties();
                outline.OutlineWidth = 10;
            }
            else
            {
                crosshairImage.color = Color.white;
                pickUpItem_gameobject.SetActive(false);
                outline.OutlineWidth = 0;
            }
        }
        else
        {
            crosshairImage.color = Color.white;
            pickUpItem_gameobject.SetActive(false);

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

        if (newItem.itemType == ItemType.isKey)
        {
            Debug.Log("Picked up key with ID: " + newItem.keyID);
        }
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
            obj.transform.GetChild(0).GetComponent<Image>().sprite = invObj.item.itemImage;
            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = invObj.item.itemName;
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = invObj.amount + "";
            obj.GetComponent<Button>().onClick.AddListener(delegate {
                UseItem(invObj.item);
                UseLetter(invObj.item);
            });

        }
    }

    void MoveItemToHand(ItemData itemData)
    {
        // Tìm prefab từ Resources
        GameObject prefab = Resources.Load<GameObject>(itemData.prefabName);
        if (prefab != null)
        {
            // Instantiate prefab vào tay còn lại
            GameObject itemObject = Instantiate(prefab, playerHandTransform);

            // Giữ nguyên vị trí và rotation từ prefab
            itemObject.transform.localPosition = prefab.transform.localPosition;
            itemObject.transform.localRotation = prefab.transform.localRotation;
            Debug.Log("Đang sử dụng item: " + itemData.itemName);
            if (itemData.itemType == ItemType.isFlashLight)
            {
                FlashLight flashLight = currentItemInHand.GetComponentInChildren<FlashLight>();
                flashLight.isFlashLightOn = true;

            }

            // kiểm tra còn tồn tại item nào trên tay không
        }
        else
        {
            Debug.LogError("Không tìm thấy prefab: " + itemData.prefabName);
        }
    }

    void UseLetter(Item item)
    {
        ItemData itemData = new ItemData(item);
        if (item.itemType == ItemType.isLetter)
        {
            GameObject prefab = Resources.Load<GameObject>(itemData.prefabName);

            if (prefab != null)
            {
                // Instantiate prefab vào tay còn lại
                GameObject itemObject = Instantiate(prefab, invPanel.transform);

                // Giữ nguyên vị trí và rotation từ prefab
                itemObject.transform.localPosition = prefab.transform.localPosition;
                itemObject.transform.localRotation = prefab.transform.localRotation;
                Debug.Log("Đang sử dụng item: " + itemData.itemName);


                // kiểm tra còn tồn tại item nào trên tay không
            }

        }
    }

    void UseItem(Item item)
    {
        if (item.itemType == ItemType.isBattery)
        {
            foreach (InventoryObject invObj in inventoryObjects)
            {
                if (invObj.item == item)
                {
                    if (invObj.amount > 0)
                    {
                        //FlashLight flashLight = currentItemInHand.GetComponentInChildren<FlashLight>();
                        //flashLight.IsFlashLightOn();
                        invObj.amount--;
                        playerAttributes.currentPin = playerAttributes.maxPin;
                        if (invObj.amount == 0)
                        {
                            inventoryObjects.Remove(invObj);
                        }
                        UpdateInventoryUI();
                        return;
                    }
                }
            }
        }

        RemoveItemHandServerRpc();
        foreach (InventoryObject invObj in inventoryObjects)
        {
            if (invObj.item == item)
            {
                if (invObj.amount > 0)
                {
                    // Chuyển item thành ItemData và thông báo đến server
                    ItemData itemData = new ItemData(item);
                    MoveItemToHandServerRpc(itemData);

                    // invObj.amount--;
                    if (invObj.amount == 0)
                    {
                        inventoryObjects.Remove(invObj);
                    }
                    UpdateInventoryUI();
                    return;
                }
            }
        }

    }
    // kiểm tra matkhau door
    public bool HasKey(int keyID)
    {
        foreach (InventoryObject inventoryObject in inventoryObjects)
        {
            if (inventoryObject.item.itemType == ItemType.isKey && inventoryObject.item.keyID == keyID)
            {
                return true;
            }
        }
        return false;
    }


    [ServerRpc(RequireOwnership = false)]
    void MoveItemToHandServerRpc(ItemData itemData)
    {
        MoveItemToHandClientRpc(itemData);
    }

    [ClientRpc]
    void MoveItemToHandClientRpc(ItemData itemData)
    {
        MoveItemToHand(itemData);
    }

    // DropItem
    public void DropItem()
    {
        if (currentItemInHand == null)
        {
            Debug.LogWarning("Không có item nào đang cầm trên tay để drop.");
            return;
        }


        InventoryObject heldItem = inventoryObjects.Find(obj => obj.amount > 0);
        if (heldItem == null)
        {
            Debug.LogWarning("Không có item nào đang cầm trên tay để drop.");
            return;
        }
        heldItem.amount--;
        if (heldItem.amount <= 0)
        {
            inventoryObjects.Remove(heldItem);
        }
        UpdateInventoryUI();
    }

    // Đổi Item
    public void SwapItem()
    {

    }

    // Xóa item Trên tay 
    public void RemoveItemHand()
    {

        // Kiểm tra xem playerHandTransform có con không
        if (playerHandTransform.childCount > 0)
        {
            // Dùng vòng lặp để xóa tất cả các con của playerHandTransform
            for (int i = playerHandTransform.childCount - 1; i >= 0; i--)
            {
                // Lấy con và phá hủy nó
                Destroy(playerHandTransform.GetChild(i).gameObject);
                // playerHandTransform.GetChild(i).gameObject.SetActive(false);

            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RemoveItemHandServerRpc()
    {
        RemoveItemHandClientRpc();
    }

    [ClientRpc]
    private void RemoveItemHandClientRpc()
    {
        RemoveItemHand();
    }
    /*void DropItem(Item item)
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

      }*/
    [ServerRpc(RequireOwnership = false)]
    void UseIceWallServerRpc(ItemData itemData , Vector3 position, Quaternion rotation)
    {
        UseIceWallClientRpc(itemData, position, rotation);
    }


    [ClientRpc]
    void UseIceWallClientRpc(ItemData itemData, Vector3 position, Quaternion rotation)
    {
        UseIceWall(itemData,position, rotation);
    
    }


    void UseIceWall(ItemData itemData, Vector3 position, Quaternion rotation)
    {


        GameObject prefab = Resources.Load<GameObject>(itemData.prefabName);
        if (prefab != null)
        {


            //Quaternion previewRotation = Quaternion.LookRotation(playerCamera.forward, Vector3.up);
            //Vector3 previewPosition = previewObject.forward + playerCamera.forward;
            // playerCamera.transform.SetPositionAndRotation(previewPosition, previewRotation);
            GameObject itemObject = Instantiate(prefab, position, rotation);

            // Kiểm tra nếu prefab có NetworkObject
            //NetworkObject networkObject = itemObject.GetComponent<NetworkObject>();
            //if (networkObject != null)
            //{
            //    // Spawn object trên server
            //    networkObject.Spawn();

            //}
            //else
            //{
            //    Debug.LogError("Prefab không có NetworkObject, không thể spawn.");
            //}

        }
        else
        {
            Debug.LogError("Không thể load prefab từ Resources: " + itemData.prefabName);
        }
    }


    public void UseItemIceWall(bool check)
    {
        if (currentItemInHand.GetComponentInChildren<Grenade>())
        {
            Grenade grenade = currentItemInHand.GetComponentInChildren<Grenade>();
            ItemData itemData = new ItemData(iceWall);
            UseIceWallServerRpc(itemData,previewObject.position,previewObject.rotation);
            check = true;
            if (check)
            {

                foreach (InventoryObject invObj in inventoryObjects)
                {
                    Item item = invObj.item;
                    if (item.itemType == ItemType.isGranada)
                    {
                        if (invObj.amount > 0)
                        {
                            invObj.amount--;
                            if (invObj.amount == 0)
                            {
                                inventoryObjects.Remove(invObj);
                                RemoveItemHandServerRpc();
                            }
                            UpdateInventoryUI();
                            return;
                        }
                    }
                }
            }

        }
    }

    //public void PrepareGrenade()
    //{

    //    if (previewObject == null)
    //    {
    //        // Tạo đối tượng tạm thời
    //        //previewObject = Instantiate(previewPrefab); // Đảm bảo sử dụng Transform
    //    }

    //    // Xoay và di chuyển theo camera
    //    Quaternion previewRotation = Quaternion.LookRotation(playerCamera.forward, Vector3.up);
    //    Vector3 previewPosition = playerCamera.position + playerCamera.forward * 5f;

    //    previewObject.transform.SetPositionAndRotation(previewPosition, previewRotation);


    //}
    [System.Serializable]
    public class InventoryObject
    {
        public Item item;
        public int amount;
    }

}