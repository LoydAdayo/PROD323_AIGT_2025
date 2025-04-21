using UnityEngine;

public class vehicleScript : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject FL_Wheel;
    public GameObject FR_Wheel;

    public float maxSpeed = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        ///rb.AddForce(Vector3.forward, ForceMode.Acceleration);
    }
    
    // Update is called once per frame
    void Update()
    {
        Debug.Log(rb.linearVelocity + " " + rb.linearVelocity.magnitude);
        //rb.AddForce(Vector3.forward, ForceMode.Force);
    }


    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            
        }
        

    }
}
