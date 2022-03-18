using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLookComponent;
 


    public void RotateCamera() {
        
        freeLookComponent.m_XAxis.Value += 90;
    }

    public void ZoomInCamera() {
        freeLookComponent.m_YAxis.Value = 0;
    }

    public void ZoomOutCamera() {
        freeLookComponent.m_YAxis.Value = 10;
    }

    public void ZoomCamera(float val)
    {
        freeLookComponent.m_YAxis.Value += val;
        CinemachineFreeLook ca;
        
    }
}
