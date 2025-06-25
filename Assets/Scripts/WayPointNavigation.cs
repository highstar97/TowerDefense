using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayPointNavigation : MonoBehaviour
{
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;

    private float rotationSpeed = 1.0f;

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