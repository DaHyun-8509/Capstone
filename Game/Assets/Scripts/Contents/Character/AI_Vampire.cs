using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Vampire : MonoBehaviour
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
        Bench,
        LackorRiver
    }

    //Transforms
    public Transform homePos;
    public Transform benchPos;
    public Transform lakePos;
    public Transform riverPos;
    public Transform restaurantPos;

    //Times
    public int TimeToGoRestaurant;
    public int TimeToGoBench;
    public int TimeToGoHome;
    public int TimeToGoLakeorRiver;

    Animator anim;
    NavMeshAgent agent;
    NPCDialog dialog;

    State state = State.None;
    Location location = Location.Home;

    bool isTalking = false;
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

        //�ƹ��͵� ���ϰ��ְ� TimeToGoRestaurant���̸�  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoRestaurant)
        {
            //����������� �̵��Ѵ�. 
            agent.destination = restaurantPos.position;
            Move();
            location = Location.Restaurant;
        }

        else if (state == State.None && Managers.Time.GetHour() == TimeToGoBench)
        {
            agent.destination = benchPos.position;
            location = Location.Bench;
            Move();
        }

        else if (state == State.None && Managers.Time.GetHour() == TimeToGoLakeorRiver)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
                agent.destination = lakePos.position;
            else
                agent.destination = riverPos.position;
            Move();
            location = Location.LackorRiver;

        }

        else if (state == State.None && Managers.Time.GetHour() == TimeToGoHome)
        {
            agent.destination = homePos.position;
            Move();
            location = Location.Home;
        }

        //Move �����̰� �������� ����������
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
                case Location.LackorRiver:
                    break;
                case Location.Bench:
                    break;
                default:
                    break;
            }
        }

        //�÷��̾ ��ȭ�� �ɾ��� �� 
        if (dialog.Talking == true && isTalking == false)
        {
            agent.isStopped = true;
            anim.SetTrigger("stop");
            transform.LookAt(dialog.Avatar);
            isTalking = true;
        }
        //��ȭ�� ������ ��
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
}
