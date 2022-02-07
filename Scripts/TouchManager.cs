using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance { get; set; }

    public bool IsTouching { get; set; }

    //private Dictionary<int, Vector2> activeTouches = new Dictionary<int, Vector2>();

    public event Action OnTouchStarted;
    public event Action OnTouchEnded;


    public Vector2 touchStartPos = Vector2.zero;
    public Vector3 TouchPos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    Vector3 touchPos = Vector3.zero;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    IsTouching = true;
                    print("Touc Started");
                    break;
                case TouchPhase.Moved:

                    touchPos = (touch.position - touchStartPos);
                    float mag = touchPos.magnitude / Screen.width;
                    TouchPos = touchPos.normalized * mag;

                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    IsTouching = false;
                    break;
                case TouchPhase.Canceled:
                    IsTouching = false;
                    break;
                default:
                    break;
            }
        }
        //    if (!IsTouching)
        //    {
        //        touchStartPos = touch.position;
        //        //OnTouchStarted.Invoke();
        //    }
        //    else // IsTouching true
        //    {
        //        r = (touch.position - touchStartPos);
        //        float mag = r.magnitude / 300;
        //        TouchPos = r.normalized * mag;
        //    }

        //    IsTouching = true;
        //}
        //else
        //{
        //    //if (IsTouching)
        //    //    OnTouchEnded.Invoke();

        //    IsTouching = false;
        //}
    }

    public void StopTouching()
    {
        Instance.IsTouching = false;
    }

    public Vector3 GetTouch()
    {
        // Read all touches from the user
        Vector3 r = Vector3.zero;

        //foreach (Touch touch in Input.touches)
        //{
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        activeTouches.Add(touch.fingerId, touch.position);

        //        OnTouchStarted.Invoke();
        //    }
        //    else if (touch.phase == TouchPhase.Ended)
        //    {
        //        if (activeTouches.ContainsKey(touch.fingerId))
        //            activeTouches.Remove(touch.fingerId);

        //        OnTouchEnded.Invoke();
        //    }
        //    else
        //    {
        //            float mag = 0; //distance
        //            r = (touch.position - activeTouches[touch.fingerId]);
        //            mag = r.magnitude / 300; // 300 is speed modifier

        //            r = r.normalized * mag;
        //    }
        //}
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                OnTouchStarted.Invoke();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnTouchEnded.Invoke();
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                r = (touch.position - touchStartPos);
                float mag = r.magnitude / Screen.width;
                return r.normalized * mag;
            }
        }

        return Vector3.zero;
    }
}
