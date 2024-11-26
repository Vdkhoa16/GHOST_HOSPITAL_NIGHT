using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManagerRoom : MonoBehaviour
{
    public ShowObjectOnPress[] showObjectOnPresses;
    public GameObject room;
    public bool checkFull = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < showObjectOnPresses.Length; i++)
        {
            if (showObjectOnPresses[0].check && showObjectOnPresses[1].check && showObjectOnPresses[2].check && showObjectOnPresses[3].check)
            {
                checkFull = true;
                break;
            }
        }

        if (checkFull)
        {
            room.SetActive(true);
        }
    }
}
