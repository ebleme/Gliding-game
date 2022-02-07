using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAreaSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> jumpAreaPrefabs;


    [Tooltip("Minimum distance between objects")]
    [SerializeField] float minDistance = 10f;

    [Header("Object min and max counts to set in Random range")]
    [SerializeField] private int objectCountMinValue;
    [SerializeField] private int objectCountMaxValue;

    //[Header("Object min and max distances to set in Random range")]
    //[SerializeField] private float objectSpawnXDistanceMinValue;
    //[SerializeField] private float objectSpawnXDistanceMaxValue;

    [Header("Object min and max Y values to set in Random range")]
    [SerializeField] private float objectMinYValue;
    [SerializeField] private float objectMaxYValue;

    //[Header("Object min and max Z values to set in Random range (According to Row Scale)")]
    //[SerializeField] private float objectMinZValue;
    //[SerializeField] private float objectMaxZValue;

    [Header("Object min and max Scales to set in Random range (According to Row Scale)")]
    [SerializeField] private float objectMinScale;
    [SerializeField] private float objectMaxScale;


    [SerializeField] private Vector3 spawnValues;
    [SerializeField] private float spawnWait;
    [SerializeField] private float spawnMostWait;
    [SerializeField] private float spawnLeastWait;
    [SerializeField] private int startWait;
    [SerializeField] private bool stop;

    int randJumArea;

    int lineIndex = 0;

    //List<GameObject> spawnedObjects;

    bool pointsGenerated;
    Vector3[] positionArray;


    public IEnumerator RandomGenerate()
    {
        yield return new WaitUntil(() => ObjectPooler.Instance.initialSpawn);

        //spawnedObjects = new List<GameObject>();
        Vector3 newRandomPosition = Vector3.zero;
        // Minimum distance they need to be apart

        int maxTriesTodetermineAPosition = 200;    // Number of times to try to generate a position
        float distanceBetweenPoint = Mathf.Infinity;

        int i;
        //int objectCount = Random.Range(objectCountMinValue, objectCountMaxValue);

        positionArray = new Vector3[ObjectPooler.Instance.size];

        for (i = 0; i < positionArray.Length; i++)
        {
            for (int j = 0; j < maxTriesTodetermineAPosition; j++)
            {
                //newRandomPosition.x = Random.Range(ObjectPooler.Instance.currentPlane.transform.lossyScale.x * -5, ObjectPooler.Instance.currentPlane.transform.lossyScale.x * 5);
                //newRandomPosition.z = Random.Range(0, ObjectPooler.Instance.currentPlane.transform.lossyScale.z * 10); 

                newRandomPosition.x = Random.Range(ObjectPooler.Instance.currentPlane.transform.lossyScale.x * -5, ObjectPooler.Instance.currentPlane.transform.lossyScale.x * 5);
                newRandomPosition.z = Random.Range(ObjectPooler.Instance.currentPlane.transform.position.z - 500, ObjectPooler.Instance.currentPlane.transform.position.z + 500);

                distanceBetweenPoint = Mathf.Infinity;

                for (int k = 0; k < i; k++)
                    distanceBetweenPoint = Mathf.Min((newRandomPosition - (positionArray[k])).magnitude, distanceBetweenPoint);

                if (distanceBetweenPoint > minDistance + objectMaxScale) // Far enough apart
                    break;
            }

            if (distanceBetweenPoint < minDistance + objectMaxScale)
            {
                //Debug.Log("Generation failed -- only found " + i + " points");
                break;
            }

            positionArray[i] = newRandomPosition;
        }

        pointsGenerated = true;
        //GenerateObjects(i, objectArray);

        RepositionObjects();
    }

    private void RepositionObjects()
    {
        for (int i = 0; i < positionArray.Length; i++)
        {
            float scale = Random.Range(objectMinScale, objectMaxScale);
            float yValue = Random.Range(objectMinYValue, objectMaxYValue);

            ObjectPooler.Instance.SpawnFromPool(positionArray[i], Quaternion.identity, new Vector3(scale, yValue, scale));
        }
    }

    //int index = 0;
    //private void FixedUpdate()
    //{
    //    if (pointsGenerated && index < positionArray.Length)
    //    {
    //        float scale = Random.Range(objectMinScale, objectMaxScale);
    //        float yValue = Random.Range(objectMinYValue, objectMaxYValue);

    //        ObjectPooler.Instance.SpawnFromPool(positionArray[index], Quaternion.identity, new Vector3(scale, yValue, scale));

    //        index++;
    //    }
    //}

    //private void GenerateObjects(int i, Vector3[] objectArray)
    //{
    //    for (int j = 0; j < i; j++)
    //    {
    //        float scale = Random.Range(objectMinScale, objectMaxScale);

    //        // Y deðerinin belirlenmesi
    //        float yValue = Random.Range(objectMinYValue, objectMaxYValue);

    //        int randomObjectIndex = Random.Range(0, jumpAreaPrefabs.Count);

    //        GameObject go = Instantiate(jumpAreaPrefabs[randomObjectIndex], gameObject.transform, true);
    //        go.transform.position = objectArray[j];
    //        go.transform.localScale = new Vector3(scale, yValue, scale);
    //        go.transform.position = new Vector3(go.transform.position.x, yValue / 2, go.transform.position.z);
    //        spawnedObjects.Add(go);
    //    }
    //}
}
