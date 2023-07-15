using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Kinki : MonoBehaviour
{
    enum State
    {
        None,
        Move
    }
    enum Location
    {
        Home,
        Yard,
        Restaurant,
        OnAWalk
    }

    //Transforms
    public Transform homePos;
    public Transform restaurantPos;
    public Transform yardPos;
    public Transform walkPos;

    //Times
    public int TimeToGoRestaurant;
    public int TimeToGoYard;
    public int TimeToGoHome1;
    public int TimeToGoForaWalk;
    public int TimeToGoHome2;


    Animator anim;
    NavMeshAgent agent;
    NPCDialog dialog;

    State state = State.None;
    Location location = Location.Home;

    bool finishedAct = false;
    bool isTalking = false;
    int nowIndex = 0;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        dialog = GetComponentInChildren<NPCDialog>();
    }

    private void Update()
    {
        if (agent == null) return;

        //아무것도 안하고있고 TimeToGoRestaurant시이면  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoRestaurant)
        {
            //레스토랑으로 이동한다. 
            agent.destination = restaurantPos.position;
            Move();
            location = Location.Restaurant;
        }


        //아무것도 안하고있고 TimeToGoYard시이면  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoYard)
        {
            //집앞 마당으로 이동한다. 
            agent.destination = yardPos.position;
            Move();
            location = Location.Yard;
        }


        //아무것도 안하고있고 TimeToGoHome1시이면  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoHome1)
        {
            //집으로 이동한다. 
            agent.destination = homePos.position;
            Move();
            location = Location.Restaurant;
        }


        //아무것도 안하고있고 TimeToGoForaWalk시이면  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoForaWalk)
        {
            //산책장소로 이동한다. 
            agent.destination = walkPos.position;
            Move();
            location = Location.OnAWalk;
        }

        //아무것도 안하고있고 TimeToGoHome2시이면  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoHome2)
        {
            //집으로 이동한다. 
            agent.destination = homePos.position;
            Move();
            location = Location.Home;
        }


        //Move 상태이고 목적지에 도달했으면
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && state == State.Move)
        {
            state = State.None;
            anim.SetTrigger("stop");

            switch (location)
            {
                case Location.Home:
                    break;
                case Location.Restaurant:
                    break;
                case Location.OnAWalk:
                    break;
                case Location.Yard:
                    break;
                default:
                    break;
            }
        }

       

        //플레이어가 대화를 걸었을 때 
        if (dialog.Talking == true && isTalking == false)
        {
            agent.isStopped = true;
            anim.SetTrigger("stop");
            transform.LookAt(dialog.Avatar);
            isTalking = true;
        }
        //대화가 끝났을 때
        if (dialog.Talking == false && isTalking == true)
        {
            agent.isStopped = false;
            isTalking = false;
            if (state == State.Move)
                anim.SetTrigger("walk");
        }


    }
    void Move()
    {
        state = State.Move;
        anim.SetTrigger("walk");
        finishedAct = false;
        StopAllCoroutines();
    }


}
