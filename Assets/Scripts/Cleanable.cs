using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cleanable : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;
    [SerializeField] private ParticleSystem cleanParticles;
    [SerializeField] private RoomManager roomManager;

    [Header(" Settings ")]
    private bool isClean;

    void Start()
    {
        MessUp();
        roomManager = transform.parent.gameObject.GetComponent<RoomManager>();
    }

    void Update()
    {
        FaceCanvas();
    }

    private void FaceCanvas()
    {
        canvas.transform.forward = (Camera.main.transform.position - canvas.transform.position).normalized;
    }

    public void CleanProcess(float value)
    {
        fillImage.fillAmount += value;

        if (fillImage.fillAmount >= 1)
            SetAsClean();
    }

    private void SetAsClean()
    {
        isClean = true;
        roomManager.numOfCleanedThings += 1;
        canvas.gameObject.SetActive(false);

        animator.Play("Clean");

        cleanParticles.Play();
    }

    public void MessUp()
    {
        isClean = false;

        canvas.gameObject.SetActive(true);

        fillImage.fillAmount = 0;

        animator.Play("MessUp");

    }

    public bool IsClean()
    {
        return isClean;
    }

}
