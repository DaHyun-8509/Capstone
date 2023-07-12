using Mono.Cecil.Cil;
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
        Move,
        Act
    }
    enum Location
    {
        Home,
        Work,
        Bench
    }

    public Transform homePos;
    public Transform benchPos;
    public Transform[] workPoses;

    Animator anim;
    NavMeshAgent agent;
    State state = State.None;
    Location location = Location.Home;

    bool finishedAct = false;
    public int targetPlayCount = 3;

private void Start()
    {   
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (agent == null) return;

        //아무것도 안하고있고 1시이면 
        if(state == State.None && Managers.Time.GetHour() == 1)
        {
            //이동한다. 
            state = State.Move;
            anim.SetTrigger("walk");
            agent.destination = workPoses[0].position;
            location = Location.Work;
        }

        //Move 상태이고 목적지에 도달했으면
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && state==State.Move)
        {
            state = State.Act;
            anim.SetTrigger("stop");

            switch (location)
            {
                case Location.Home:
                    break;
                case Location.Work:
                    DoWork();
                    break;
                case Location.Bench:
                    SitOnBench();
                    break;

            }
        }

        //Act 상태이고 행동이 끝났으면 
        if(state == State.Act && finishedAct == true)
        {
            agent.destination = workPoses[1].position;
            location = Location.Work;
            state = State.Move;
            anim.SetTrigger("walk");
            //목적지를 랜덤하게 선택해 이동한다. 
            //Move 상태로 바꾸고 위치를 지정한다
        }
    }

    void DoWork()
    {
        StartCoroutine(PlayWorkAimCoroutime());
    }


    void SitOnBench()
    {
        //의자에서 앞방향을 바라본다. 
        //앉는 애니메이션
    }


    private IEnumerator PlayWorkAimCoroutime()
    {
        anim.SetTrigger("pull_plant");
        yield return new WaitForSeconds(15f);
        anim.SetTrigger("stop");
        state = State.Move;
        anim.SetTrigger("walk");
    }
}
