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

    void Start()
    {
        if (rigid == null)
        {
            rigid = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        HandleMovement();
        HandleSteering();
    }

    void HandleMovement()
    {
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
        float steerInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            steerInput = -0.1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            steerInput = 0.1f;
        }

        frontLeftWheel.localRotation = Quaternion.Euler(0, steerInput * steerAngle, 0);
        frontRightWheel.localRotation = Quaternion.Euler(0, steerInput * steerAngle, 0);
    }
}
