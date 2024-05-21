using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int numOfCleanedThings;
    public int numOfThings;
    public bool roomHasCleaned = false;

    public HotelTableScript hotelTableScript;
         
    private void Update()
    {
        if(numOfCleanedThings >= numOfThings)
        {
            if (!roomHasCleaned)
            {
                hotelTableScript.numOfRoomsCleaned += 1;
                roomHasCleaned = true;
                hotelTableScript.isReloadable = true;
                hotelTableScript.Reload_gap();
            }            
        }     
    }
}