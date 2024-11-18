using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript gameManager;

    public GameObject playerCar;
    public GameObject bot1;
    public GameObject bot2;

    public GameObject gameMenu;
    public Button pause;
    public bool enterMenu;
    private bool movingFaster;
    public AudioSource source;
    public AudioClip accelerate;
    public AudioClip idle;
    private float carCurrHeight;
    //public TMP_Text startCount;
   

  //  public 

    public Vector3 carDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
{ 
    enterMenu = false;
    gameMenu.SetActive(false);
    Debug.Log("Playing idle audio");

    // Assigning and playing idle audio
    source.clip = idle;
    source.Play();
    movingFaster = false;
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
    public void pauseControl(){
        if(enterMenu==true){
            enterMenu=false;
            gameMenu.SetActive(false);
        }
        else{ 
        enterMenu=true;
        menu();
        }
    }
    public void menu(){
        if(enterMenu)
        gameMenu.SetActive(true);
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
