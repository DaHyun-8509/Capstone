using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Lewis : MonoBehaviour
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
        FrontHome
    }

    //Transforms
    public Transform homePos;
    public Transform restaurantPos;
    public Transform restaurantForwardPos;
    public Transform frontHomePos;
    public Transform frontHomeForwardPos;

    //Times
    public int TimeToGoToRestaurant;
    public int TimeToGoFrontHome;
    public int TimeToGoHome;

    //Items
    public GameObject food;

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

        food.SetActive(false);
    }

    private void Update()
    {
        if (agent == null) return;

        anim.SetFloat("speed", agent.velocity.magnitude);

        hour = Managers.Time.GetHour();

        if (!isTalking && state != State.Move && location == Location.Restaurant)
        {
            transform.LookAt(restaurantForwardPos);
        }

        if (state == State.None && hour == TimeToGoToRestaurant)
        {
            //이동한다. 
            agent.destination = restaurantPos.position;
            state = State.Move;
            location = Location.Restaurant;
        }
        else if (state == State.None && hour == TimeToGoHome)
        {
            //이동한다. 
            anim.SetTrigger("stop");
            food.SetActive(false);
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
                case Location.Restaurant:
                    OnRestaurant();
                    break;
                case Location.FrontHome:
                    OnFrontHome();
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
            if (location == Location.Restaurant && state != State.Move)
            {
                OnRestaurant();
            }
        }
    }

    void OnRestaurant()
    {
        transform.LookAt(restaurantForwardPos);
        anim.SetTrigger("sit");
        food.SetActive(true);
    }

    void OnFrontHome()
    {
        transform.LookAt(frontHomeForwardPos);
    }

    private IEnumerator WaitAndSetStateNone()
    {
        yield return new WaitForSeconds(Managers.Time.GetOneHourTime());

        state = State.None;
    }

}
