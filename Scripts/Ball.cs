using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Ball : MonoBehaviour
{
    [SerializeField] Vector3 x1JumpForce;
    [SerializeField] Vector3 x2JumpForce;

    Rigidbody rb;
    bool shooted;

    BallGlider ballGlider;

    //Tweener rotateTween;

    public event Action OnBallShooted;

    SpawnAroundBall spawnAround;

    private void Awake()
    {
        spawnAround = GetComponent<SpawnAroundBall>();
        rb = GetComponent<Rigidbody>();
        ballGlider = GetComponent<BallGlider>();
    }

    //private void Start()
    //{
    //    //ballGlider.OnGlidingStart += BallGlider_OnGlidingStart;
    //    //ballGlider.OnGlidingEnd += BallGlider_OnGlidingEnd;
    //}

    //private void BallGlider_OnGlidingEnd()
    //{
    //    rotateTween.Play();
    //}

    //private void BallGlider_OnGlidingStart()
    //{
    //    rotateTween.Pause();
    //}

    //float time = 1f;
    //float timeCounter = 0f;

    //private void Update()
    //{
    //    if (!shooted)
    //        return;
    //}

    public void Launch(Vector3 force)
    {
        if (shooted)
            return;

        shooted = true;
        OnBallShooted.Invoke();

        rb.isKinematic = false;
        
        rb.AddForce(force);
    }

    public bool IsShooted()
    {
        return shooted;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.X1Tag))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = true;
            rb.AddForce(x1JumpForce);

            ballGlider.SetIsGliding(false);
        }
        else if (collision.gameObject.CompareTag(Constants.X2Tag))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            rb.useGravity = true;
            rb.AddForce(x2JumpForce);

            ballGlider.SetIsGliding(false);
        }
    }
}
