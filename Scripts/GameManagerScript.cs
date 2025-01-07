using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public enum GAMESTATES
    { // Declaring enum to store the game states
        MENU,
        PLAYING,
        PAUSE,
        LEADERBOARD
    }

    public static GameManagerScript instance; // Creating instance of script so it can be accessed publicly
    public static GAMESTATES gameState = GAMESTATES.MENU;
    public static GAMESTATES CurrentGameState = GAMESTATES.MENU;

    public GameObject GameMenuCanvas, PlayingCanvas, PauseCanvas, ControlCanvas, LeaderBoardCanvas;
    public TMP_Text countDown; // On-screen countdown for race to start

    // Player Car GameObject
    public GameObject playerCar;

    // Bot Car GameObjects
    public GameObject[] botCars;

    // Storing initial positions for each bot
    public Vector3 playerInitialPos=new Vector3(-18f,2.998f,-13f);
    public Vector3[] initialBotPositions;
    public Quaternion[] initialBotRotation;

    public Button pause;
    public bool paused; // Boolean to get if game is paused
    public bool restarted=false;
    private bool movingFaster;

    // Variables to store source of audio and two audio clips
    public AudioSource source;
    public AudioClip accelerate;
    public AudioClip idle;

    private float carCurrHeight;

    public Vector3 carDirection;

    void Start()
{
    instance = this;
    paused = false;
    SetGameState(GAMESTATES.MENU); // Starting game in menu
    Debug.Log("Playing idle audio");

    // Store initial positions as Vector3
    //playerInitialPos = playerCar.transform.position; 

    // Correctly store the initial positions
    initialBotPositions = new Vector3[botCars.Length];
    initialBotRotation = new Quaternion[botCars.Length];


    for (int i = 0; i < botCars.Length; i++)
    {
        initialBotPositions[i] = botCars[i].transform.position;
        initialBotRotation[i] = botCars[i].transform.rotation;
    }

    //playerInitialPos = playerCar.transform.position; // Store initial position as Vector3
}


    public void startGame()
    {
        SetGameState(GAMESTATES.PLAYING); // Changing game state to playing

        Debug.Log("Switched to playing state");
        StartCoroutine(StartCountDown()); // Starting countdown for race to start
    }

    private IEnumerator StartCountDown()
    {
        countDown.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            if (i == 3)
                countDown.color = Color.red; // Red for 3
            else if (i == 2)
                countDown.color = new Color(1f, 0.647f, 0f); // Amber for 2
            else if (i == 1)
                countDown.color = Color.green; // Green for 1

            countDown.text = i.ToString(); // Display "3", "2", "1"
            yield return new WaitForSeconds(1f); // Wait one second
        }

        countDown.color = Color.green;
        countDown.text = "GO!"; // Display "GO!" at the end
        yield return new WaitForSeconds(1f);

        countDown.gameObject.SetActive(false);
    }

//Metho to switch between game states
    public static void SetGameState(GAMESTATES state)
    {
        gameState = state;
        Debug.Log(state);
        if (instance == null) return; //if instance is null returns

        switch (state)//using switchstatement to activate and ddeactivate state canvaases as needed
        {
            case GAMESTATES.MENU:
                if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(true);
                if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
                if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
                if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(false);
                CurrentGameState = GAMESTATES.MENU;//updating current game state
                break;

            case GAMESTATES.PLAYING:
                if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(true);
                if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
                if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
                if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(false);
                CurrentGameState = GAMESTATES.PLAYING;//updating current game state
                break;

            case GAMESTATES.PAUSE:
                if (instance.PauseCanvas) instance.PauseCanvas.SetActive(true);
                if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
                if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
                if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(false);
                CurrentGameState = GAMESTATES.PAUSE;//updating current game state
                break;

            case GAMESTATES.LEADERBOARD:
                if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(true);
                if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
                if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
                if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
                CurrentGameState = GAMESTATES.LEADERBOARD;//updating current game state
                break;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movingFaster = true;
            changeAudio();//changing audio when car starts going fast
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            movingFaster = false;
            changeAudio();//changing audio when car is no longer accelerating
        }
        else if (playerCar.GetComponent<Rigidbody>().linearVelocity.magnitude <= 2f && movingFaster)
        {
            movingFaster = false;
            changeAudio();
        }

        carCurrHeight = playerCar.transform.position.y;
        if (carCurrHeight > 10f)
        {
            respawn();
        }
    }

    public void restartGame()//method to be called when restart game bbutton is pressed
    {
        restarted=true;
        Debug.Log("Restart Button Clicked");

        // Reset positions
        playerCar.transform.position = playerInitialPos;//resetting player car to intial position
        Debug.Log(botCars.Length);

        //Resetting bot cars back to their initial rotations and positions at the start of the race
        for (int i = 0; i < botCars.Length; i++)
        {
            Debug.Log("Resetting bot " + i + " position to: " + initialBotPositions[i]);
            botCars[i].transform.position = initialBotPositions[i];
            botCars[i].transform.rotation=initialBotRotation[i];
        }

        // Reset Rigidbody velocities
        ResetRigidBody(playerCar);

        foreach (var bot in botCars)
        {
            ResetRigidBody(bot);//resetting all bot cars rigidbodies
        }
       StartCoroutine(StartCountDown());
       //SetGameState(GAMESTATES.PAUSE);
       SetGameState(GAMESTATES.PLAYING);//setting game state back to playing
    }

    private void ResetRigidBody(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();//getting object rigid body
        if (rb != null)
        {
            //resetting velocities to zero
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void pauseGame()
    {
        Debug.Log("Pause Button Clicked");
        SetGameState(GAMESTATES.PAUSE);//switching game state when pause button is clicked
    }

    public void resumeGame()
    {
        Debug.Log("Resume Button Clicked");
        if (CurrentGameState == GAMESTATES.PAUSE)//resume game by switching state from pause to playing
        {
            Debug.Log("Resuming game...");
            SetGameState(GAMESTATES.PLAYING);
        }
    }

    public void quitGame()
    {
        Debug.Log("Quit Button Clicked");
        SetGameState(GAMESTATES.MENU);//swithcing to main menu when quit button is clicked
    }

    // public void viewControls()
    // {
    //     Debug.Log("Activating ControlCanvas...");
    //     if (ControlCanvas != null) ControlCanvas.SetActive(true);
    // }

//Method to switch between audios as needed
    public void changeAudio()
    {
        if (movingFaster && source.clip != accelerate)
        {
            Debug.Log("Playing accelerate audio");
            source.Stop();
            source.clip = accelerate;
            source.Play();
        }
        else if (!movingFaster && source.clip != idle)
        {
            Debug.Log("Playing idle audio");
            source.Stop();
            source.clip = idle;
            source.Play();
        }
    }

    public void respawn()
    {
        Debug.Log(carCurrHeight);
        //bring car back to ground if it goes too far, simulating downforce
        playerCar.transform.position = new Vector3(playerCar.transform.position.x, 0f, playerCar.transform.position.z);
    }
}
