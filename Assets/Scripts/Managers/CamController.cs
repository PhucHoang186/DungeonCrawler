using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCamera;
using UnityEngine;

namespace Managers
{
    public class CamController : MonoBehaviour
    {
        public static CamController Instance;
        private List<VirtualCam> cams;
        private VirtualCam currentCam;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            GetAllCams();
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void GetAllCams()
        {
            cams = GetComponentsInChildren<VirtualCam>().ToList();
        }

        public void SetCurrentCam(CamType camType, Transform followTarget = null, Transform lookAtTarget = null, float fov = 0f)
        {
            SetCamPrior(camType);
            SetFollowAndLookAt(followTarget, lookAtTarget, fov);
        }

        private void SetFollowAndLookAt(Transform followTarget, Transform lookAtTarget, float fov)
        {
            if (followTarget != null)
                currentCam.Follow(followTarget);
            if (lookAtTarget != null)
                currentCam.LookAt(lookAtTarget);
            if (fov != 0f)
                currentCam.FOV(fov);
        }

        private void SetCamPrior(CamType camType)
        {
            foreach (var cam in cams)
            {
                if (cam.camType == camType)
                {
                    cam.SetPriority(999);
                    currentCam = cam;
                }
                else
                {
                    cam.SetPriority(10);
                }
            }
        }

        public void ShakeCam(float duration, float amplitude, float frequency)
        {
            currentCam.ShakeCam(duration, amplitude, frequency);
        }
    }
}
