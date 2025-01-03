using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody playerRigidBody;
    public float moveCam = 50f;
    public float followSpeed = 5f;

    private Vector3 followOffset = new Vector3(0f, 5f, -10f);
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

        // Adjust camera position using keys for rotation around the player's local axis
        if (Input.GetKey(KeyCode.A))
        {
            // Rotate left around the player's local up axis
            transform.RotateAround(playerTransform.position, playerTransform.up, 10f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Rotate right around the player's local up axis
            transform.RotateAround(playerTransform.position, -playerTransform.up, 10f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            // Rotate forward around the player's local right axis
            transform.RotateAround(playerTransform.position, playerTransform.right, 10f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Rotate backward around the player's local right axis (opposite of W)
            transform.RotateAround(playerTransform.position, -playerTransform.right, 10f * Time.deltaTime);
        }
        // Smoothly interpolate to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // Make the camera look at the player
        transform.LookAt(playerTransform);
    }
}
