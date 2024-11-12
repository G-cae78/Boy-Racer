using System.Data.Common;
using UnityEngine;


public class PlayerCar : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject gameCam;
     public float followSpeed = 1f; 
     public ParticleSystem flameParticleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Mass:"+rigid.mass);
    }

    // Update is called once per frame
    void Update()
    {
        // // Target position with an offset
        // Vector3 targetPosition = this.transform.position + new Vector3(0, 1, -6);
        
        // Smoothly interpolate between the current position and the target position
        //gameCam.transform.position = Vector3.Lerp(gameCam.transform.position, targetPosition, followSpeed);
        
        if(Input.GetKey(KeyCode.UpArrow)){
            rigid.AddForce(transform.forward* (rigid.mass * Time.fixedDeltaTime *1000f));
            if(!flameParticleSystem.isPlaying){
                flameParticleSystem.Play();
            }
            else if(flameParticleSystem.isPlaying){
                flameParticleSystem.Stop();
            }
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            rigid.AddForce(-transform.forward* (rigid.mass * Time.fixedDeltaTime *1000f));
        }
         if (Input.GetKey(KeyCode.LeftArrow))
          rigid.AddTorque(-Vector3.up * (rigid.mass * Time.fixedDeltaTime * 4000f));
        else if (Input.GetKey(KeyCode.RightArrow))
            rigid.AddTorque(Vector3.up * (rigid.mass * Time.fixedDeltaTime * 4000f));
    
    }
}
