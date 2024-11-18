using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnScreenWheelScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float wheelRotation;
    public float prevRotation;
    private Vector2 centre;
    public bool beingUsed;
    public float maxSteeringAngle = 200f;
    public float returnSpeed = 300f;
    public RectTransform wheel;

    void Start()
    {
        wheelRotation = 0f; // Initial wheel rotation
    }

    void Update()
    {
        if (!beingUsed)
        {
            // Return the wheel back to center over time when not being dragged
            wheelRotation = Mathf.MoveTowards(wheelRotation, 0, returnSpeed * Time.deltaTime);
        }

        // Apply the rotation to the wheel's transform
        if (wheel != null)
        {
            wheel.localEulerAngles = new Vector3(0, 0, wheelRotation); // Negative for clockwise rotation
        }
    }

    public void OnPointerDown(PointerEventData pointData)
    {
        beingUsed = true;
        centre = RectTransformUtility.WorldToScreenPoint(pointData.pressEventCamera, wheel.position);
        prevRotation = Vector2.Angle(Vector2.up, pointData.position - centre);
    }

    public void OnPointerUp(PointerEventData pointData)
    {
        beingUsed = false;
    }

    public void OnDrag(PointerEventData pointData)
    {
        if (beingUsed)
        {
            Vector2 currentPosition = pointData.position;
            float currentRotation = Vector2.Angle(Vector2.up, currentPosition - centre);
            float rotationDifference = currentRotation - prevRotation;

            // Adjust for screen-space angle changes
            if (Vector3.Cross(Vector2.up, currentPosition - centre).z < 0)
            {
                rotationDifference = -rotationDifference;
            }

            wheelRotation += rotationDifference;
            wheelRotation = Mathf.Clamp(wheelRotation, -maxSteeringAngle, maxSteeringAngle);
            prevRotation = currentRotation;
        }
    }
}
