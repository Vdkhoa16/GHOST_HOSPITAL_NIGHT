using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;


public class RunEnemy : StateMachineBehaviour
{
    NavMeshAgent agent;
    List<Transform> players = new List<Transform>();
    float ChaseRange = 10;
    float JumpRange = 4f;
    //sound
    private Audio_dir playsound;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        TryFindPlayers();
        agent.speed = 5f;
        //sound
        playsound = animator.GetComponent<Audio_dir>();
        playsound.Playaudio();

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

    Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(player.position, agent.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (players.Count == 0)
        {
            TryFindPlayers();
        }
        Transform closestPlayer = GetClosestPlayer();
        agent.SetDestination(closestPlayer.position);

        //animator.SetBool("isChasing", true);

        if (closestPlayer != null)
        {
            float distance = Vector3.Distance(closestPlayer.position, animator.transform.position);
            if (distance >= ChaseRange)
            {
                //Debug.Log("Player is within chase range. Setting isChasing to false.");
                animator.SetBool("isChasing", false);
            }
            if (distance <= JumpRange)
            {
                //Debug.Log("Player is within chase range. Setting isAttacking to true.");
                animator.SetBool("isJump", true);

                //animator.SetBool("isChasing", false);
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
        // Stop sound and reset sound state
        //if (isSoundPlaying && playsound != null)
        //{
        //    playsound.StopSound();
        //    isSoundPlaying = false;
        //}
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
