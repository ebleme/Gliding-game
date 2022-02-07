using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraFollowSpeed;


    private Launcher launcher;

    Camera cam;

    bool followCam;

    private void Awake()
    {
        cam = Camera.main;

        launcher = FindObjectOfType<Launcher>();
        launcher.OnLaunched += Launcher_OnLaunched;
    }

    private void Launcher_OnLaunched()
    {
        followCam = true;
        //cam.transform.DOLookAt(ball.transform.position, 1f);

        cam.transform.DORotateQuaternion(target.rotation, 1f);

    }

    private void FixedUpdate()
    {
        if (followCam)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, target.position + cameraOffset, cameraFollowSpeed);
            //cam.transform.DOMove(ball.transform.position + cameraOffset, cameraFollowSpeed);
        }
    }
}
