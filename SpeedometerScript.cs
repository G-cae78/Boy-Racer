using TMPro;
using UnityEngine;

public class SpeedometerScript : MonoBehaviour
{
    public Rigidbody car;
    public TMP_Text speedkmh; 
    public Transform needle;
    public float maxSpeed = 180f; // Max speed for the needle to reach 0 degrees
    
    void Start()
    {
       
    }

    void FixedUpdate()
    {
        //Adjusting scale of needle so it fits speedometer
        if(needle.localEulerAngles.z>125 || needle.localEulerAngles.z<45){
            needle.localScale = new Vector3(0.5f, 1f, 1f);
        }
        else{
            needle.localScale = new Vector3(1f, 1f, 1f);
        }
       
        // Get the current speed in km/h
        int currSpeed = Mathf.RoundToInt(car.linearVelocity.magnitude * 3.6f); // Unity units to km/h
        speedkmh.text = currSpeed + " Km/h";//writing speed inside speedometer

        // Calculate the needle's rotation angle
        float angle = Mathf.Lerp(180, 0, Mathf.Clamp01(currSpeed / maxSpeed));//smoothly moving needle
        needle.localEulerAngles = new Vector3(0, 0, angle);
    }
}
