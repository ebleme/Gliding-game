using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    [SerializeField]
    private Ball ball;

    [SerializeField]
    private float fogOffset = 200f;
    
    [SerializeField]
    private float speed = 1f;

    ParticleSystem fogParticle;

    private void Awake()
    {
        fogParticle = GetComponent<ParticleSystem>();

        //StartCoroutine(StartFog());
    }

    private IEnumerator StartFog()
    {
        yield return new WaitUntil(() => TouchManager.Instance.GameStarted);

        fogParticle.Play();
    }

    private void FixedUpdate()
    {
        Vector3 target = ball.gameObject.transform.position;
        target.z += fogOffset;

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target, speed);      
    }
}
