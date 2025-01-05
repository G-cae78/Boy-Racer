using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FinishLine : MonoBehaviour
{
    public int totalLaps = 1; // Total laps to start with
    private int playerLaps=1;
//    private int aiLapsRemaining=2;
     public TMP_Text Laps;// Variable to display number of laps on canvas
     List<string> finishingOrder = new List<string>();//list to store finishing order
     public enum CarType //enum to store names of cars on grid
    {
        PlayerCar,
        Gray,
        Blue,
        Red,
        Yellow,
        Purple
    }

    // Dictionary to store laps remaining for each car
    private Dictionary<CarType, int> carLapsRemaining;//dictionary to map car to the number of laps it has remaining

    void Start()
    {
        // Initialize laps remaining for each car
        carLapsRemaining = new Dictionary<CarType, int>
        {
            { CarType.Gray, totalLaps },
            { CarType.Blue, totalLaps },
            { CarType.Red, totalLaps },
            { CarType.Yellow, totalLaps },
            { CarType.Purple, totalLaps }
        };
    }

    void Update(){
      Laps.text="Lap "+playerLaps+"/"+totalLaps;//updating number of laps for player car
    }

    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log($"Object entered the finish line: {other.name}");
        // Check if the player car crosses the finish line
         if (other.CompareTag(CarType.PlayerCar.ToString()))
        {
            playerLaps++;//increase lap count after player crosses finish line
            Debug.Log($"Player car: {playerLaps} laps completed.");

            if (playerLaps > totalLaps) //if player has completed all laps
            {
                Debug.Log("Player car finished all laps!");
                finishingOrder.Add(CarType.PlayerCar.ToString());//add player to finish order list to determine finish pos
                // Update the leaderboard
            LeaderboardTable leaderboardTable = FindFirstObjectByType<LeaderboardTable>();//find leaderboard object in canvas
            if (leaderboardTable != null)
            {
                //leaderboardTable.UpdateLeaderboard(finishingOrder);
                leaderboardTable.UpdateLeaderBoard(finishingOrder);//update leaderboard with current finishng order
            }
        else
        {
        Debug.LogError("LeaderboardTable not found in the scene!");
        }
                GameManagerScript.SetGameState(GameManagerScript.GAMESTATES.LEADERBOARD);//switch game state to display leaderboard
                playerLaps = 1; // Reset or handle completion
            }
        }
        // Check if an AI car crosses the finish line
        else
        {
            foreach (CarType carType in carLapsRemaining.Keys)
            {
                if (other.CompareTag(carType.ToString()))//determine what car crossed finish line
                {
                    carLapsRemaining[carType]--;//updating laps remaining count for car

                    Debug.Log($"{carType}: {carLapsRemaining[carType]} laps remaining.");

                    if (carLapsRemaining[carType] == 0)
                    {
                        Debug.Log($"{carType} finished all laps!");
                        finishingOrder.Add(carType.ToString());//if car has finished race add to finish list to determine order
                    }

                    break; // Exit loop once the matching car is found
                }
            }
        }
    }
}