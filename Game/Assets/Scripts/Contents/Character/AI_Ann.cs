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

        //아무것도 안하고있고 TimeToGoFrontHome시이면  
        if (state == State.None && Managers.Time.GetHour() == TimeToGoFrontHome)
        {
            //집앞의자로 이동한다. 
            agent.destination = frontHomePos.position;
            state = State.Move;
            gpt.nowState = "";
            location = Location.FrontHome;
        }
        //움직이지 않고있고 TimeToGoHome시이면  
        if (state != State.Move && Managers.Time.GetHour() == TimeToGoHome)
        {
            //집으로 이동한다. 
            anim.SetTrigger("stop");
            agent.destination = homePos.position;
            state = State.Move;
            location = Location.Home;
        }


        //Move 상태이고 목적지에 도달했으면
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

        //플레이어가 대화를 걸었을 때 
        if (dialog.Talking == true && isTalking == false)
        {
            agent.acceleration = 0;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            isTalking = true;
        }
        //대화가 끝났을 때
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
