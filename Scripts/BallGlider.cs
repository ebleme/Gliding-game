using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGlider : MonoBehaviour
{
    [SerializeField]
    private float glidingSpeed = 3f;

    [SerializeField]
    private float glidingRotSpeed = 50f;

    [Tooltip("Smaller is  faster")]
    [SerializeField]
    private float ballOrbitalRotationTime = 0.1f;

    //public event Action OnGlidingStart;
    //public event Action OnGlidingEnd;

    private Animator ballAnimator;

    Rigidbody rb;
    Ball ball;

    bool isGliding;
    bool isShooted;

    Tweener rotateTween;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ball = GetComponent<Ball>();
        ballAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        //TouchManager.Instance.OnTouchStarted += Instance_OnTouchStarted;
        //TouchManager.Instance.OnTouchEnded += Instance_OnTouchEnded;

        ball.OnBallShooted += (Ball_OnBallShooted);
    }

    private void Ball_OnBallShooted()
    {
        isShooted = true;

        rotateTween = gameObject.transform.transform.DORotate(new Vector3(180, 0, 0), ballOrbitalRotationTime).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();
    }

    private void OnTouchStarted()
    {
        isGliding = true;
        
        rotateTween.TogglePause();
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        ballAnimator.SetBool("openWings", true);

        // Top yere bakacak þekilde rotasyonu düzenleniyor.
        gameObject.transform.DORotate(new Vector3(90, 0, 0), 0.3f);

        rb.useGravity = false;
        //rb.velocity = new Vector3(rb.velocity.x, -glidingSpeed, rb.velocity.z + 2);
        rb.velocity = new Vector3(0, -glidingSpeed, rb.velocity.z + 2);
    }

    private void OnTouchEnded()
    {
        TouchManager.Instance.StopTouching();

        isGliding = false;

        ballAnimator.SetBool("openWings", false);
        //rb.AddForce(-Vector3.forward);

        StartCoroutine(WaitTillWingsClosed());
    }

    private IEnumerator WaitTillWingsClosed()
    {
        yield return new WaitForSeconds(0.2f);

        rotateTween.TogglePause();
        rb.useGravity = true;
    }

    public void SetIsGliding(bool gliding)
    {
        if (gliding)
            OnTouchStarted();
        else
            OnTouchEnded();
    }

    private void Update()
    {
        if (!isShooted && !isGliding)
            return;

        if (TouchManager.Instance.IsTouching && !isGliding)
        {
            OnTouchStarted();
        }
        else if (!TouchManager.Instance.IsTouching && isGliding)
        {
            OnTouchEnded();
        }
    }

    void FixedUpdate()
    {
        if (!isShooted || !isGliding)
            return;

        Vector3 target = gameObject.transform.position;
        target.x += TouchManager.Instance.TouchPos.x;

        // Plane Boundaries fixation
        if (target.x >= 498)
            return;//target.x -= 0.5f;
        else if (target.x <= -498)
            return;//target.x += 0.5f;

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target, glidingRotSpeed * Time.fixedDeltaTime);

        //Quaternion targetRot = transform.rotation;
        //targetRot.z += -TouchManager.Instance.TouchPos.x / 450;
        //targetRot.z = Mathf.Clamp(targetRot.z, -30f, 30f);
        float x = 90;
        float y = 0;
        float z = -45 * TouchManager.Instance.TouchPos.x;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(x, y, z), glidingSpeed * Time.fixedDeltaTime);
    }
}
