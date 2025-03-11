using UnityEngine;

public class Letter : MonoBehaviour
{
    [SerializeField] private GameObject readCanvas;  // Canvas show "Read"
    [SerializeField] private GameObject textCanvas;  // Canvas with the text tutorial
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Player player;
    [SerializeField] private CameraManager cameraController;
    
    private bool isPlayerNear = false; // near to the letter
    private bool isInputLock = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readCanvas.SetActive(false);
        textCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInputLock && isPlayerNear && playerController.GetInteract() > 0) 
        {          
            readCanvas.SetActive(false);
            textCanvas.SetActive(true);
            LockPlayer();
        }
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readCanvas.SetActive(true);
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readCanvas.SetActive(false);
            isPlayerNear = false;
        }
    }


    void LockPlayer()
    {
        isInputLock = true;

        
        if (playerController != null)
        {
            player.enabled = false; // Disable player movement
            cameraController.enabled = false; // Disable camera movement
        }
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }

    void UnlockPlayer()
    {
        isInputLock = false;

        if (playerController != null)
        {
            player.enabled = true; // Re-enable player movement
            cameraController.enabled = true; // Re-enable camera movement
        }
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor at the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    public void CloseTextCanvas()
    {
        textCanvas.SetActive(false);
        UnlockPlayer();
    }
}
