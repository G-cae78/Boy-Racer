using UnityEngine;

// Attempt to make car wheels turn while car is turning to improve UI
public class FrontWheelScript : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject gameCam;
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public float followSpeed = 1f;
    public float moveForce = 2000f;
    public float steerTorque = 4000f;
    public float steerAngle = 30f;

    public bool aiControlled=false;
    public Transform waypoint;

    void Start()
    {
        if (rigid == null)
        {
            rigid = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        HandleMovement();//calling script to adjust wheels relative to the cars movement
        HandleSteering();//calling script to adjust wheel rotation relative to the steering
    }

    void HandleMovement()
    {
        //Moving the wheels relative to the cars movement
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigid.AddForce(transform.forward * (rigid.mass * Time.fixedDeltaTime * moveForce));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rigid.AddForce(-transform.forward * (rigid.mass * Time.fixedDeltaTime * moveForce));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.AddTorque(-Vector3.up * (rigid.mass * Time.fixedDeltaTime * steerTorque));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rigid.AddTorque(Vector3.up * (rigid.mass * Time.fixedDeltaTime * steerTorque));
        }
    }

    void HandleSteering()
    {
        //Turning wheels to make game more relaistic as the left and right arrow keys are pressed to simulate a steering wheel
        float steerInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            steerInput = -0.1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            steerInput = 0.1f;
        }

        frontLeftWheel.localRotation = Quaternion.Euler(0, steerInput * steerAngle, 0);//rotating front left wheel
        frontRightWheel.localRotation = Quaternion.Euler(0, steerInput * steerAngle, 0);//rotating front left wheel
    }
}
