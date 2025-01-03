using UnityEngine;
using TMPro;

public class FinishLine : MonoBehaviour
{
    public int totalLaps = 3; // Total laps to start with
    private int playerLaps=1;
    private int aiLapsRemaining=1;
     public TMP_Text Laps;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize laps remaining for both cars
        // playerLapsRemaining = totalLaps;
        // aiLapsRemaining = totalLaps;
    }
    void Update(){
      Laps.text="Lap "+playerLaps+"/"+totalLaps;
    }

    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log($"Object entered the finish line: {other.name}");
        // Check if the player car crosses the finish line
        if (other.CompareTag("PlayerCar"))
        {
                playerLaps++;
                Debug.Log($"Player car: {playerLaps} laps remaining.");
        }

        // Check if the AI car crosses the finish line
        else if (other.CompareTag("AICar"))
        {
            if (aiLapsRemaining > 0)
            {
                aiLapsRemaining--;
                Debug.Log($"AI car: {aiLapsRemaining} laps remaining.");
            }
        }

        // Optional: Trigger a win/loss condition if laps reach zero
        if (playerLaps > totalLaps)
        {
            Debug.Log("Player car finished all laps!");
            GameManagerScript.SetGameState(GameManagerScript.GAMESTATES.MENU);
        }

        if (aiLapsRemaining == 0)
        {
            Debug.Log("AI car finished all laps!");
        }
    }
}