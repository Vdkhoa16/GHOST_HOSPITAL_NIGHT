using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class InforUI : MonoBehaviour
{
    public TextMeshProUGUI hpUI;
    public TextMeshProUGUI speedUI;
    public TextMeshProUGUI liveUI;
    private AttributesManager attributesManager;
    void Start()
    {
        attributesManager = GetComponentInParent<AttributesManager>();

        // Kiểm tra xem có phải là đối tượng của player cục bộ hay không
        if (GetComponentInParent<NetworkObject>().IsOwner)
        {
            // Hiển thị UI nếu là player cục bộ
            this.gameObject.SetActive(true);
        }
        else
        {
            // Ẩn UI nếu không phải là player cục bộ
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (attributesManager != null)
        {
            hpUI.text = "HP: " + attributesManager.health.ToString();
            speedUI.text = "Speed: " + attributesManager.running_Speed.ToString();
            liveUI.text = "Live: "+ attributesManager.live.ToString();
        }
    }
}