using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIcontroller : MonoBehaviour
{
    public enum AIState { WaitingInLine, MovingToRoom, Resting, Leaving }
    public AIState currentState;

    public Transform room;
    public Transform exitPoint;
    public Transform waitingPosition;

    [SerializeField]private HotelTableScript hotelTable;
    [SerializeField] private Animator animator;
    [SerializeField]private Cleanable[] cleanables;

    public GameObject noteBundlePrefab;
    public Transform[] spawnPoints;

    private NavMeshAgent agent;
    private float restDuration = 5f;
    private float restTimer = 0f;

    public bool AIisMoving;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartWaitingInLine();
        hotelTable = GameObject.FindGameObjectWithTag("Hall_Table").GetComponent<HotelTableScript>();
        cleanables = FindObjectsOfType<Cleanable>();
    }

    void Update()
    {
        animationControl();
        switch (currentState)
        {
            case AIState.WaitingInLine:
                if (hotelTable.RoomIsAvaliable)
                {

                    MoveToRoom();
                }
                break;
            case AIState.MovingToRoom:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    StartResting();
                }
                break;
            case AIState.Resting:
                restTimer += Time.deltaTime;
                if (restTimer >= restDuration)
                {
                    LeaveHotel();
                    hotelTable.RoomIsAvaliable = false;
                }
                break;
            case AIState.Leaving:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    ResetGuest();
                }
                break;
        }
    }
    void MoveToWaitingPosition()
    {
        if (waitingPosition != null)
        {
            agent.SetDestination(waitingPosition.position);
            AIisMoving = true;
        }
        else
        {
            Debug.LogError("Waiting position is not assigned for " + gameObject.name);
        }
    }
    void MoveToRoom()
    {
        if (room != null)
        {
            currentState = AIState.MovingToRoom;
            agent.SetDestination(room.position);
            AIisMoving = true;

            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomIndex];

            Instantiate(noteBundlePrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        }
        else
        {
            Debug.LogError("Room is not assigned for " + gameObject.name);
        }
    }
    void StartResting()
    {
        currentState = AIState.Resting;
        agent.isStopped = true;
        restTimer = 0f;
        AIisMoving = false;
    }
    void LeaveHotel()
    {
        if (exitPoint != null)
        {
            currentState = AIState.Leaving;
            agent.isStopped = false;
            agent.SetDestination(exitPoint.position);
        }
        else
        {
            Debug.LogError("Exit point is not assigned for " + gameObject.name);
        }
    }
    void ResetGuest()
    {
        currentState = AIState.WaitingInLine;
        agent.isStopped = false;
        StartWaitingInLine();
    }
    public void StartWaitingInLine()
    {
        currentState = AIState.WaitingInLine;
        MoveToWaitingPosition();
    }
    void animationControl()
    {
        float speed = agent.velocity.magnitude;

        animator.SetFloat("Speed", speed);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftRoomGate") || other.CompareTag("RightRoomGate"))
        {
            hotelTable.numOfRoomsCleaned -= 1;
            foreach (var cleanable in cleanables)
            {
                cleanable.MessUp();
            }
        }
    }
}