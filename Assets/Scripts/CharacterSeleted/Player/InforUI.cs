using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class InforUI : MonoBehaviour
{
    public TextMeshProUGUI hpUI;
    public AttributesManager attributesManager;
    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner)
    //    {
    //        enabled = false;
    //        return;
    //    }
    //    //if(NetworkManager.Singleton.LocalClient != null)
    //    //{
    //    //    var playerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
    //    //    if(playerObject != null)
    //    //    {
                 
               
    //    //    }
    //    //}
    //}

    void Start()
    {
       // attributesManager = GetComponent<AttributesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hpUI.text = attributesManager.health.ToString();
    }
}
