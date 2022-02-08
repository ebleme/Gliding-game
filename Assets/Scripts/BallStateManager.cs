using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BallStateManager : MonoBehaviour
{

    #region State Machine

    BallBaseState currentState;

    public BallOnLauncherState BallOnLauncherState = new BallOnLauncherState();
    public BallGlidingState BallGlidingState = new BallGlidingState();
    public BallRollingState BallRollingState = new BallRollingState();

    #endregion

    #region Component Parameters

    [SerializeField]
    public float glidingDecreaseSpeed = 3f;

    [SerializeField]
    public float glidingRotSpeed = 50f;

    [Tooltip("Smaller is  faster")]
    [SerializeField]
    public float ballOrbitalRotationTime = 0.1f;

    [Header("Collide Managers")]
    [SerializeField] public Vector3 x1JumpForce;
    [SerializeField] public Vector3 x2JumpForce;
    [SerializeField] public EndPanel endPanel;

    [Header("Plane Boundaries")]
    [SerializeField] public float planeLowBoundarie = -498;
    [SerializeField] public float planeHighBoundarie = 498;

    [Space(20)]
    [Tooltip("Dokunurken topun alacaðý rotasyon")]
    [SerializeField] public Vector3 ballGlidingRotationVector = new Vector3(90, 0, -45);

    [Tooltip("Top süzülmeye baþladýðý an alacaðý rotasyon")]
    [SerializeField] public Vector3 ballGlidingVector = new Vector3(90, 0, 0);

    #endregion


    #region Local Parameters
    [HideInInspector]
    public Tweener rotateTween;

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public Animator ballAnimator;

    [HideInInspector]
    public Vector3 startPosition;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        currentState = BallOnLauncherState;
        currentState.EnterState(this);
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(BallBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public void Launch(Vector3 force)
    {
        SwitchState(BallRollingState);

        rotateTween = gameObject.transform.transform.DORotate(new Vector3(180, 0, 0), ballOrbitalRotationTime).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();

        rb.isKinematic = false;
        rb.AddForce(force);
    }
}
