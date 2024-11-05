using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HideShow : MonoBehaviour
{
    public BoxTriger[] boxTriger;
    public bool check;
    public CheatBoxHaS cheatBox;
    public TextMeshPro passR;
    public SafeController safeController;
    // Start is called before the first frame update
    void Start()
    {
        RandomPass();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < boxTriger.Length; i++)
        {
            if (boxTriger[i].CheckOnTriger() == true)
            {
                LoseGame();
            }
        }
       
  
    }

    public void LoseGame()
    {
        for(int i = 0;i < boxTriger.Length; i++)
        {
            boxTriger[i].gameObject.SetActive(false);
            boxTriger[i].isTriger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cheatBox.cheat == false)
        {
            if (other.CompareTag("Player"))
            {
                RandomPass();
                for (int i = 0; i < boxTriger.Length; i++)
                {
                    boxTriger[i].setActive();
                    check = true;
                }
            }
        }
        
    }

    public void RandomPass()
    {
        int pass;
        pass = Random.Range(1000, 9999);
        passR.text = pass.ToString();
        safeController.keyID = pass ;
    }
}
