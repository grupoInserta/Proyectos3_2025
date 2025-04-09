using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Camera cam;
    public float maxSpeed;
    public float acceleration;

    [Header("Mouse settings")]
    public float sensibility;
    public Quaternion mouseRotation;

    [Header("Clamping")]
    public float minPitch = -90f;
    public float maxPitch = 90f;

    public Vector2 movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseRotation();
        GetMovement();
        transform.rotation = Quaternion.Euler(new Vector3(0f,mouseRotation.eulerAngles.y,0f));
        cam.transform.rotation = mouseRotation;
    }

    void GetMovement()
    {
        bool usingGamepad = Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "";

        if (usingGamepad)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
    }
    void GetMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibility;
        float mouseY = Input.GetAxis("Mouse Y") * sensibility;

        mouseRotation.x += mouseX;
        mouseRotation.y -= mouseY;
        mouseRotation.y = Mathf.Clamp(mouseRotation.y, minPitch, maxPitch);

        Quaternion yaw = Quaternion.Euler(0f, mouseRotation.x, 0f);
        Quaternion pitch = Quaternion.Euler(mouseRotation.y, 0f, 0f);

        Quaternion finalRotation = yaw * pitch;
        mouseRotation = finalRotation;
    }
}
