using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShow : MonoBehaviour
{
    public BoxTriger[] boxTriger;
    public bool check;
    public CheatBoxHaS cheatBox;
    // Start is called before the first frame update
    void Start()
    {
        cheatBox = GetComponent<CheatBoxHaS>();
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
                for (int i = 0; i < boxTriger.Length; i++)
                {
                    boxTriger[i].setActive();
                    check = true;
                }
            }
        }
        
    }
}
