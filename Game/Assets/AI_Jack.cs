using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class AI_Jack : MonoBehaviour
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
        Chop,
        Pan,
        Counter
    }

    //Transforms
    public Transform homePos;
    public Transform counterPos;
    public Transform counterForwardPos;
    public Transform chopPos;
    public Transform chopForwardPos;
    public Transform panPos;
    public Transform panForwardPos;

    //Times
    public int TimeToGoToWork;
    public int TimeToGoHome;

    //Items
    public GameObject knife;
    public GameObject pan;

    Animator anim;
    NavMeshAgent agent;
    NPCDialog dialog;

    [SerializeField]
    private State state = State.None;
    [SerializeField]
    private Location location = Location.Home;

    bool isTalking = false;

    bool finishedAct = true;


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

        if (state == State.None && Managers.Time.GetHour() == TimeToGoToWork)
        {
            //이동한다. 
            agent.destination = counterPos.position;
            Move();
            location = Location.Counter;
        }
        else if(state == State.None && TimeToGoToWork < Managers.Time.GetHour() && Managers.Time.GetHour() < TimeToGoHome )
        {
            if (finishedAct)
            {
                finishedAct = false;
                StopAllCoroutines();
                int rand = Random.Range(0, 3);
                Move();
                switch (rand)
                {
                    case 0:
                        agent.destination = counterPos.position;
                        location = Location.Counter;
                        break;
                    case 1:
                        agent.destination = chopPos.position;
                        location = Location.Chop;
                        break;
                    case 2:
                        agent.destination = panPos.position;
                        location = Location.Pan;
                        break;
                    default:
                        break;
                }
                
            }
        }
        else if (state == State.None && Managers.Time.GetHour() == TimeToGoHome)
        {
            //이동한다. 
            agent.destination = homePos.position;
            Move();
            location = Location.Home;
        }


        //Move 상태이고 목적지에 도달했으면
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && state == State.Move)
        {
            state = State.Act;
            anim.SetTrigger("stop");
            StopAllCoroutines();

            switch (location)
            {
                case Location.Home:
                    break;
                case Location.Counter:
                    StartCoroutine(FinishCounterActCoroutine());
                    break;
                case Location.Pan:
                    StartCoroutine(PanActCoroutine());
                    break;
                case Location.Chop:
                    StartCoroutine(ChopActCoroutine());
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
            if (state == State.Move)
                anim.SetTrigger("walk");
        }
    }
    void Move()
    {
        state = State.Move;
        anim.SetTrigger("walk");
    }

    private IEnumerator PanActCoroutine()
    {
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("pan");
        transform.LookAt(panForwardPos);
        yield return new WaitForSeconds(15f);
        finishedAct = true;
        state = State.None;
    }

    private IEnumerator ChopActCoroutine()
    {
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("chop");
        transform.LookAt(chopForwardPos);
        yield return new WaitForSeconds(15f);
        finishedAct = true;
        state = State.None;
    }

    private IEnumerator FinishCounterActCoroutine()
    {
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("stop");
        transform.LookAt(counterForwardPos);
        yield return new WaitForSeconds(20f);
        finishedAct = true;
        state = State.None;
    }
}
