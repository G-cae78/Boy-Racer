using UnityEngine;
using UnityEngine.AI;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    //public Vector3 followOffset = new Vector3(0, 2, -10); // Set a reasonable offset
    public Rigidbody playerRigidBody;
    public float moveCam=7f;

    void FixedUpdate()
    {
        Vector3 targetPosition;
        // Ensure playerTransform is assigned
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }
      
      Debug.Log("Velocity: "+playerRigidBody.linearVelocity.magnitude);
        if(playerRigidBody !=null && playerRigidBody.linearVelocity.magnitude>1f){
            Debug.Log("Player moving");
            targetPosition = new Vector3(playerTransform.position.x+0f,playerTransform.position.y+5f,playerTransform.position.z-15f);
            Debug.Log("TargetPos: "+targetPosition);
            transform.position =  Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);;
        }
        else{ 
        Debug.Log("Player not moving");
         Debug.Log("Player: "+playerTransform.position);
        // Calculate the target position based on player position and offset
         targetPosition = new Vector3(playerTransform.position.x+0f,playerTransform.position.y+5f,playerTransform.position.z-15f);
         Debug.Log("TargetPos: "+targetPosition);
       

        // Debug output to verify the position
        Debug.Log("Camera Position: " + transform.position + ", Target Position: " + targetPosition);

        if(Input.GetKey(KeyCode.A)){
            targetPosition.x-=moveCam * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.D)){
            targetPosition.x+=moveCam * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.W)){
            targetPosition.y+=moveCam * Time.deltaTime;
        }

         transform.position = targetPosition;
    }
     // Ensure the camera is looking at the player
        transform.LookAt(playerTransform);
    }
}
