using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Ann : MonoBehaviour
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
        FrontHome
    }

    //Transforms
    public Transform homePos;
    public Transform frontHomePos;
    public Transform frontHomeForwardPos;

    //Times
    public int TimeToGoFrontHome;

    public int TimeToGoHome;

    Animator anim;
    NavMeshAgent agent;
    NPCDialog dialog;

    [SerializeField]
    State state = State.None;
    [SerializeField]
    Location location = Location.Home;

    bool isTalking = false;

    float agentAccel;

    ChatGPT gpt;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        dialog = GetComponentInChildren<NPCDialog>();

        agentAccel = agent.acceleration;
        gpt = gameObject.transform.Find("ToActivate").GetComponentInChildren<ChatGPT>();

    }

    private void Update()
    {
        if (agent == null) return;
        anim.SetFloat("speed", agent.velocity.magnitude);

        //�ƹ��͵� ���ϰ��ְ� TimeToGoFrontHome���̸�  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoFrontHome)
        {
            //�������ڷ� �̵��Ѵ�. 
            agent.destination = frontHomePos.position;
            state = State.Move;
            gpt.nowState = "";
            location = Location.FrontHome;
        }
        //�������� �ʰ��ְ� TimeToGoHome���̸�  
        if (state != State.Move && Managers.Time.GetHour() == TimeToGoHome)
        {
            //������ �̵��Ѵ�. 
            anim.SetTrigger("stop");
            agent.destination = homePos.position;
            state = State.Move;
            location = Location.Home;
        }


        //Move �����̰� �������� ����������
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && state == State.Move)
        {
            state = State.None;
            switch (location)
            {
                case Location.Home:
                    break;
                case Location.FrontHome:
                    gpt.nowState = "sitting and feeling sunshine and nature";
                    OnFrontHome();
                    break;
                default:
                    break;
            }
        }

        //�÷��̾ ��ȭ�� �ɾ��� �� 
        if (dialog.Talking == true && isTalking == false)
        {
            agent.acceleration = 0;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            isTalking = true;
        }
        //��ȭ�� ������ ��
        if (dialog.Talking == false && isTalking == true)
        {
            agent.acceleration = agentAccel;
            agent.isStopped = false;

            isTalking = false;
            if (state != State.Move && location == Location.FrontHome)
                OnFrontHome();
        }
    }

    void OnFrontHome()
    {
        transform.LookAt(frontHomeForwardPos);
        anim.SetTrigger("sit");
    }

}
