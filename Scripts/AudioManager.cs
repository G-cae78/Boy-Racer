using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource currAudio;
    public AudioClip accelerate;
    public AudioClip idle;
    public Rigidbody car;
    private bool movingFaster;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Playing games default audio
        Debug.Log("Playing idle audio");
        currAudio.clip=idle;
        currAudio.Play();
        movingFaster = false;
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //If the car has started to speed up change to acceleration audio
        if (car.linearVelocity.magnitude > 2f && !movingFaster)
        {
            movingFaster = true;
            changeAudio();
        }
        //if car is slowing down switch to idle audio
        else if (car.linearVelocity.magnitude <= 2f && movingFaster)
        {
            movingFaster = false;
            changeAudio();
        }
    }

//method to stop current playing audio and switch to required audio as needed
    public void changeAudio()
    {
        if (movingFaster && currAudio.clip != accelerate)
        {
            Debug.Log("Playing accelerate audio");
            currAudio.Stop();
            currAudio.clip = accelerate;
            currAudio.Play();
        }
        else if (!movingFaster && currAudio.clip != idle)
        {
             Debug.Log("Playing idle audio");
            currAudio.Stop();
            currAudio.clip = idle;
            currAudio.Play();
        }
    }
}
