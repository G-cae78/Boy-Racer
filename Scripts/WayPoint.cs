using UnityEngine;

public class WaypointCar : MonoBehaviour
{
    public Transform[] waypoints; // Assign waypoints in the Inspector
    public float speed = 10f;
    public float rotationSpeed = 2f; // Adjust for faster/slower turning
    private int currentWaypointIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards the current waypoint
        if (waypoints.Length == 0) return; // Exit if no waypoints are set

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        float step = speed * Time.deltaTime;

        // Rotate smoothly towards the waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Check if the AI car reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint, looping back to the start if needed
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}