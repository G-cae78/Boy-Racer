using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public int totalLaps = 3; // Total laps to start with
    private int playerLapsRemaining;
    private int aiLapsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize laps remaining for both cars
        playerLapsRemaining = totalLaps;
        aiLapsRemaining = totalLaps;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Object entered the finish line: {other.name}");
        // Check if the player car crosses the finish line
        if (other.CompareTag("Player"))
        {
            if (playerLapsRemaining > 0)
            {
                playerLapsRemaining--;
                Debug.Log($"Player car: {playerLapsRemaining} laps remaining.");
            }
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
        if (playerLapsRemaining == 0)
        {
            Debug.Log("Player car finished all laps!");
        }

        if (aiLapsRemaining == 0)
        {
            Debug.Log("AI car finished all laps!");
        }
    }
}