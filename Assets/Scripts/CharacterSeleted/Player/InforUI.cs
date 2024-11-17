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
    public TextMeshProUGUI TextliveUI;
    private AttributesManager attributesManager;
    public GameObject pinGameObject;
    public GameObject[] pinUI;
    public float maxPin = 60f;
    public GameObject hand;

    public TextMeshProUGUI timerUI; // Add a UI element to display the timer
    private float timeRemaining = 1800f; // 30 minutes in seconds
    private bool timerIsRunning = true;
    private FlashLight flashLight;
    void Start()
    {
        attributesManager = GetComponentInParent<AttributesManager>();
        pinGameObject.SetActive(false);
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
            liveUI.text = "Day: " + attributesManager.live.ToString();
            TextliveUI.text = "Day: " + attributesManager.textlive.ToString();

        }
        // Update the countdown timer
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("end");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
        if (hand.GetComponentInChildren<FlashLight>()) 
        {
            pinGameObject.SetActive(true);
            float pin;
            pin = (attributesManager.currentPin / attributesManager.maxPin) * 100;
            Debug.Log(pin);
            if (pin > 50)
            {
                pinUI[0].SetActive(true);
                pinUI[1].SetActive(false);
            }
            if (pin < 50)
            {
                pinUI[0].SetActive(false);
                pinUI[1].SetActive(true);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1; // Adding 1 second to show 00:00 instead of -00:01 when time runs out

        // Convert time to minutes and seconds format
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the timer UI text in MM:SS format
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}