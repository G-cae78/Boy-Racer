using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody playerRigidBody;
    public float moveCam = 20f;
    public float followSpeed = 5f;

    private Vector3 followOffset = new Vector3(0f, 5f, -15f);
    private Vector3 inspectionOffset = Vector3.zero;

    void FixedUpdate()
    {
        // Ensure playerTransform is assigned
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        // Base target position (follow the player with an offset)
        Vector3 targetPosition = playerTransform.position + followOffset + inspectionOffset;

        // Adjust camera position using keys for inspection
        if (Input.GetKey(KeyCode.A))
        {
            inspectionOffset.x -= moveCam * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inspectionOffset.x += moveCam * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            inspectionOffset.z += moveCam * Time.deltaTime;
        }

        // Smoothly interpolate to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // Make the camera look at the player
        transform.LookAt(playerTransform);
    }
}
