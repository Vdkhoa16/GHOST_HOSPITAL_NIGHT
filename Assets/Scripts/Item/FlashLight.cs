using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light flashlight;
   // public float maxTimeLight = 60f;
    public float batteryConsumptionRate = 1f;

    private bool isFlashLightOn = false;
    //public float currentBatteryLife;

    private bool isUseLight;

    private AttributesManager attributesManager;
    void Start()
    {
        attributesManager = GetComponentInParent<AttributesManager>();
    }

    void Update()
    {
        if (attributesManager.currentPin <= 0)
        {
            flashlight.enabled = false;
        }
        if (isFlashLightOn)
        {
            UseFlashLight();
        }
    }

    public void UseFlashLight()
    {
        if (attributesManager.currentPin > 0)
        {
            attributesManager.currentPin -= batteryConsumptionRate * Time.deltaTime;
            if(attributesManager.currentPin <= 0)
            {
                attributesManager.currentPin = 0;
                flashlight.enabled = false;
                isFlashLightOn = false;
            }
           // Debug.Log("Pin còn lại: " + attributesManager.currentPin);
        }
    }

    public bool IsFlashLightOn()
    {
        isFlashLightOn = true;
        return isFlashLightOn;
    }

    //public float Battery()
    //{
    //    return (currentBatteryLife / maxTimeLight) * 100;
    //}
}
