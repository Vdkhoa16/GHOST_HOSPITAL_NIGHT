using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    public AudioSource Scream;
    public GameObject JumpCam;
    public GameObject FlashImg;
    private bool hasTriggered = false;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            hasTriggered = true;
            Scream.Play();
            JumpCam.SetActive(true);
            FlashImg.SetActive(true);
            StartCoroutine(EndJump());
        }
   

    }
    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(2.03f);
        JumpCam.SetActive(false);
        FlashImg.SetActive(false);
    }
}
