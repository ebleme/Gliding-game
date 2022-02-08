using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    Ball ball;

    public List<GameObject> prefabs;
    [SerializeField] int minSize = 1000;
    [SerializeField] int maxSize = 1500;

    [Space(20)]
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private float nearVector = 200f;
    [SerializeField] private float cameraBehindVector = 30f;

    public int size;
    Queue<GameObject> pool;
    public bool initialSpawn;

    public GameObject currentPlane;
    Vector3 firstPlanePos = new Vector3(0, 0, -500);

    #region Singleton

    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }

    #endregion

    void Start()
    {
        ball = FindObjectOfType<Ball>();

        GeneratePlane();

        pool = new Queue<GameObject>();
        size = Random.Range(minSize, maxSize);

        GameObject prefab;
        for (int i = 0; i < size + 1000; i++)
        {
            prefab = prefabs[Random.Range(0, prefabs.Count)];

            GameObject obj = Instantiate(prefab, transform.parent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        initialSpawn = true;
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        GameObject objToSpawn = pool.Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.localScale = scale;
        objToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        pool.Enqueue(objToSpawn);

        return objToSpawn;
    }

    public void HideObjectsBehindTheCamera()
    {
        foreach (var item in pool)
            if (item.transform.position.z + cameraBehindVector < ball.gameObject.transform.position.z)
                item.SetActive(false);
    }

    //void Update()
    //{

    //}

    float time = 1f;
    float timeCount;
    private void FixedUpdate()
    {
        ManageObjesctByBallMovement();
    }

    private void ManageObjesctByBallMovement()
    {
        timeCount += Time.fixedDeltaTime;
        if (timeCount > time)
        {
            timeCount = 0;

            HideObjectsBehindTheCamera();
            CreateNewPlaneIfBallNear(ball.transform.position);
        }
    }

    private void GeneratePlane()
    {
        Vector3 position = currentPlane != null ? currentPlane.transform.position : firstPlanePos;
        position += new Vector3(0, 0, 1000);

        currentPlane = Instantiate(planePrefab, transform);
        currentPlane.transform.position = position;

        StartCoroutine(GetComponent<JumpAreaSpawner>().RandomGenerate());
    }

    public void CreateNewPlaneIfBallNear(Vector3 ballPos)
    {
        if (currentPlane.transform.position.z + 500 - ballPos.z < nearVector)
        {
            GeneratePlane();
        }
    }

}
