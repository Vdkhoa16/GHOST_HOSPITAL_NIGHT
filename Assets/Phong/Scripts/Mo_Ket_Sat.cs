﻿using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Mo_Ket_Sat : MonoBehaviour
{
    [SerializeField] private Animator safeAnimator; // Tham chiếu đến Animator của két sắt

    private bool isOpen = false; // Biến để theo dõi trạng thái mở của két sắt
    [SerializeField] private GameObject key;
    private BoxCollider boxItem;
    private void Start()
    {
        //key.SetActive(false);
        boxItem = key.GetComponent<BoxCollider>();
        safeAnimator.SetBool("isOpen", false);
        boxItem.enabled = false;
        
    }
    public void OpenSafe()
    {
        if (!isOpen) // Kiểm tra xem két sắt đã mở chưa
        {
            isOpen = true; // Đánh dấu két sắt là đã mở
            safeAnimator.SetBool("isOpen", true); // Kích hoạt trạng thái Open_Door
           // key.SetActive(true);
            boxItem.enabled = true;
            
        }
    }
}
