using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Rigidbody car;
    public float wheelRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(car!=null){
            wheelRotation=car.transform.eulerAngles.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(wheelRotation>0){
            Debug.Log(wheelRotation);
        }
    }
}
