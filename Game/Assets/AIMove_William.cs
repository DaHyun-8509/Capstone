using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using Unity.VisualScripting;

public class AIMove_William : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform bench;
    public Transform home;
    public Transform restuarant;

    public Transform[] workplace;
    Animator anim;

    [SerializeField]
    William_Location location = William_Location.Home;
    bool ismoving = false;

    enum William_Location
    {
        Home,
        Workplace,
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
                case 7:
                    {
                        MoveToLocation(workplace[0].position);
                        location = William_Location.Workplace;
                    }
                    break;
                case 8:
                    {
                        MoveToLocation(workplace[1].position);
                        location = William_Location.Workplace;
                    }
                    break;
                case 9:
                    {
                        MoveToLocation(workplace[2].position);
                        location = William_Location.Workplace;
                    }
                    break;
                case 10:
                    {
                        MoveToLocation(workplace[3].position);
                        location = William_Location.Workplace;
                    }
                    break;
                case 12:
                    {
                        MoveToLocation(bench.position);
                        location = William_Location.Bench;
                    }
                    break;
                case 15:
                    {
                        MoveToLocation(workplace[0].position);
                        location = William_Location.Workplace;
                    }
                    break;

                case 18:
                    {
                        MoveToLocation(restuarant.position);
                        location = William_Location.Restaurant;
                    }
                    break;
                case 21:
                    { 
                        MoveToLocation(home.position);
                        location = William_Location.Home;
                    }
                    break;
                default:
                    break;
            }
        }

        if (ismoving && navMeshAgent.hasPath && navMeshAgent.remainingDistance  <= navMeshAgent.stoppingDistance)
        {
            ismoving = false;
           
            switch (location)
            {
                case William_Location.Home:
                    {
                    }
                    break;
                case William_Location.Bench:
                    {
                        SitOnTheBench();
                    }
                    break;
                case William_Location.Workplace:
                    {
                        InvokeRepeating("Work", 0f, 2f);
                    }
                    break;
                case William_Location.Restaurant:
                    {
                        SitInRestaurant();
                    }
                    break;
                default:
                    break;
            }
        }  
    }

    private void MoveToLocation(Vector3 location )
    {
        CancelInvoke("Work");
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

    private void SitInRestaurant()
    {
        //Quaternion targetRotation = Quaternion.LookRotation(-Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 200, 0);
        anim.SetFloat("move_speed", 0);
        anim.SetTrigger("sit");
    }

}