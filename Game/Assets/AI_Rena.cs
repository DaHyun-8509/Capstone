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
            //이동한다. 
            agent.destination = swingPos.position;
            state = State.Move;
            location = Location.Swing;
        }
        else if (state == State.None && hour == TimeToGoHome)
        {
            //이동한다. 
            anim.SetTrigger("stop");
            agent.destination = homePos.position;
            state = State.Move;
            location = Location.Home;
        }
        //Move 상태이고 목적지에 도달했으면
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

        //플레이어가 대화를 걸었을 때 
        if (dialog.Talking == true && isTalking == false)
        {
            agent.isStopped = true;
            anim.SetTrigger("stop");
            isTalking = true;
        }
        //대화가 끝났을 때
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
