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
        Debug.Log("Playing idle audio");
        currAudio.clip=idle;
        currAudio.Play();
        movingFaster = false;
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (car.linearVelocity.magnitude > 2f && !movingFaster)
        {
            movingFaster = true;
            changeAudio();
        }
        else if (car.linearVelocity.magnitude <= 2f && movingFaster)
        {
            movingFaster = false;
            changeAudio();
        }
    }

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
