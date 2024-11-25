using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


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

    public TextMeshProUGUI timerUI;
    [SerializeField] private float timeRemaining = 60f;
    private bool timerIsRunning = true;
    private FlashLight flashLight;

    //kiểm tra cửa
    private Door_main doorMain;
    //chuyển sceen
    [SerializeField] private string loadsceen = "GameBox";
    //img win lose + text
    public GameObject endTimerImage;
    public TextMeshProUGUI endTimerText;
    //ấm thanh đếm ngược
    public AudioSource Audiotime;
    private bool isAudioPlayed = false;
    private bool isAudioLooping = false;
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
        endTimerImage.SetActive(false);
        endTimerText.gameObject.SetActive(false);
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
        //// Kiểm tra cửa và thực hiện ưingame
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (doorMain != null && doorMain.IsDoorOpen())
        //    {
        //        WinGame(); // win
        //    }
        //}
        // Update timer
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                // Kiểm tra thời gian còn lại và phát âm thanh lặp
                if (timeRemaining <= 10 )
                {
                    if (!isAudioPlayed)
                    {
                        Audiotime.Play(); // Phát âm thanh lần đầu
                        isAudioPlayed = true; // Đặt cờ đã phát âm thanh
                    }
                    if (!isAudioLooping)
                    {
                        isAudioLooping = true; // Đặt cờ lặp âm thanh
                        StartCoroutine(PlayAudioLoop()); // Bắt đầu lặp
                    }
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                //dừng âm thanh
                StopCoroutine(PlayAudioLoop());
                Audiotime.Stop();
                isAudioLooping = false;

                //kiểm tra cửa đã được mở hay chưa
                if (doorMain != null && doorMain.IsDoorOpen())
                {
                    WinGame();
                }
                else
                {
                    EndGame();
                }
            }
            if (hand.GetComponentInChildren<FlashLight>())
            {

                pinGameObject.SetActive(true);
                float pin;
                pin = (attributesManager.currentPin / attributesManager.maxPin) * 100;
                //Debug.Log(pin);
                if (pin >= 75)
                {
                    for (int i = 0; i < pinUI.Length; i++)
                    {
                        if (pinUI[i] == pinUI[0])
                        {
                            pinUI[i].SetActive(true);
                        }
                        else
                        {
                            pinUI[i].SetActive(false);
                        }
                    }
                }
                else if (pin >= 50)
                {
                    for (int i = 0; i < pinUI.Length; i++)
                    {
                        if (pinUI[i] == pinUI[1])
                        {
                            pinUI[i].SetActive(true);
                        }
                        else
                        {
                            pinUI[i].SetActive(false);
                        }
                    }
                }
                else if (pin >= 25)
                {
                    for (int i = 0; i < pinUI.Length; i++)
                    {
                        if (pinUI[i] == pinUI[2])
                        {
                            pinUI[i].SetActive(true);
                        }
                        else
                        {
                            pinUI[i].SetActive(false);
                        }
                    }
                }
                else if (pin > 0)
                {
                    for (int i = 0; i < pinUI.Length; i++)
                    {
                        if (pinUI[i] == pinUI[3])
                        {
                            pinUI[i].SetActive(true);
                        }
                        else
                        {
                            pinUI[i].SetActive(false);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < pinUI.Length; i++)
                    {
                        if (pinUI[i] == pinUI[4])
                        {
                            pinUI[i].SetActive(true);
                        }
                        else
                        {
                            pinUI[i].SetActive(false);
                        }
                    }
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
    public void EndGame()
    {
        endTimerImage.SetActive(true);
        endTimerText.gameObject.SetActive(true);
        endTimerText.text = "Thời gian của bạn đã hết!";
        StartCoroutine(TransitionSceneAfterDelay(3f));
    }
    public void DedGame()
    {
        endTimerImage.SetActive(true);
        endTimerText.gameObject.SetActive(true);
        endTimerText.text = "bạn đac chết";
        StartCoroutine(TransitionSceneAfterDelay(3f));
    }
    public void WinGame()
    {
        endTimerImage.SetActive(true);
        endTimerText.gameObject.SetActive(true);
        endTimerText.text = "Bạn đã thoát được nhưng \n Mọi thứ vẫn chưa kết thúc....";
        StartCoroutine(TransitionSceneAfterDelay(3f));
    }
    private IEnumerator TransitionSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(loadsceen);
    }

    private IEnumerator PlayAudioLoop()
    {
        while (isAudioLooping)
        {
            Audiotime.Play();
            yield return new WaitForSeconds(1f);
        }
    }
}