using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pickUpOB;
    public GameObject player;
    public GameObject pickUpText;
    public GameObject cannotPickUpText;
    public int addHealth = 25;
    private float currentHealth;

    public AudioSource healthPickUpSound;

    public GameObject screenFX;

    public bool inReach;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inReach = true;
            pickUpText.SetActive(true);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inReach = false;
            pickUpText.SetActive(false);
            cannotPickUpText.SetActive(false);
        }
    }

    void Start()
    {
        currentHealth = player.GetComponent<AttributesManager>().health;
        cannotPickUpText.SetActive(false);
        pickUpText.SetActive(false);

        screenFX.SetActive(false);

        inReach = false;
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E) && player.GetComponent<AttributesManager>().health < 100)
        {
            inReach = false;
            healthPickUpSound.Play();
            player.GetComponent<AttributesManager>().health += addHealth;
            screenFX.SetActive(true);
            pickUpOB.GetComponent<BoxCollider>().enabled = false;
            pickUpOB.GetComponent<MeshRenderer>().enabled = false;
            pickUpText.SetActive(false);
            StartCoroutine(TurnScreenFXOFF());
        }

        else if (inReach && Input.GetKeyDown(KeyCode.E) && player.GetComponent<AttributesManager>().health == 100)
        {
            pickUpText.SetActive(false);
            cannotPickUpText.SetActive(true);
        }

    }

    IEnumerator TurnScreenFXOFF()
    {
        yield return new WaitForSeconds(1.25f);
        screenFX.SetActive(false);
        pickUpOB.SetActive(false);
    }
}
