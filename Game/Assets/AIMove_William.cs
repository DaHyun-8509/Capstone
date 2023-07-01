using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove_William : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform farm;
    bool onFarm = false;
    Animator anim;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = farm.position;
        anim = GetComponent<Animator>();
        anim.SetFloat("move_speed", 2);
    }

    private void Update()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.SetDestination(transform.position);
            onFarm = true;
        }

        // 상태에 따라 애니메이션 전환
        if (onFarm == true)
        {
            // 목적지에 도달한 경우 Idle 상태로 전환
            anim.SetTrigger("pick_fruit");
            anim.SetFloat("move_speed", 0);
        }
    }


}