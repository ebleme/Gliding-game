using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Top ilerledikçe topun etrafýnda objeler oluþturma denemesi
public class SpawnAroundBall : MonoBehaviour
{
    public GameObject jumperPrefab;


    public void CreateJumpersInsideUnitCircle(int num, Vector3 point, float radius)
    {
        for (int i = 0; i < num; i++)
        {
            Spawn(radius, point);
        }
    }


    public void Spawn(float radius, Vector3 point)
    {
        var randomPos = (Vector3)UnityEngine.Random.insideUnitSphere * radius;
        randomPos += point;
        randomPos.y = 0;

        Instantiate(jumperPrefab, randomPos, transform.rotation);
    }

    public void CreateEnemiesAroundPoint(int num, Vector3 point, float radius)
    {

        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var jumper = Instantiate(jumperPrefab, spawnPos, Quaternion.identity) as GameObject;

            /* Rotate the enemy to face towards player */
            jumper.transform.LookAt(point);

            /* Adjust height */
            jumper.transform.Translate(new Vector3(0, jumper.transform.localScale.y / 2, 0));
        }
    }
}
