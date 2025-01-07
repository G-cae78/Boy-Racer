using UnityEngine;

public class WaypointCar : MonoBehaviour
{
    public Transform[] waypoints; //Array to store checkpoints to traverse map
    public float speed = 10f;
    public float rotationSpeed = 2f; // setting turning speed
    private int currentWaypointIndex = 0;
    //private FrontWheelScript wheelScript;
    private float raceStartTime=3f;
    private float spawntime; //used to make bot cars wait for countdown before starting
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spawntime = Time.time;
    }

    void Update()
    {
        // Start the race after the race start time
        if (GameManagerScript.CurrentGameState == GameManagerScript.GAMESTATES.PLAYING)
        {
            // Set spawntime when the game enters the PLAYING state
            if (spawntime == 0f)
            {
                spawntime = Time.time;//getting time game entered playing state and begon countdown
            }
        if (Time.time > spawntime + raceStartTime)//waiting 3 seconds
        {
            // Check if there are waypoints
            if (waypoints.Length == 0) return; // Exit if no waypoints are set

            // Get the current target waypoint
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = targetWaypoint.position - transform.position;
            float step = speed * Time.deltaTime;

            // Rotate smoothly towards the waypoint
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the waypoint
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

            // Check if the car reached the waypoint
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                // Move to the next waypoint, looping back to the start if needed
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }
    if(GameManagerScript.instance.restarted){
        currentWaypointIndex=0;//resetting waypoint index when game is restarted
    }

}
}