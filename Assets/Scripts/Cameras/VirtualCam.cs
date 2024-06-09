using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using NaughtyAttributes;

public class VirtualCam : MonoBehaviour
{
    public CamType camType;
    [SerializeField] bool useFollow;
    [ShowIf("useFollow")][SerializeField] Vector3 followOffset;
    [SerializeField] bool useLookAt;
    [ShowIf("useLookAt")][SerializeField] Vector3 lookAtOffset;
    [SerializeField] bool useFov;
    [ShowIf("useFov")][SerializeField] float fov;
    private CinemachineTransposer transposer;
    private CinemachineComposer composer;
    private CinemachineVirtualCamera virtualCam;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        transposer = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
        composer = virtualCam.GetCinemachineComponent<CinemachineComposer>();

        if (useFollow)
            transposer.m_FollowOffset = followOffset;
        if (useLookAt)
            composer.m_TrackedObjectOffset = lookAtOffset;
        if (useFov)
            FOV(fov);
    }

    public void LookAt(Transform target)
    {
        virtualCam.m_LookAt = target;
    }

    public void Follow(Transform target)
    {
        virtualCam.m_Follow = target;
    }

    public void FOV(float fov)
    {
        virtualCam.m_Lens.OrthographicSize = fov;
    }

    public void SetPriority(int prior)
    {
        virtualCam.Priority = prior;
    }

}
