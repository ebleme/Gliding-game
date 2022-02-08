using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArea : MonoBehaviour, IPooledObject
{
    [SerializeField] private bool isCube;

    public void OnObjectSpawn()
    {
        if (isCube)
        {
            Vector3 newScale = transform.localScale;
            newScale.y *= 2;

            transform.localScale = newScale;
        }
    }
}
