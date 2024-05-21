using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class CleaningSystem : MonoBehaviour
{
    [Header(" Elements ")]
    private PlayerController playerController;

    [Header(" Settings ")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private float cleanSpeed;
    [SerializeField] private LayerMask detectionMask;

    [Header(" Test ")]
    [SerializeField] private int frameRate;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        Application.targetFrameRate = frameRate;
    }

    void Update()
    {

        if (!playerController.IsMoving())
        {
            DetectCleanables();
        }
            
    }

    private void DetectCleanables()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

        foreach (Collider collider in detectedColliders)
        {
            if (collider.transform.parent.TryGetComponent(out Cleanable cleanable))
            {
                if (!cleanable.IsClean())
                {
                    cleanable.CleanProcess(cleanSpeed * Time.deltaTime);
                    break;
                }
            }
            if (collider.transform.parent.TryGetComponent(out HotelTableScript hotelTableScript))
            {
                if (hotelTableScript.isReloadable)
                {
                    hotelTableScript.ReloadProcess(cleanSpeed * Time.deltaTime);
                    break ;
                }
            }
        }
            
    }
}
