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

        //�ƹ��͵� ���ϰ��ְ� TimeToGoRestaurant���̸�  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoRestaurant)
        {
            //����������� �̵��Ѵ�. 
            agent.destination = restaurantPos.position;
            Move();
            location = Location.Restaurant;
        }


        //�ƹ��͵� ���ϰ��ְ� TimeToGoYard���̸�  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoYard)
        {
            //���� �������� �̵��Ѵ�. 
            agent.destination = yardPos.position;
            Move();
            location = Location.Yard;
        }


        //�ƹ��͵� ���ϰ��ְ� TimeToGoHome1���̸�  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoHome1)
        {
            //������ �̵��Ѵ�. 
            agent.destination = homePos.position;
            Move();
            location = Location.Restaurant;
        }


        //�ƹ��͵� ���ϰ��ְ� TimeToGoForaWalk���̸�  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoForaWalk)
        {
            //��å��ҷ� �̵��Ѵ�. 
            agent.destination = walkPos.position;
            Move();
            location = Location.OnAWalk;
        }

        //�ƹ��͵� ���ϰ��ְ� TimeToGoHome2���̸�  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoHome2)
        {
            //������ �̵��Ѵ�. 
            agent.destination = homePos.position;
            Move();
            location = Location.Home;
        }


        //Move �����̰� �������� ����������
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
        finishedAct = false;
        StopAllCoroutines();
    }


}
