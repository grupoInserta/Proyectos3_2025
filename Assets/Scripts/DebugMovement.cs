using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DebugMovement : MonoBehaviour
{
    public float linearSpeed;
    public float angularSpeed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * linearSpeed;

        rb.angularVelocity = rb.angularVelocity = Vector3.up * angularSpeed * Mathf.Deg2Rad;
    }
}
