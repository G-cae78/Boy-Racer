using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public enum GAMESTATES{
        MENU,
        PLAYING,
        PAUSE
    }
    public static GameManagerScript instance;
    public static GAMESTATES gameState = GAMESTATES.MENU;
    public static GAMESTATES CurrentGameState = GAMESTATES.MENU;
    public GameObject GameMenuCanvas, PlayingCanvas,PauseCanvas,ControlCanvas;
    public TMP_Text countDown;

    public GameObject playerCar;
    //public GameObject bot1;
    //public GameObject bot2;
   // public TMP_Text Laps;

    //public GameObject gameMenu;
    public Button pause;
    public bool paused;
    private bool movingFaster;
    public  AudioSource source;
    public  AudioClip accelerate;
    public  AudioClip idle;
    private float carCurrHeight;
    //public TMP_Text startCount;
   

  //  public 

    public Vector3 carDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
{ 
    instance=this;
    paused = false;
    SetGameState(GAMESTATES.MENU);
    Debug.Log("Playing idle audio");

   
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
  SetGameState(GAMESTATES.PLAYING);

  Debug.Log("Switched to playing state");
  StartCoroutine(StartCountDown());
}
private IEnumerator StartCountDown()
    {
        // Show the countdown numbers
        countDown.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countDown.text = i.ToString(); // Display "3", "2", "1"
            yield return new WaitForSeconds(1f);
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

    switch (state)
    {
        case GAMESTATES.MENU:
            if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(true);
            if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
            if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
            CurrentGameState=GAMESTATES.MENU;
            break;

        case GAMESTATES.PLAYING:
            if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(true);
            if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
            if (instance.PauseCanvas) instance.PauseCanvas.SetActive(false);
            CurrentGameState=GAMESTATES.PLAYING;
            break;

        case GAMESTATES.PAUSE:
            if (instance.PauseCanvas) instance.PauseCanvas.SetActive(true);
            if (instance.GameMenuCanvas) instance.GameMenuCanvas.SetActive(false);
            if (instance.PlayingCanvas) instance.PlayingCanvas.SetActive(false);
            CurrentGameState=GAMESTATES.PAUSE;
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
    public void pauseGame(){
        SetGameState(GAMESTATES.PAUSE);
    }
    public void resumeGame()
{
    if (gameState == GAMESTATES.PAUSE)
    {
        Debug.Log("Resuming game...");
        SetGameState(GAMESTATES.PLAYING);
    }
}

    public void quitGame(){
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
