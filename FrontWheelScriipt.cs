using UnityEngine;

//Attempt to make car wheels turn while car is turning to improve UI
public class FrontWheelScript : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject gameCam;
    public float followSpeed = 1f;
    public float moveForce = 2000f;
    public float steerTorque = 4000f;

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
}
