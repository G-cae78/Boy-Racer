using UnityEngine;

public class ControlLights : MonoBehaviour
{
    public Rigidbody carRigidbody;  // Reference to the car's Rigidbody
    public GameObject reverseLights;  // Reference to the reverse lights GameObject
    public Material reverseLightsMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure reverse lights are initially off
        reverseLights.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the car is moving backward
        if (Vector3.Dot(carRigidbody.linearVelocity, carRigidbody.transform.forward) < 1 && Input.GetKey(KeyCode.DownArrow))
        {
            // The car is moving backward, turn on reverse lights
            reverseLights.SetActive(true);
            reverseLightsMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            // The car is not moving backward, turn off reverse lights
            reverseLights.SetActive(false);
            reverseLightsMaterial.DisableKeyword("_EMISSION");
        }
    }
}
