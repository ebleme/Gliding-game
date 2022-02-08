using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Stick")]
    [SerializeField] private Transform stickEndTransform;
    [SerializeField] private float stickEndPositioningTime = 1f;

    [Header("Drag & Force")]
    [SerializeField] Transform ball;

    [Tooltip("Oyun ekranýnýn yüzde kaçý kadar hareket yapýlacaðýný belirler. Örn: 2 seçilirse ekran geniþliðinin % 50 kadarý hareket max DragPower ý verir.")]
    [SerializeField] float dragMultiplier = 2f;
    [SerializeField] float forceMultiplier = 30f;
    [SerializeField] float forceMultiplierY = 3f;

    //private Vector3 mOffset;
    //private float mZCoord;
    //float distanceFromStart;

    Vector3 startPos;
    [Header("Debug")]
    public float dragPower;
    public Vector3 forceVector;

    bool isShoot;

    //Vector3 dragStartPos;
    //Touch touch;

    public event Action OnLaunched;

    //Vector3 initialTransformOfLauncherCube;


    private void Awake()
    {
        startPos = transform.position;
    }

    private void Start()
    {
        //initialTransformOfLauncherCube = gameObject.transform.position;

        //TouchManager.Instance.OnTouchStarted += Instance_OnTouchStarted;
        //TouchManager.Instance.OnTouchEnded += Instance_OnTouchEnded;
    }

    bool isDragging;
    private void Update()
    {
        if (isShoot)
            return;

        if (TouchManager.Instance.IsTouching && !isDragging)
        {
            isDragging = true;
        }
        else if (!TouchManager.Instance.IsTouching && isDragging)
        {
            isDragging = false;

            DragRelease();
        }

        if (!isDragging)
            return;

        dragPower = TouchManager.Instance.TouchPos.x * -1 * dragMultiplier;
        dragPower = Mathf.Clamp(dragPower, 0f, 0.9f);
        AnimateStick();

        //if (isShoot || !draggingStarted)
        //    return;


        //distanceFromStart = Mathf.Abs( initialTransformOfLauncherCube.z - TouchManager.Instance.GetTouch().z);
        ////print("Distance: " + distanceFromStart);

        //dragPower = distanceFromStart / 10;
        //dragPower = Mathf.Clamp(dragPower, 0f, 0.9f);
        //AnimateStick();
        //if (Input.touchCount > 0)
        //{
        //    touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        DragStart();
        //    }

        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //        Dragging();
        //    }

        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        DragRelease();
        //    }
        //}
    }

    //void DragStart()
    //{
    //    if (isShoot) return;

    //    //dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);

    //    //mZCoord = Camera.main.WorldToScreenPoint(touch.position).z;

    //    //mOffset = gameObject.transform.position - GetTouchAsWorldPoint();

    //}
    //void Dragging()
    //{
    //    //if (isShoot) return;


    //    ////print("Touch Pos: " + touch.position);

    //    //transform.position = GetTouchAsWorldPoint() + mOffset + new Vector3(0, 0.75f, 0);

    //    ////print("startPos: " + startPos);
    //    ////print("currentPos: " + transform.position);

    //    //distanceFromStart = Vector3.Distance(startPos, transform.position);
    //    ////print("Distance: " + distanceFromStart);

    //    //dragPower = distanceFromStart / 10;
    //    //dragPower = Mathf.Clamp(dragPower, 0f, 0.9f);
    //    //AnimateStick();
    //}

    void DragRelease()
    {
        animator.SetBool("isShoot", true);
        OnLaunched.Invoke();

        StartCoroutine(StopAnimation());
    }

    //private Vector3 GetTouchAsWorldPoint()
    //{
    //    // Pixel coordinates of touch (x,y)
    //    Vector3 mousePoint = touch.position;

    //    // z coordinate of game object on screen
    //    mousePoint.z = mZCoord;

    //    // Convert it to world points
    //    return Camera.main.ScreenToWorldPoint(mousePoint);
    //}

    private IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        if (isShoot) yield return null;
        else isShoot = true;

        animator.SetFloat("dragPower", 0);
        animator.SetBool("isShoot", false);
        animator.StopPlayback();

        forceVector = dragPower * forceMultiplier * transform.position;
        forceVector.z *= -1;
        forceVector.y *= forceMultiplierY;

        ball.gameObject.GetComponent<Ball>().Launch(forceVector);

        gameObject.SetActive(false);
    }

    private void AnimateStick()
    {
        animator.SetTrigger("dragPower");
        animator.SetFloat("dragPower", dragPower);
        animator.Play("Armature|Bend_Stick");
    }

    private void FixedUpdate()
    {
        if (!isShoot)
            ball.position = Vector3.Lerp(ball.position, stickEndTransform.position, stickEndPositioningTime);
    }
}


//void OnMouseDown()
//{
//    if (isShoot) return;

//    mZCoord = Camera.main.WorldToScreenPoint(
//        gameObject.transform.position).z;

//    // Store offset = gameobject world pos - mouse world pos

//    mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
//}

//private Vector3 GetMouseAsWorldPoint()
//{
//    // Pixel coordinates of mouse (x,y)
//    Vector3 mousePoint = Input.mousePosition;

//    // z coordinate of game object on screen
//    mousePoint.z = mZCoord;

//    // Convert it to world points
//    return Camera.main.ScreenToWorldPoint(mousePoint);
//}

//void OnMouseDrag()
//{
//    if (isShoot) return;

//    transform.position = GetMouseAsWorldPoint() + mOffset;

//    print("startPos: " + startPos);
//    print("currentPos: " + transform.position);

//    distanceFromStart = Vector3.Distance(startPos, transform.position);
//    print("Distance: " + distanceFromStart);

//    dragPower = distanceFromStart / 10;
//    dragPower = Mathf.Clamp(dragPower, 0f, 0.9f);
//    AnimateStick();
//}

//private void OnMouseUp()
//{
//    if (isShoot) return;
//    else isShoot = true;

//    animator.SetBool("isShoot", true);

//    StartCoroutine(StopAnimation());

//}
