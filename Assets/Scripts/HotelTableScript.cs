using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotelTableScript : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Canvas canvas;
    public int numOfRoomsCleaned;

    [Header(" Settings ")]
    public bool isReloadable;
    public bool RoomIsAvaliable;

    void Update()
    {
        FaceCanvas();
        if (fillImage.fillAmount >= 1)
        {
            isReloadable = false;
            fillImage.fillAmount = 0;
            if(numOfRoomsCleaned >= 1)
            {
                RoomIsAvaliable = true;
            }            
        }        
    }

    private void FaceCanvas()
    {
        canvas.transform.forward = (Camera.main.transform.position - canvas.transform.position).normalized;
    }

    public void ReloadProcess(float value)
    {
        fillImage.fillAmount += value;        
        
    }
    public void Reload_gap()
    {
        StartCoroutine(Reload_time());
    }

    IEnumerator Reload_time()
    {
        yield return new WaitForSeconds(1);
        isReloadable = true;
    }
}