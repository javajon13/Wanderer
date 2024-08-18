using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CamType{front, left, right, top}
public class CameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera frontCam;
    public CinemachineVirtualCamera leftCam;
    public CinemachineVirtualCamera rightCam;
    public CinemachineVirtualCamera topCam;

    public CamType myType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            switch(myType)
            {
                case CamType.front:
                    frontCam.Priority = 10;
                    leftCam.Priority = 0;
                    rightCam.Priority = 0;
                    topCam.Priority = 0;
                    break;
                case CamType.left:
                    frontCam.Priority = 0;
                    leftCam.Priority = 10;
                    rightCam.Priority = 0;
                    topCam.Priority = 0;
                    break;
                case CamType.right:
                    frontCam.Priority = 0;
                    leftCam.Priority = 0;
                    rightCam.Priority = 10;
                    topCam.Priority = 0;
                    break;
                case CamType.top:
                    frontCam.Priority = 0;
                    leftCam.Priority = 0;
                    rightCam.Priority = 0;
                    topCam.Priority = 10;
                    break;
            }
        }
    }
}
