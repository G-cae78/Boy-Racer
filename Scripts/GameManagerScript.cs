using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public enum GAMESTATES{ //declaring enum to store the game states
        MENU,
        PLAYING,
        PAUSE,
        LEADERBOARD
    }
    public static GameManagerScript instance;//creatibg instance of script to it can be accessed publically
    public static GAMESTATES gameState = GAMESTATES.MENU;
    public static GAMESTATES CurrentGameState = GAMESTATES.MENU;
    public GameObject GameMenuCanvas, PlayingCanvas,PauseCanvas,ControlCanvas,LeaderBoardCanvas;
    public TMP_Text countDown;//on screen countdown for race to start

//Car Gameobjects
    public GameObject playerCar;
    public GameObject bot1;
    public GameObject bot2;
    public GameObject bot3;
    public GameObject bot4;
    public GameObject bot5;
//Storing initial positions for each car game object
    public Transform initialPos1;
    public Transform initialPos2;
    public Transform initialPos3;
    public Transform initialPos4;
    public Transform initialPos5;
    public Transform initialPos6;
    
   //public TMP_Text Laps;

    //public GameObject gameMenu;
    public Button pause;
    public bool paused;//boolean to get if game is paused
    private bool movingFaster;
    //variables to store source of audio and two audio clips
    public  AudioSource source;
    public  AudioClip accelerate;
    public  AudioClip idle;
    private float carCurrHeight;
    //public TMP_Text startCount;
   
    public Vector3 carDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
{ 
    instance=this;
    paused = false;
    SetGameState(GAMESTATES.MENU);//starting game in menu 
    Debug.Log("Playing idle audio");
    initialPos1=playerCar.transform;
    initialPos2=bot1.transform;
    initialPos3=bot2.transform;
    initialPos4=bot3.transform;
    initialPos5=bot4.transform;
    initialPos6=bot5.transform;

   
    //startCountDown();

    // // Set initial positions for the car and bot
    // GameObject player= Instantiate(playerCar);
    // player.transform.position = new Vector3(-129.7f, 5f, -30.4f); // Replace with your specified values.
    // GameObject bot11= Instantiate(bot1);
    // bot11.transform.position = new Vector3(-881.9f, -425f, 110f);
    // GameObject bot22= Instantiate(bot2);
    // bot22.transform.position= new Vector3(-147.3f,-0.3f,43.9f);
    // bot22.transform.position= new Vector3(0,-22f,0f);
}
public void startGame(){
  SetGameState(GAMESTATES.PLAYING);//changing game state to playing

  Debug.Log("Switched to playing state");
  StartCoroutine(StartCountDown());//starting countdown for race to start
}
private IEnumerator StartCountDown()
    {
        // Show the countdown numbers
        countDown.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countDown.text = i.ToString(); // Display "3", "2", "1"
            yield return new WaitForSeconds(1f);//wait one sec between each count
        }

        countDown.text = "GO!"; // Display "GO!" at the end
        yield return new WaitForSeconds(1f);

        countDown.gameObject.SetActive(false); // Hide the countdown
        //SetGameState(GAMESTATES.PLAYING); // Transition to playing state
    }
public static void SetGameState(GAMESTATES state)
{
    gameState = state;
    Debug.Log(state);
    if (instance == null) return;

    switch (state) // switch statement to change between game states as specified
    {
        case GAMESTATES.MENU:
            if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(true);
            if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
            if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
            if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(false);
            CurrentGameState=GAMESTATES.MENU;
            break;

        case GAMESTATES.PLAYING:
            if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(true);
            if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
            if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
            if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(false);
            CurrentGameState=GAMESTATES.PLAYING;
            break;

        case GAMESTATES.PAUSE:
            if (instance.PauseCanvas) instance.PauseCanvas.SetActive(true);
            if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
            if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
            if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(false);
            CurrentGameState=GAMESTATES.PAUSE;
            break;
        case GAMESTATES.LEADERBOARD:
            if (instance.LeaderBoardCanvas) instance.LeaderBoardCanvas.SetActive(true);
            if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
            if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
            if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
            CurrentGameState=GAMESTATES.LEADERBOARD;
            break;
    }
}


    // Update is called once per frame
    void FixedUpdate()
    {
         if (Input.GetKey(KeyCode.UpArrow))
        {
            movingFaster = true;
            changeAudio();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow)) // Detect when the UpArrow key is released
        {
        movingFaster = false;
        changeAudio();
        }
        else if (playerCar.GetComponent<Rigidbody>().linearVelocity.magnitude <= 2f && movingFaster)
        {
            movingFaster = false;
            changeAudio();
        }
         carCurrHeight=playerCar.transform.position.y;
          if(carCurrHeight > 10f){
            respawn();
          }
    }
    public void restartGame()
{
    Debug.Log("Restart Button Clicked");

    // Reset positions
    playerCar.transform.position = initialPos1.position;
    bot1.transform.position = initialPos2.position;
    bot2.transform.position = initialPos3.position;
    bot3.transform.position = initialPos4.position;
    bot4.transform.position = initialPos5.position;
    bot5.transform.position = initialPos6.position;

    // Reset Rigidbody velocities
    ResetRigidBody(playerCar);
    ResetRigidBody(bot1);
    ResetRigidBody(bot2);
    ResetRigidBody(bot3);
    ResetRigidBody(bot4);
    ResetRigidBody(bot5);

    // Set game state to PLAYING
    SetGameState(GAMESTATES.PLAYING);
}

private void ResetRigidBody(GameObject obj)
{
    Rigidbody rb = obj.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

    public void pauseGame(){
        Debug.Log("Pause Button CLicked");
        SetGameState(GAMESTATES.PAUSE);
    }
    public void resumeGame()
    {
        Debug.Log("Resume Button CLicked");
    if (CurrentGameState == GAMESTATES.PAUSE)
    {
        Debug.Log("Resuming game...");
        SetGameState(GAMESTATES.PLAYING);
    }
    }

    public void quitGame(){
        Debug.Log("Quit Button CLicked");
        SetGameState(GAMESTATES.MENU);
    }
    public void viewControls()
    {
    Debug.Log("Activating ControlCanvas...");
    if (ControlCanvas != null) ControlCanvas.SetActive(true);
    }

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
    public void respawn(){
       
        Debug.Log(carCurrHeight);
       
     
        playerCar.transform.position= new Vector3(playerCar.transform.position.x,0f,playerCar.transform.position.z);
       
    }
}
