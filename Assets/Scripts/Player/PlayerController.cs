using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private MobileJoystick joystick;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;
    public CinemachineVirtualCamera plyrCam;
    public GameManager gameManager;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float newOffsetDuration = 1f;
    [SerializeField] private Vector3 newFollowLeftOffset;
    [SerializeField] private Vector3 newFollowRightOffset;
    [SerializeField] private Vector3 initialOffset;

    private Coroutine changeOffsetCoroutine;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        Vector3 correctedJoystickVector = joystick.GetMoveVector();
        correctedJoystickVector.z = correctedJoystickVector.y;
        correctedJoystickVector.y = 0;

        playerAnimator.ManageAnimations(correctedJoystickVector);

        Vector3 moveVector = joystick.GetMoveVector() * moveSpeed * Time.deltaTime;

        moveVector.z = moveVector.y;
        moveVector.y = 0;

        characterController.Move(moveVector);
    }

    public bool IsMoving()
    {
        return joystick.GetMoveVector().magnitude > 0;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Upgrade"))
        {
            gameManager.MinusPoints();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            gameManager.AddPoint();
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("LeftRoomGate"))
        {
            if (changeOffsetCoroutine != null)
            {
                StopCoroutine(changeOffsetCoroutine);
            }
            changeOffsetCoroutine = StartCoroutine(SmoothChangeOffset(newFollowLeftOffset, newOffsetDuration));
        }
        if (other.CompareTag("RightRoomGate"))
        {
            if (changeOffsetCoroutine != null)
            {
                StopCoroutine(changeOffsetCoroutine);
            }
            changeOffsetCoroutine = StartCoroutine(SmoothChangeOffset(newFollowRightOffset, newOffsetDuration));
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftRoomGate"))
        {
            if (changeOffsetCoroutine != null)
            {
                StopCoroutine(changeOffsetCoroutine);
            }
            changeOffsetCoroutine = StartCoroutine(SmoothChangeOffset(initialOffset, newOffsetDuration));
        }
        if (other.CompareTag("RightRoomGate"))
        {
            if (changeOffsetCoroutine != null)
            {
                StopCoroutine(changeOffsetCoroutine);
            }
            changeOffsetCoroutine = StartCoroutine(SmoothChangeOffset(initialOffset, newOffsetDuration));
        }
    }
    IEnumerator SmoothChangeOffset(Vector3 targetOffset, float duration)
    {
        var transposer = plyrCam.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer == null) yield break;

        Vector3 startOffset = transposer.m_FollowOffset;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transposer.m_FollowOffset = Vector3.Lerp(startOffset, targetOffset, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transposer.m_FollowOffset = targetOffset;
    }
}