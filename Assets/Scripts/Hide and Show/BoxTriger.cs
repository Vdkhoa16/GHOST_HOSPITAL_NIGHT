using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTriger : MonoBehaviour
{
    public bool isTriger = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive()
    {
        gameObject.SetActive(true);
    }


    public bool CheckOnTriger()
    {
       
        return isTriger;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriger = true;
            //Destroy(gameObject);
        }
    }
}
