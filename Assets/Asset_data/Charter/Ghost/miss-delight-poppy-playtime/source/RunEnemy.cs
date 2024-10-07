using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;


public class RunEnemy : StateMachineBehaviour
{
    NavMeshAgent agent;
    List<Transform> players = new List<Transform>();
    float ChaseRange = 15;
    float AttackRange = 2f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.SetBool("isChasing", false);
        agent = animator.GetComponent<NavMeshAgent>();
        // player = GameObject.FindGameObjectWithTag("Player").transform;
        TryFindPlayers();
        agent.speed = 6f;
        //Debug.Log("Entered Chase State");

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
            if (distance > ChaseRange)
            {
                //Debug.Log("Player is within chase range. Setting isChasing to false.");
                animator.SetBool("isChasing", false);
            }
            if (distance < AttackRange)
            {
                //Debug.Log("Player is within chase range. Setting isAttacking to true.");
                animator.SetBool("isAttacking", true);
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
        //Debug.Log("Exiting Chase State");
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
