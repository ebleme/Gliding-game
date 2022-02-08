using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRollingState : BallBaseState
{
    public override void EnterState(BallStateManager ball)
    {
        //ball.OnBallShooted += Ball_OnBallShooted;
        //ball.rotateTween = ball.transform.transform.DORotate(new Vector3(180, 0, 0), ball.ballOrbitalRotationTime).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();
        //ball.rotateTween.Play();

        TouchManager.Instance.StopTouching();

        ball.ballAnimator.SetBool("openWings", false);

        ball.StartCoroutine(WaitTillWingsClosed(ball.rotateTween, ball.rb));
    }

    private IEnumerator WaitTillWingsClosed(Tweener rotateTween, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.2f);

        rotateTween.Play();
        rb.useGravity = true;
    }

    public override void OnCollisionEnter(BallStateManager ball, Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.X1Tag))
        {
            ball.rb.velocity = Vector3.zero;
            ball.rb.angularVelocity = Vector3.zero;

            ball.rb.useGravity = true;
            ball.rb.AddForce(ball.x1JumpForce);

            ball.SwitchState(ball.BallRollingState);
        }
        else if (collision.gameObject.CompareTag(Constants.X2Tag))
        {
            ball.rb.velocity = Vector3.zero;
            ball.rb.angularVelocity = Vector3.zero;

            ball.rb.useGravity = true;
            ball.rb.AddForce(ball.x2JumpForce);

            ball.SwitchState(ball.BallRollingState);
        }
        else if (collision.gameObject.CompareTag(Constants.Plane))
        {
            float distance = Vector3.Distance(ball.transform.position, ball.startPosition);
            ball.endPanel.FinishGame(distance);
        }
    }

    public override void UpdateState(BallStateManager ball)
    {
        if (TouchManager.Instance.IsTouching)
        {
            ball.SwitchState(ball.BallGlidingState);
        }
    }

    public override void FixedUpdateState(BallStateManager ball)
    {

    }
}
