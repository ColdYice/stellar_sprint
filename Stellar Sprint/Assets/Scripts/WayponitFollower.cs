using UnityEngine;

public class WayponitFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 4f;

    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (currentWaypointIndex == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, Time.deltaTime * speed);
            if (Vector2.Distance(transform.position, waypoints[0].transform.position) < .1f)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
                currentWaypointIndex = 1;
            }
        }

        if (currentWaypointIndex == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, Time.deltaTime * speed);
            if (Vector2.Distance(transform.position, waypoints[1].transform.position) < .1f)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
                currentWaypointIndex = 0;
            }
        }
        if (currentWaypointIndex == 0 && transform.localEulerAngles == new Vector3(0, 0, 0))
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (currentWaypointIndex != 0 && transform.localEulerAngles == new Vector3(0, 180, 0))
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (gameObject.CompareTag("Trap"))
        {
            speed = 8f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(waypoints[0].transform.position, waypoints[1].transform.position);
    }
}
