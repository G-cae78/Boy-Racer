using UnityEngine;

public class WaypointCar : MonoBehaviour
{
    public Transform[] waypoints; // Assign waypoints in the Inspector
    public float speed = 10f;
    public float rotationSpeed = 2f; // Adjust for faster/slower turning
    private int currentWaypointIndex = 0;
    private FrontWheelScript wheelScript;
    private float raceStartTime=3f;
    private float spawntime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawntime=Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>spawntime+raceStartTime){
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
         if(wheelScript!=null){
            wheelScript.waypoint=targetWaypoint;
         }
        // Check if the AI car reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint, looping back to the start if needed
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
    }
}