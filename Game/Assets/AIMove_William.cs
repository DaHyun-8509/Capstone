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

        // ���¿� ���� �ִϸ��̼� ��ȯ
        if (onFarm == true)
        {
            // �������� ������ ��� Idle ���·� ��ȯ
            anim.SetTrigger("pick_fruit");
            anim.SetFloat("move_speed", 0);
        }
    }


}