using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGlidingState : BallBaseState
{
    public override void EnterState(BallStateManager ball)
    {
        ball.rotateTween.Pause();
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        ball.ballAnimator.SetBool("openWings", true);

        // Top yere bakacak þekilde rotasyonu düzenleniyor.
        ball.gameObject.transform.DORotate(ball.ballGlidingVector, 0.3f);

        ball.rb.useGravity = false;
        //rb.velocity = new Vector3(rb.velocity.x, -glidingSpeed, rb.velocity.z + 2);
        ball.rb.velocity = new Vector3(0, -ball.glidingDecreaseSpeed, ball.rb.velocity.z);
    }

    public override void FixedUpdateState(BallStateManager ball)
    {
        Vector3 target = ball.gameObject.transform.position;
        target.x += TouchManager.Instance.TouchPos.x;

        // Plane Boundaries fixation
        if (target.x >= ball.planeHighBoundarie)
            return;//target.x -= 0.5f;
        else if (target.x <= ball.planeLowBoundarie)
            return;//target.x += 0.5f;

        ball.gameObject.transform.position = Vector3.Lerp(ball.gameObject.transform.position, target, ball.glidingRotSpeed * Time.fixedDeltaTime);

        Quaternion rot = Quaternion.Euler(ball.ballGlidingRotationVector.x, ball.ballGlidingRotationVector.y, ball.ballGlidingRotationVector.z * TouchManager.Instance.TouchPos.x);

        ball.gameObject.transform.rotation = Quaternion.Lerp(ball.gameObject.transform.rotation, rot, ball.glidingDecreaseSpeed * Time.fixedDeltaTime);
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
            //FindObjectOfType<EndPanel>().FinishGame(distance);
            ball.endPanel.FinishGame(distance);
        }
    }

    public override void UpdateState(BallStateManager ball)
    {
        if (!TouchManager.Instance.IsTouching)
        {
            ball.SwitchState(ball.BallRollingState);
        }
    }
}
