using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using static PlayerInventory;

public class Grenade : MonoBehaviour
{
    //public Transform cameraTransform; // Camera chính
    //public GameObject PlooWall;       // Vật thể thực sự
    //public GameObject previewPrefab; // Prefab tạm thời
    //private GameObject previewObject; // Đối tượng tạm thời
    void Start()
    {
        //cameraTransform = Camera.main.transform;


    }

    //public void PrepareGrenade()
    //{

    //        if (previewObject == null)
    //        {
    //            // Tạo đối tượng tạm thời
    //            previewObject = Instantiate(previewPrefab);
    //        }

    //        // Xoay và di chuyển theo camera
    //        Quaternion previewRotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
    //        Vector3 previewPosition = cameraTransform.position + cameraTransform.forward * 5f;

    //        previewObject.transform.SetPositionAndRotation(previewPosition, previewRotation);


    //}

    //public void UseIceWall(ItemData itemData, Transform previewObject)
    //{


    //    GameObject prefab = Resources.Load<GameObject>(itemData.prefabName);
    //    if (prefab != null)
    //    {

    //        GameObject itemObject = Instantiate(prefab, previewObject);

    //        // Kiểm tra nếu prefab có NetworkObject
    //        NetworkObject networkObject = itemObject.GetComponent<NetworkObject>();
    //        if (networkObject != null)
    //        {
    //            // Spawn object trên server
    //            networkObject.Spawn();

    //        }

    //    }
    //}
    //[ServerRpc(RequireOwnership = false)]
    //public void UseIceWallServerRpc(ItemData itemData, Transform previewObject)
    //{
    //    UseIceWallClientRpc(itemData, previewObject);
    //}


    //[ClientRpc]
    //public void UseIceWallClientRpc(ItemData itemData, Transform previewObject)
    //{
    //    UseIceWall(itemData, previewObject);
    //}
}
