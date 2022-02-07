using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [Header("Objects to spawn")]
    [SerializeField] private List<GameObject> jumpingObjects;

    [Header("Object min and max counts to set in Random range")]
    [SerializeField] private int objectCountMinValue;
    [SerializeField] private int objectCountMaxValue;

    [Header("Object min and max distances to set in Random range")]
    [SerializeField] private float objectSpawnXDistanceMinValue;
    [SerializeField] private float objectSpawnXDistanceMaxValue;

    [Header("Object min and max Y values to set in Random range")]
    [SerializeField] private float objectMinYValue;
    [SerializeField] private float objectMaxYValue;

    [Header("Object min and max Z values to set in Random range (According to Row Scale)")]
    [SerializeField] private float objectMinZValue;
    [SerializeField] private float objectMaxZValue;

    [Header("Object min and max Scales to set in Random range (According to Row Scale)")]
    [SerializeField] private float objectMinScale;
    [SerializeField] private float objectMaxScale;


    List<GameObject> spawnedObjects;
    float lastSpawnedObjectXValue;

    void Awake()
    {
        spawnedObjects = new List<GameObject>();
        lastSpawnedObjectXValue = -5; //gameObject.transform.lossyScale.x / 2 * -1;

        int objectCount = Random.Range(objectCountMinValue, objectCountMaxValue);

        Vector3 rowScale = gameObject.transform.lossyScale;

        for (int i = 0; i < objectCount; i++)
        {
            // scale X ve Z içinde ayný olacak
            float scale = Random.Range(objectMinScale, objectMaxScale);

            // Y deðerinin belirlenmesi
            float yValue = Random.Range(objectMinYValue, objectMaxYValue);
            float zValue = Random.Range(objectMinZValue, objectMaxZValue);

            // X deðerinde baþlangýç noktasýna göre veya önceki objenin konumuna göre ne kadar fark olsun
            float xDistance = Random.Range(objectSpawnXDistanceMinValue, objectSpawnXDistanceMaxValue);

            int randomObjectIndex = Random.Range(0, jumpingObjects.Count);

            Vector3 pos = new Vector3(lastSpawnedObjectXValue + xDistance, yValue, zValue);

            lastSpawnedObjectXValue = pos.x;

            GameObject obj = Instantiate(jumpingObjects[randomObjectIndex], gameObject.transform);
            obj.transform.localPosition = pos;
            obj.transform.localScale = new Vector3(scale / rowScale.x, 1, scale / rowScale.z);
            spawnedObjects.Add(obj);
        }
    }
}
