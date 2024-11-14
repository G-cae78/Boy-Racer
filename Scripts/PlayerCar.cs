using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject gameCam;
    public float followSpeed = 1f;
    public ParticleSystem flameParticleSystem;

    // Start is called before the first execution of Update
    void Start()
    {
        Debug.Log("Mass:" + rigid.mass);
    }

    // Update is called once per frame
    void Update()
    {
        // Existing keyboard controls (keep these if you also want to support keyboard input)
    
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
}
