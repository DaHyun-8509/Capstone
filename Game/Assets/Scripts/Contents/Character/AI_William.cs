using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class AI_William : MonoBehaviour
{
    enum State
    {
        None,
        Idle,
        Move,
        Work,
        Sit
    }
    enum Location
    {
        Home,
        Work1,
        Work2,
        Work3,
        Bench,
        Restaurant
    }

    public Transform homePos;
    public Transform workPos;
    public Transform workPos2;
    public Transform workPos3;
    public Transform benchPos;
    public Transform restaurantPos;

    Animator anim;
    NavMeshAgent agent;
    State state = State.Idle;
    Location location = Location.Home;

private void Start()
    {   
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        int time = Managers.Time.GetHour();
        if (state != State.Move)
        {
            switch (time)
            {
                case 7: //일터로 이동
                    MovetoDestination(workPos.position);
                    location = Location.Work1;
                    break;
                case 9: //일터로 이동
                    MovetoDestination(workPos2.position);
                    location = Location.Work2;
                    break;
                case 12: //벤치로 이동
                    MovetoDestination(benchPos.position);
                    location = Location.Bench;
                    break;
                case 15://일터로 이동
                    MovetoDestination(workPos3.position);
                    location = Location.Work3;
                    break;
                case 17: //식당으로 이동
                    MovetoDestination(restaurantPos.position);
                    location = Location.Restaurant;
                    break;
                case 19: //집으로 이동
                    MovetoDestination(homePos.position);
                    location = Location.Home;
                    break;
                default:
                    break;
            }
        }
  

        //이동하여 도착했다면 
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && state == State.Move )
        {
            anim.SetFloat("move_speed", 0);
            switch (location)
            {
                case Location.Work1:
                    Work();
                    break;
                case Location.Work2:
                    StartToWork();
                    break;
                case Location.Work3:
                    StartToWork();
                    break;
                case Location.Home:
                    break;
                case Location.Bench:
                    SitOnBench();
                    break;
                case Location.Restaurant:
                    break;
            }
        }
    }

    private void MovetoDestination(Vector3 dest)
    {
        
        anim.SetFloat("move_speed", 2.0f);
        agent.destination = dest;
        state = State.Move;
    }
    private void StartToWork()
    {
        InvokeRepeating("Work", 2, 3);
        state = State.Work;
    }
    private void SitOnBench()
    {
        //앉는 애니메이션
        state = State.Sit;
    }

   private void Work()
    {
        anim.SetTrigger("pick_fruit");
        state = State.Work;
    }


}
