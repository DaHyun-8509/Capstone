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

    //Transforms
    public Transform homePos;
    public Transform benchPos;
    public Transform[] workPoses;

    //Times
    public int TimeToGoToWork;
    public int TimeToGoBackHome;

    Animator anim;
    NavMeshAgent agent;
    State state = State.None;
    Location location = Location.Home;

    bool finishedAct = false;
    int nowIndex = 0;

private void Start()
    {   
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (agent == null) return;

        //아무것도 안하고있고 TimeToGoToWork시이면 
        if (state == State.None && Managers.Time.GetHour() == TimeToGoToWork)
        {
            //이동한다. 
            agent.destination = workPoses[nowIndex].position;
            MoveToWork();
        }

        //일이 끝났고 TimeToGoBackHome시 이후면
        if (state == State.Act && finishedAct && Managers.Time.GetHour() >= TimeToGoBackHome)
        {
            agent.destination = homePos.position;
            state = State.None;
            anim.SetTrigger("walk");
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

    void SitOnBench()
    {
        //의자에서 앞방향을 바라본다. 
        //앉는 애니메이션
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
