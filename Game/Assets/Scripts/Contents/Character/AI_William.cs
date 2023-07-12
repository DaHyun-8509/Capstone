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

        //�ƹ��͵� ���ϰ��ְ� 1���̸� 
        if(state == State.None && Managers.Time.GetHour() == 1)
        {
            //�̵��Ѵ�. 
            state = State.Move;
            anim.SetTrigger("walk");
            agent.destination = workPoses[0].position;
            location = Location.Work;
        }

        //Move �����̰� �������� ����������
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

        //Act �����̰� �ൿ�� �������� 
        if(state == State.Act && finishedAct == true)
        {
            agent.destination = workPoses[1].position;
            location = Location.Work;
            state = State.Move;
            anim.SetTrigger("walk");
            //�������� �����ϰ� ������ �̵��Ѵ�. 
            //Move ���·� �ٲٰ� ��ġ�� �����Ѵ�
        }
    }

    void DoWork()
    {
        StartCoroutine(PlayWorkAimCoroutime());
    }


    void SitOnBench()
    {
        //���ڿ��� �չ����� �ٶ󺻴�. 
        //�ɴ� �ִϸ��̼�
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
