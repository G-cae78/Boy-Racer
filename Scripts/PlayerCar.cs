using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerCar : MonoBehaviour
{
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float maxSteeringAngle = 30f;

    private float currentSteeringAngle;
    private float currentBrakeForce;
    private bool isBraking;

    public TMP_Text countDown;
    private bool finishedCountDown;

    void Start()
    {
        finishedCountDown = false;
        StartCoroutine(StartCountDown());
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 1500f;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 0.1f;
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Adjust the center of mass for stability
    }

    void Update()
    {
        if (finishedCountDown)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheelPoses();
            ApplyDownforce();
        }
    }

    private void GetInput()
    {
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        float motorInput = Input.GetAxis("Vertical");
        frontLeftWheel.motorTorque = motorInput * motorForce;
        frontRightWheel.motorTorque = motorInput * motorForce;

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        frontLeftWheel.brakeTorque = currentBrakeForce;
        frontRightWheel.brakeTorque = currentBrakeForce;
        rearLeftWheel.brakeTorque = currentBrakeForce;
        rearRightWheel.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        float steeringInput = Input.GetAxis("Horizontal");
        currentSteeringAngle = steeringInput * maxSteeringAngle;
        frontLeftWheel.steerAngle = currentSteeringAngle;
        frontRightWheel.steerAngle = currentSteeringAngle;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransform);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion quat;
        wheelCollider.GetWorldPose(out pos, out quat);
        transform.position = pos;
        transform.rotation = quat;
    }

    private IEnumerator StartCountDown()
    {
        countDown.text = "3";
        countDown.color = Color.red;
        yield return new WaitForSeconds(1f);

        countDown.text = "2";
        countDown.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
        yield return new WaitForSeconds(1f);

        countDown.text = "1";
        countDown.color = Color.yellow;
        yield return new WaitForSeconds(1f);

        countDown.text = "GO!!";
        countDown.color = Color.green;
        yield return new WaitForSeconds(1f);

        countDown.text = "";
        finishedCountDown = true;
    }
    public float downForce = 50f;

    private void ApplyDownforce()
    {
    GetComponent<Rigidbody>().AddForce(-transform.up * downForce * GetComponent<Rigidbody>().linearVelocity.magnitude);
    }

}
