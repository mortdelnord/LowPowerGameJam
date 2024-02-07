using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Cameras : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vCam1;
    [SerializeField] CinemachineVirtualCamera vCam2;
    [SerializeField] CinemachineVirtualCamera vCam3;
    [SerializeField] CinemachineVirtualCamera vCam4;

    [SerializeField] Transform playerPos;


    public void Update()
    {
        float playerToCam1 = Vector3.Distance(vCam1.transform.position, playerPos.position);
        float playerToCam2 = Vector3.Distance(vCam2.transform.position, playerPos.position);
        float playerToCam3 = Vector3.Distance(vCam3.transform.position, playerPos.position);
        float playerToCam4 = Vector3.Distance(vCam4.transform.position, playerPos.position);

        if (playerToCam1 < playerToCam2 && playerToCam1 < playerToCam3 && playerToCam1 < playerToCam4)
        {
            vCam1.Priority = 10;
            vCam2.Priority = 9;
            vCam3.Priority = 9;
            vCam4.Priority = 9;
        }
        if (playerToCam2 < playerToCam1 && playerToCam2 < playerToCam3 && playerToCam2 < playerToCam4)
        {
            vCam1.Priority = 9;
            vCam2.Priority = 10;
            vCam3.Priority = 9;
            vCam4.Priority = 9;
        }
        if (playerToCam3 < playerToCam1 && playerToCam3 < playerToCam2 && playerToCam3 < playerToCam4)
        {
            vCam1.Priority = 9;
            vCam2.Priority = 9;
            vCam3.Priority = 10;
            vCam4.Priority = 9;
        }
        if (playerToCam4 < playerToCam1 && playerToCam4 < playerToCam2 && playerToCam4 < playerToCam3)
        {
            vCam1.Priority = 9;
            vCam2.Priority = 9;
            vCam3.Priority = 9;
            vCam4.Priority = 10;
        }
    }
}
