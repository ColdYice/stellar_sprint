using UnityEngine;

public class WayponitFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 4f;

    private int currentWaypointIndex = 0;

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance = 10f;

    private void Update()
    {
        if (isChasing)
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }

        else
        {
            // Для врагов
            if (gameObject.name == "Enemy")
            {
                if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                {
                    isChasing = true;
                }

                if (currentWaypointIndex == 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, Time.deltaTime * speed);
                    if (Vector2.Distance(transform.position, waypoints[0].transform.position) < .1f)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        currentWaypointIndex = 1;
                    }
                }

                if (currentWaypointIndex == 1)
                {
                    transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, Time.deltaTime * speed);
                    if (Vector2.Distance(transform.position, waypoints[1].transform.position) < .1f)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        currentWaypointIndex = 0;
                    }
                }
            }

            // Для платформ, лезвии и тп
            else
            {
                if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
                {
                    currentWaypointIndex++;
                    if (currentWaypointIndex >= waypoints.Length)
                    {
                        currentWaypointIndex = 0;
                    }
                }
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
            }
        }
        if (gameObject.CompareTag("Trap"))
        {
            speed = 8f;
        }
    }
}
