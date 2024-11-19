using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class FlashLight : NetworkBehaviour
{
    public Light flashlight;
    // public float maxTimeLight = 60f;
    public float batteryConsumptionRate = 1f;

    public bool isFlashLightOn = false;
    //public float currentBatteryLife;

    private bool isUseLight;

    private AttributesManager attributesManager;

    void Start()
    {
        attributesManager = GetComponentInParent<AttributesManager>();

    }

    void Update()
    {


        if (isFlashLightOn)
        {
            UseFlashLight();
        }
        if (attributesManager.currentPin <= 0)
        {

            isFlashLightOn = false;
            flashlight.enabled = false;
        }
        else
        {
            isFlashLightOn = true;
            flashlight.enabled = true;
        }

    }

    public void UseFlashLight()
    {
        if (attributesManager.currentPin > 0)
        {
            attributesManager.currentPin -= batteryConsumptionRate * Time.deltaTime;
            if (attributesManager.currentPin <= 0)
            {
                attributesManager.currentPin = 0;

            }
            // Debug.Log("Pin còn lại: " + attributesManager.currentPin);
        }
    }

    public void IsFlashLightOn()
    {
        isFlashLightOn = true;
    }


}