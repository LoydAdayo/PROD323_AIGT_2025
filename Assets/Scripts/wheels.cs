using UnityEngine;

public class wheels : MonoBehaviour
{
    public Rigidbody rb;

    public float maxSpeed = 10f;
    public float torque = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rb.linearVelocity + " " + rb.linearVelocity.magnitude);
        rb.AddTorque(new Vector3(1f, 0f, 0f) * torque);
    }
}
