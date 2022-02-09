using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  hat tip to youtuber Sebastian Lague for patrolling ideas and snippets I used as starting point
public class PatrolControlType2 : WhetzelControllerType2
{
    public float waitTime = .8f;
    public Transform pathHolder;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, waypoints[i].y, transform.position.z);
        }
        StartCoroutine(FollowPath(waypoints));
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, movementSpeed * Time.deltaTime);
            moveDirection = new Vector2(targetWaypoint.x - transform.position.x, targetWaypoint.y - transform.position.y).normalized;
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .5f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}
