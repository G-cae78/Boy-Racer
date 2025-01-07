using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarRaceScript : MonoBehaviour
{
    public Transform[] waypoints;  // Waypoints to follow
    public GameObject[] cars;  // All cars in the race
    public TMP_Text racePositionText;  // To display the player's race position
    public TMP_Text finalPlace; 

    private int currentWaypointIndex = 0;  // The current waypoint the car is heading to
    private float distanceTravelled = 0f;  // Distance the car has traveled along the track
    private float[] carDistances;  // Array to store the distances of all cars

    private void Start()
    {
        carDistances = new float[cars.Length];  // Initialize the array to store car distances
    }

    private void Update()
    {
        CalculateDistance();
        UpdateRacePosition();//calling method to continually update race position
    }

    private void CalculateDistance()
    {
        // If no waypoints, do nothing
        if (waypoints.Length == 0) return;

        // Calculate the distance between the car and the next waypoint
        Transform nextWaypoint = waypoints[currentWaypointIndex];

        // Calculate the distance between the car and the next waypoint
        distanceTravelled = Vector3.Distance(transform.position, nextWaypoint.position);
    }

    private void UpdateRacePosition()
    {
        // Update distances for all cars
        for (int i = 0; i < cars.Length; i++)
        {
            CarRaceScript carScript = cars[i].GetComponent<CarRaceScript>();
            carDistances[i] = carScript.distanceTravelled;  // Store each car's distance to the next waypoint
        }

        // Sort the cars by distance to the next waypoint the closest to the waypoint is leading
        float[] sortedDistances = (float[])carDistances.Clone();
        System.Array.Sort(sortedDistances);

        // Determine the player's position based on the sorted distances
        int playerIndex = System.Array.IndexOf(cars, gameObject);
        int playerPosition = System.Array.IndexOf(sortedDistances, carDistances[playerIndex]) + 1;  // Add 1 to start position from 1, not 0

        // Update the UI with the current position
        Debug.Log(playerPosition);
        racePositionText.text = playerPosition +" / 6" ;
        finalPlace.text=playerPosition.ToString();
    }
}
