using UnityEngine;

public class Oar : MonoBehaviour
{
    public Transform leftOar;        
    public Transform rightOar;       
    public Transform leftOarPivot;   
    public Transform rightOarPivot;  

    public float strokeSpeed = 1f;   // rowing cycle speed
    public float strokeAngle = 30f;  // Maximum rotation angle for the oars 
    public float yawAngle = 15f;     // Lateral rotation (yaw), affecting horizontal movement of the oars
    public float rollAngle = 10f;    // Lateral rotation (roll), affecting the side-to-side tilt of the oars

    private float strokeProgress = 0f; // keeps track of the progression of the rowing cycle (0 to 1)
    private bool isRowing = false;     // check if the player is rowing

    void Start()
    {
        
        // These are the default rotation values for the oars at rest 
        leftOar.localRotation = Quaternion.Euler(284.35f, 83.67f, 179.31f);
        rightOar.localRotation = Quaternion.Euler(284.35f, 270f, 179.31f);
    }

    void Update()
    {
        
        if (isRowing)
        {
            // Increment the stroke progress over time based on stroke speed
            // This value will loop back to 0 once it exceeds 1, creating a repeating cycle
            strokeProgress += Time.deltaTime * strokeSpeed;
            if (strokeProgress > 1f)
            {
                strokeProgress = 0f;  // Reset strokeProgress to start a new full cycle
            }

            // calculate a sinusoidal function for the rotation to simulate circular movement
            // using Mathf.Sin and Mathf.Cos creates smooth back-and-forth motion in a circular path
            float oarRotation = Mathf.Sin(strokeProgress * Mathf.PI * 2) * strokeAngle;  // Vertical oar movement
            float oarYaw = Mathf.Cos(strokeProgress * Mathf.PI * 2) * yawAngle;          // Lateral rotation (side-to-side) for horizontal movement
            float oarRoll = Mathf.Sin(strokeProgress * Mathf.PI * 2) * rollAngle;        // Lateral rotation (roll) for side-to-side tilting of the oar

            
            // The left and right oars rotate in opposite directions (hence the negative sign for the right oar)
            leftOarPivot.localRotation = Quaternion.Euler(oarRotation, oarYaw, oarRoll); 
            rightOarPivot.localRotation = Quaternion.Euler(oarRotation, -oarYaw, -oarRoll); 
        }
    }

    // Method to handle player input for rowing
    // This method is called when the player provides input to start or stop rowing
    public void SetRowingInput(float input)
    {

        //if (input > 0)
        //{
        //    isRowing = true;  
        //    strokeSpeed = 1f;  // Set the stroke speed to 1 (you can adjust this value to control the speed)
        //}
        //else
        //{
        //    isRowing = false; 
        //    strokeSpeed = 0f;  // Set the stroke speed to 0, effectively pausing the oar movement
        //}

        if (input > 0)
        {
            isRowing = true;  // Activate oar movement
            strokeSpeed = 1f; // Normal speed
        }
        // If the input is less than 0, the player is rowing backward
        else if (input < 0)
        {
            isRowing = true;  // Activate oar movement
            strokeSpeed = 1f; // Normal speed
            // Reverse the oar movement when moving backward
            leftOarPivot.localRotation = Quaternion.Euler(-Mathf.Sin(strokeProgress * Mathf.PI * 2) * strokeAngle, -Mathf.Cos(strokeProgress * Mathf.PI * 2) * yawAngle, -Mathf.Sin(strokeProgress * Mathf.PI * 2) * rollAngle);
            rightOarPivot.localRotation = Quaternion.Euler(-Mathf.Sin(strokeProgress * Mathf.PI * 2) * strokeAngle, Mathf.Cos(strokeProgress * Mathf.PI * 2) * yawAngle, Mathf.Sin(strokeProgress * Mathf.PI * 2) * rollAngle); // Opposite for right oar
        }
        else
        {
            isRowing = false; // Stop rowing if no input is given
        }
    }


    
}

