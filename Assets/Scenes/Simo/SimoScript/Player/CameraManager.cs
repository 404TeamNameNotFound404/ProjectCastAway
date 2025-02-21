using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Transform playerBody; 
    [SerializeField] private float sensitivity = 100f; 
    [SerializeField] private float maxVerticalAngle = 80f; 

    private float verticalRotation = 0f; 
    private float horizontalRotation = 0f;
    private PlayerController playerController;

    public static bool canLook = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        Look();
    }

    private void Look() 
    {
        
        if (!canLook) 
        {
            return;
        }

        Vector2 mouseInput = playerController.GetMouseLook();

        float mouseX = mouseInput.x * sensitivity * Time.fixedDeltaTime;
        float mouseY = mouseInput.y * sensitivity * Time.fixedDeltaTime;

        // Update vertical rotation with clamping
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // Update horizontal rotation
        horizontalRotation += mouseX;

        // Apply rotations
        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);

        // Apply horizontal rotation to the player's body
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
