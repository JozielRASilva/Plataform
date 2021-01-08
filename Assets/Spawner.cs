using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject enemy;

    float time = 0;

    public float coolDown = 2;

    void Start()
    {

        PoolManager.WarmPool(enemy, 100);
        time = Time.time;
    }

    void Update()
    {
        if (time < Time.time)
        {
            PoolManager.SpawnObject(enemy, transform.position, transform.rotation);
            time += 0.1f * coolDown;
        }

        Debug.Log($"{time} : {Time.time}");
    }
}
