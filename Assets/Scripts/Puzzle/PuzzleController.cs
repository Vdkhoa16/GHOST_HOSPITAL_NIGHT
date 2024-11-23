using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PuzzleController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI interactionText;    
    private bool playerInRange = false;
    private GameObject Cameramain;
    private GameObject PuzzleCamera;
    void Start()
    {
        PuzzleCamera = GameObject.FindGameObjectWithTag("PuzzleCamera");
        PuzzleCamera.SetActive(false);
        Cameramain = Camera.main.gameObject;
        // Ensure the puzzle panel and interaction text are initially hidden
        if (interactionText)
            interactionText.gameObject.SetActive(false);  // Hide the "Press E" prompt initially
    }

    // When the player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the collider is the player (ensure player is tagged as "Player")
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                playerInRange = true;
                interactionText.gameObject.SetActive(true);  // Show "Press E to solve puzzle"
            }
            else
            {
                playerInRange = false;
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    // When the player leaves the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionText.gameObject.SetActive(false);  // Hide the "Press E to solve puzzle" prompt
        }
    }

    void Update()
    {
        // If the player is in range and presses "E", change scene
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Cameramain.SetActive(false);
            PuzzleCamera.SetActive(true);
            ONMose();
        }
        }

    public void ClosePuzzle()
    {
        Cameramain.SetActive(true);
        PuzzleCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Method to load a new scene
    public void ONMose()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
