using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBarIM;
    public float CurrentHeath;
    private float MaxHealth = 100f;
    AttributesManager player;





    void Start()
    {
        healthBarIM = GetComponent<Image>();


    }



    void Update()
    {
        CurrentHeath = player.health;
        healthBarIM.fillAmount = CurrentHeath / MaxHealth;



    }
}
