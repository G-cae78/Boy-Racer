using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float followSpeed = 5f;    // Speed at which the camera follows the car
    public Vector3 followOffset = new Vector3(0f, 5f, -10f); // Offset relative to the car

    void FixedUpdate()
    {
        // Ensure playerTransform is assigned
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        // Calculate the target position behind the player based on the player's rotation
        Vector3 targetPosition = playerTransform.TransformPoint(followOffset);

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // Ensure the camera is always looking at the player
        transform.LookAt(playerTransform);
    }
}
