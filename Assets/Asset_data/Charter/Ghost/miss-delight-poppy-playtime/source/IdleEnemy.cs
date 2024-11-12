using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class IdleEnemy : StateMachineBehaviour
{
    float timer;
    List<Transform> players = new List<Transform>();


    //sound
    private Sound playsound;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isChasing", false);
        timer = 0;
        TryFindPlayers();

        //sound
        playsound = animator.GetComponent<Sound>();
        playsound.StopSound();

    }

    void TryFindPlayers()
    {
        players.Clear();
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            players.Add(playerObject.transform);
        }
    }

    Transform GetClosestPlayer(Animator animator)
    {
        Transform closestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(player.position, animator.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (players.Count == 0)
        {
            TryFindPlayers();
        }

        if (timer > 5)
        {
            animator.SetBool("isPatrolling", true);
            //ebug.Log("walk");
        }

        Transform closestPlayer = GetClosestPlayer(animator);
        if (closestPlayer != null)
        {
            float distance = Vector3.Distance(closestPlayer.position, animator.transform.position);
            if (distance <= 10)
            {
                animator.SetBool("isChasing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
