using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class AI_Cheif : MonoBehaviour
{
    enum State
    {
        None,
        Move,
        Act
    }
    enum Location
    {
        Home,
        Restaurant,
        Market,
        HomeStand,
        Work
    }

    //Transforms
    public Transform homePos;
    public Transform restaurantPos;
    public Transform restaurantForwardPos;
    public Transform marketPos;
    public Transform marketForwardPos;
    public Transform homeStandPos;
    public Transform[] workPoses;

    //Times
    public int TimeToGoToWork;
    public int Type1_TimeToStand;
    public int Type1_TimeToGoToWork2;
    public int Type2_TimeToGoToMarket;
    public int TimeToGoToRestaurant;
    public int TimeToGoHome;

    Animator anim;
    NavMeshAgent agent;
    NPCDialog dialog;

    State state = State.None;
    Location location = Location.Home;

    int type = 1;
    bool randValueSelected = false;
    bool isTalking = false;

    bool finishedAct = false;
    int nowIndex = 0;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        dialog = GetComponentInChildren<NPCDialog>();
        anim.SetTrigger("walk");
    }

    private void Update()
    {
        if (agent == null) return;

        if(randValueSelected == false && Managers.Time.GetHour() == 0)
        {
            type = Random.Range(1, 3);
            randValueSelected = true;
        }

        if (state == State.None && Managers.Time.GetHour() == TimeToGoToWork)
        {
            //이동한다. 
            agent.destination = workPoses[nowIndex].position;
            MoveToWork();
        }

        else if (type == 1 && location != Location.HomeStand &&  state != State.Move && finishedAct && Managers.Time.GetHour() == Type1_TimeToStand)
        {
            
            agent.destination = homeStandPos.position;
            Move();
            location = Location.HomeStand;
        }

        else if (type == 1 && state == State.None && Managers.Time.GetHour() == Type1_TimeToGoToWork2)
        {
            agent.destination = workPoses[nowIndex].position;
            MoveToWork();
        }

        else if (type == 2 && state == State.Act && finishedAct && Managers.Time.GetHour() == Type2_TimeToGoToMarket)
        {
            agent.destination = marketPos.position;
            Move();
            location = Location.Market;
        }

        else if (state == State.None && Managers.Time.GetHour() == TimeToGoHome)
        {
            agent.destination = homePos.position;
            Move();
            location = Location.Home;
        }
        //Act 상태이고 행동이 끝났으면 
        else if (location == Location.Work && state == State.Act && finishedAct == true)
        {
            //목적지를 랜덤하게 선택해 이동한다. 
            int idx;
            while (true)
            {
                idx = Random.Range(0, workPoses.Length);
                if (idx != nowIndex) break;
            }
            nowIndex = idx;

            agent.destination = workPoses[nowIndex].position;

            //Move 상태로 바꾸고 위치를 지정한다
            MoveToWork();
        }


        //Move 상태이고 목적지에 도달했으면
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && state == State.Move)
        {
            state = State.Act;
            StartCoroutine(WaitAndSetStateNone());
            anim.SetTrigger("stop");

            switch (location)
            {
                case Location.Home:
                    break;
                case Location.Restaurant:
                    break;
                case Location.HomeStand:
                    break;
                case Location.Work:
                    DoWork();
                    break;
                case Location.Market:
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
    }

    private IEnumerator WaitAndSetStateNone()
    {
        yield return new WaitForSeconds(Managers.Time.GetOneHourTime());

        state = State.None;
    }

    void MoveToWork()
    {
        state = State.Move;
        location = Location.Work;
        anim.SetTrigger("walk");
        finishedAct = false;
        StopAllCoroutines();
    }

    void DoWork()
    {
        StartCoroutine(PlayWorkAimCoroutine());
    }

    private IEnumerator PlayWorkAimCoroutine()
    {
        anim.SetTrigger("pull_plant");
        //15.5초 후 애니메이션을 멈춘다
        yield return new WaitForSeconds(15.5f);
        finishedAct = true;
        anim.SetTrigger("stop");
    }

}
