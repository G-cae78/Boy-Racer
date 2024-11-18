using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject gameCam;
    public float followSpeed = 1f;
    public ParticleSystem flameParticleSystem;
    public TMP_Text countDown;
    private bool finishedCountDown;
    private OnScreenWheelScript wheelScript;

    // Start is called before the first execution of Update
    void Start()
    {
        Debug.Log("Mass:" + rigid.mass);
        finishedCountDown=false;
        StartCoroutine(startCountDown());
    }

    // Update is called once per frame
    void Update()
    {
        // Existing keyboard controls (keep these if you also want to support keyboard input)
    if(finishedCountDown){
    if (Input.GetKey(KeyCode.UpArrow))
    {
        MoveForward();
    }
    else if (Input.GetKey(KeyCode.DownArrow))
    {
        MoveBackward();
    }
    
    if (Input.GetKey(KeyCode.LeftArrow))
    {
        TurnLeft();
    }
    else if (Input.GetKey(KeyCode.RightArrow))
    {
        TurnRight();
    }
    if(wheelScript!=null && wheelScript.beingUsed){
        float steering=wheelScript.wheelRotation/wheelScript.maxSteeringAngle;
        Debug.Log(steering);
        rigid.AddTorque(Vector3.up * steering * (rigid.mass *Time.fixedDeltaTime * 4000f));
    }
    }

    }

    // Public methods for button input
    public void MoveForward()
    {
        rigid.AddForce(transform.forward * (rigid.mass * Time.fixedDeltaTime * 1000f));
        if (!flameParticleSystem.isPlaying)
        {
            flameParticleSystem.Play();
        }
    }

    public void MoveBackward()
    {
        rigid.AddForce(-transform.forward * (rigid.mass * Time.fixedDeltaTime * 1000f));
    }

    public void TurnLeft()
    {
        rigid.AddTorque(-Vector3.up * (rigid.mass * Time.fixedDeltaTime * 4000f));
    }

    public void TurnRight()
    {
        rigid.AddTorque(Vector3.up * (rigid.mass * Time.fixedDeltaTime * 4000f));
    }

   private IEnumerator startCountDown(){
    //setting initial color and number to be displayed for each countdown second
    countDown.text="3";
    countDown.color=Color.red;
    yield return new WaitForSeconds(1f);

    countDown.text="2";
    countDown.color=Color.Lerp(Color.red,Color.yellow,0.5f);//setting color to be between red and yellow
    finishedCountDown=true;
    yield return new WaitForSeconds(1f);

    countDown.text="1";
    countDown.color=Color.yellow;
    yield return new WaitForSeconds(1f);

    countDown.text="GO!!";
    countDown.color=Color.green;
    yield return new WaitForSeconds(1f);

    countDown.text=" ";//hide text bpox after go
    finishedCountDown=true;
    
 }

}
