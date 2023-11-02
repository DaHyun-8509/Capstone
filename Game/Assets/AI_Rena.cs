using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Rena : MonoBehaviour
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
        Swing
    }

    //Transforms
    public Transform homePos;
    public Transform swingPos;
    public Transform swingForwardPos;

    //Times
    public int TimeToGoToSwing;
    public int TimeToGoHome;


    Animator anim;
    NavMeshAgent agent;
    NPCDialog dialog;

    [SerializeField]
    private State state = State.None;
    [SerializeField]
    private Location location = Location.Home;

    bool isTalking = false;

    int hour;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        dialog = GetComponentInChildren<NPCDialog>();
    }

    private void Update()
    {
        if (agent == null) return;

        anim.SetFloat("speed", agent.velocity.magnitude);

        hour = Managers.Time.GetHour();

        if(!isTalking && state != State.Move && location == Location.Swing)
        {
            transform.LookAt(swingForwardPos);
        }

        if (state == State.None && hour == TimeToGoToSwing)
        {
            //�̵��Ѵ�. 
            agent.destination = swingPos.position;
            state = State.Move;
            location = Location.Swing;
        }
        else if (state == State.None && hour == TimeToGoHome)
        {
            //�̵��Ѵ�. 
            anim.SetTrigger("stop");
            agent.destination = homePos.position;
            state = State.Move;
            location = Location.Home;
        }
        //Move �����̰� �������� ����������
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && state == State.Move)
        {
            state = State.Act;
            StartCoroutine(WaitAndSetStateNone());

            switch (location)
            {
                case Location.Home:
                    break;
                case Location.Swing:
                    OnSwing();
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
            isTalking = true;
        }
        //��ȭ�� ������ ��
        if (dialog.Talking == false && isTalking == true)
        {
            agent.isStopped = false;
            isTalking = false;
            if(location == Location.Swing && state != State.Move)
            {
                OnSwing();
            }
        }
    }

    void OnSwing()
    {
        transform.LookAt(swingForwardPos);
        anim.SetTrigger("sit");
    }

    private IEnumerator WaitAndSetStateNone()
    {
        yield return new WaitForSeconds(Managers.Time.GetOneHourTime());

        state = State.None;
    }


}
