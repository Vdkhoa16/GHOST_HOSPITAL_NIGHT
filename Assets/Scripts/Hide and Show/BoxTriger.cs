using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxTriger : MonoBehaviour
{
    public bool isTriger = false;
    public SafeController safeController;
    public Transform transformRandom;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        transformRandom.position = new Vector3(transform.position.x, transform.position.y, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        RandomTransform();
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
            RandomPass();
            RandomTransform();
        }
    }

    public void RandomPass()
    {
        int pass;
        pass = Random.Range(1000, 9999);
        safeController.keyID = pass;
    }
    public void RandomTransform()
    {
        
        float randomZ = Random.Range(0.0f, 1.0f);
        


    }
}
