using Invector.vCharacterController;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class AttributesManager : NetworkBehaviour
{
    public bool isTesting = false;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    public int health;
    public int live;
    public float walk_Speed = 2f;
    public float running_Speed = 4f;
    public float sprint_Speed = 6f;
    public int textlive;

    // Replace Transform with float variables for x, y, and z coordinates
    public float respawnX;
    public float respawnY;
    public float respawnZ;


    public GameObject imageOnLiveDecrease; // First image to show on live decrease
    public GameObject imageOnAttack; // Second image to show on attack
    public GameObject Textlive;

    private int maxHealth = 5;
    private float initialWalkSpeed;
    private float initialRunningSpeed;
    private float initialSprintSpeed;

    [Header("FlastLight")]
    public float maxPin = 60f;
    public float currentPin;

    private vThirdPersonController vThirdPersonController;

    void Start()
    {
        currentPin = maxPin;

        health = maxHealth;
        live = 1;
        textlive = 1;
        initialWalkSpeed = walk_Speed;
        initialRunningSpeed = running_Speed;
        initialSprintSpeed = sprint_Speed;

        vThirdPersonController = GetComponent<vThirdPersonController>();

        imageOnLiveDecrease.SetActive(false);
        imageOnAttack.SetActive(false);
    }

    public void Atacking()
    {
        health -= 1;
        sprint_Speed -= 1f;
        walk_Speed -= 0.2f;
        running_Speed -= 0.5f;

        if (health <= 1)
        {
            live += 1;
            textlive += 1;
            ResetAttributes();
            if (live <= 5)
            {
                ResetPositionAndShowImage();
            }
            else
            {
                Debug.Log("end");
            }
        }
        else
        {
            ShowAttackImage();
        }
    }

    private void ResetPositionAndShowImage()
    {
        // Set player position to the specified x, y, z coordinates
        transform.position = new Vector3(respawnX, respawnY, respawnZ);
        imageOnLiveDecrease.SetActive(true);
        Textlive.SetActive(true);
        StartCoroutine(HideImageAfterDelay(imageOnLiveDecrease, 1f)) ; // Hide after 1 second
        StartCoroutine(HideImageAfterDelay(Textlive, 1f));
    }

    private void ShowAttackImage()
    {
        imageOnAttack.SetActive(true);
        StartCoroutine(HideImageAfterDelay(imageOnAttack, 3f)); // Hide after 3 seconds
    }

    private IEnumerator HideImageAfterDelay(GameObject image, float delay)
    {
        yield return new WaitForSeconds(delay);
        image.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dameE"))
        {
            Atacking();
        }
    }

    private void ResetAttributes()
    {
        health = maxHealth;
        walk_Speed = initialWalkSpeed;
        running_Speed = initialRunningSpeed;
        sprint_Speed = initialSprintSpeed;
    }

    void Update()
    {
        if (isTesting)
        {
            Atacking();
            isTesting = false;
        }
        vThirdPersonController.UpdateSpeed(walk_Speed, running_Speed, sprint_Speed);
    }
}
