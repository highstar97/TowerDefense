using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public float minTime = 1;
    public float maxTime = 5;
    float createTime;
    float currentTime;
    public Transform[] spawnPoints;
    public GameObject droneFactory;
    // Start is called before the first frame update
    void Start()
    {
        createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > createTime)
        {
            GameObject drone = Instantiate(droneFactory);
            int index = Random.Range(0, spawnPoints.Length);
            drone.transform.position = spawnPoints[index].position;
            currentTime = 0;
            createTime = Random.Range(minTime, maxTime);
        }
    }
}
