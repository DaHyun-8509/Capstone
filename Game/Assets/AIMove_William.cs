using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using TMPro;


public class AIMove_William : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform bench;
    public Transform workplace1;
    Animator anim;
    William_Location location = William_Location.Home;
    bool ismoving = false;
    bool onDestination = false;

    enum William_Location
    {
        Home,
        Workplace1,
        Workplace2,
        Workplace3,
        Bench,
        Restaurant
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!ismoving)
        {
            //정해진 시간에 목적지 할당
            switch (Managers.Time.GetHour())
            {
                case 1:
                    {
                        MoveToLocation(bench.position);
                        location = William_Location.Bench;
                    }
                    break;
                case 3:
                    {
                        MoveToLocation(workplace1.position);
                        location = William_Location.Workplace1;
                    }
                    break;
                case 10:
                    break;
                case 14:
                    break;
                default:
                    break;
            }
        }
        
        //도착했을 때 
        if(ismoving&& navMeshAgent.hasPath && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            ismoving = false;
            onDestination = true;
            switch (location)
            {
                case William_Location.Home:
                    break;
                case William_Location.Bench:
                    SitOnTheBench();
                    break;
                case William_Location.Workplace1:
                    Work();
                    break;
                case William_Location.Workplace2:
                    Work();
                    break;
                default:
                    break;
            }
        }
    }

    private void MoveToLocation(Vector3 location )
    {
        navMeshAgent.destination = location;
        anim.SetFloat("move_speed", 2);
        ismoving = true;
    }

    private void SitOnTheBench()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        anim.SetFloat("move_speed", 0);
        anim.SetTrigger("sit");
    }

    private void Work()
    {
        anim.SetFloat("move_speed", 0);
        anim.SetTrigger("pick_fruit");
    }
}