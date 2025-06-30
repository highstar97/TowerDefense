using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayPointNavigation : MonoBehaviour
{
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;

    private float rotationSpeed = 1.0f;

    // 작성자 : 박규탁
    // 기  능 : 맵에 배치된 웨이포인트들을 따라가게 하는 것을 목표로 구현중이었으나, 기획을 폐기하고 네비매쉬 사용하기로 결정
    void Update()
    {
        if (waypoints.Length == 0) return;

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) 
            {
                currentWaypointIndex = 0; 
            }
        }
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, Time.deltaTime * 2f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.1f);
        }
    }
}