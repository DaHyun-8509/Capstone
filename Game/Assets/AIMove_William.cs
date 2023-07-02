using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using TMPro;


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

        if (onFarm == true)
        {
            //anim.SetTrigger("pick_fruit");
            Vector3 targetDirection = new Vector3(0f, 0f, 1f); // z¹æÇâ º¤ÅÍ
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10* Time.deltaTime);
            anim.SetFloat("move_speed", 0);
            anim.SetTrigger("sit");
        }

    
    }
}