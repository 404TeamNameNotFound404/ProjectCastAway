using UnityEngine;

public class LifeBoat : MonoBehaviour
{

    //RB
    private Rigidbody rb;

    // CONTROLLER 
    [SerializeField] private LifeboatController lifeBoatController;

    // PLAYER REF
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerExitSpawn;
    [SerializeField] private Transform playerSeatPosition;
    private PlayerController playerController;
    private Rigidbody playerRb;

    private bool isPlayerOnBoard = false;  
    private bool canControl = false;

    // CAMERA REF
    [SerializeField] private Camera camera;
    [SerializeField] private Transform cameraBoatPosition;
    [SerializeField] private float cameraLerpSpeed = 3f;

    [SerializeField] private Transform originalCameraPos;

    private bool isLerpingToBoat = false;
    private bool isLerpingToOrigin = false;

    // BOAT
    [SerializeField] private Transform EnterBoatPoint;
    [SerializeField] private Transform ExitBoatPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 10f;

    private Harbor harbor;


    private Oar oar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerRb = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<PlayerController>();
        oar = GetComponent<Oar>();
        harbor = GameObject.Find("BoatExit").GetComponent<Harbor>();


    }

    // Update is called once per frame
    void Update()
    {

        if (isLerpingToBoat)
        {
            CameraBoatOn();
        }
        if (isLerpingToOrigin)
        {
            CameraBoatOff();
        }


        if (isPlayerOnBoard)
        {
            if (lifeBoatController.GetInteract() > 0)
            {
                Debug.Log("Pressing to enter boat");
                canControl = true;
                EnterBoat();
            }

            if (canControl)
            {
                Vector2 moveInput = lifeBoatController.GetMovement();

                float move = moveInput.y;
                float turn = moveInput.x;

                Vector3 movementDirection = transform.forward;

                if (move > 0)
                {
                    // Apply force to accelerate the boat in the forward direction
                    rb.AddForce(-movementDirection * move * speed * Time.deltaTime, ForceMode.Acceleration);
                }

                // When pressing S (backward)
                if (move < 0)
                {
                    // Apply negative force to slow down the boat
                    rb.AddForce(movementDirection * Mathf.Abs(move) * speed * Time.deltaTime, ForceMode.Acceleration);
                }

                // When no input is provided, decelerate the boat
                if (move == 0)
                {
                    // Apply a deceleration force (negative forward force) to gradually slow down the boat
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, Mathf.Lerp(rb.linearVelocity.z, 0f, Time.deltaTime * 2f));
                }


                //rb.AddForce(movementDirection * move * speed * Time.deltaTime, ForceMode.Acceleration); //transform.forward
                transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);

                //oar movement based on input
                oar.SetRowingInput(move);

                player.transform.position = playerSeatPosition.position;
                player.transform.rotation = playerSeatPosition.rotation;


               
            }

            if (harbor.canPlayerExit) // && lifeBoatController.GetInteract() > 0
            {
                if (lifeBoatController.GetInteract() > 0)
                {
                    ExitBoat();
                    Debug.Log("Pressing to exit boat");
                    harbor.canPlayerExit = false;
                    harbor.DisableCollider();
                }

                else
                {
                    lifeBoatController.enabled = true;
                    Debug.Log($"can player exit {harbor.canPlayerExit}");
                    harbor.BoxCollider.enabled = true;
                }


            }

        }
    }


    private void EnterBoat()
    {
        Debug.Log("ENTER ON BOAT ");

        player.transform.SetParent(transform);
        rb.isKinematic = false;
        playerRb.isKinematic = true;
        playerController.enabled = false; // disable the player input
        lifeBoatController.enabled = true;

        harbor.BoxCollider.enabled = true;

        //Camera
        camera.transform.SetParent(null);
        isLerpingToBoat = true;
        isLerpingToOrigin = false;


    }

    private void ExitBoat()
    {
        Debug.Log("EXIT BOAT ");
        player.transform.SetParent(null);
        player.transform.position = playerExitSpawn.position;

       
        canControl = false;
        playerRb.isKinematic = false;
        rb.isKinematic = true;   
        
        playerController.enabled = true; // reable the player input
                                        

        //Camera
        camera.transform.SetParent(null);
        isLerpingToBoat = false;
        isLerpingToOrigin = true;
    }

   

    
    private void CameraBoatOn()
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position, cameraBoatPosition.position, cameraLerpSpeed * Time.deltaTime);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraBoatPosition.rotation, cameraLerpSpeed * Time.deltaTime);

        if (Vector3.Distance(camera.transform.position, cameraBoatPosition.position) <= 0.5f)
        {
            isLerpingToBoat = false;
            camera.transform.SetParent(cameraBoatPosition.transform);
            camera.transform.forward = cameraBoatPosition.transform.forward;
            CameraManager.canLook = false;


        }

        // ---------------------------
        //float distanceToTarget = Vector3.Distance(camera.transform.position, cameraBoatPosition.position);

        //float lerpSpeed = cameraLerpSpeed;

        //if(distanceToTarget <= 1f) 
        //{
        //    lerpSpeed = cameraLerpSpeed * (distanceToTarget / 1f); // Rallenta quando siamo pi� vicini
        //}

        //camera.transform.position = Vector3.Lerp(camera.transform.position, cameraBoatPosition.position, lerpSpeed * Time.deltaTime);
        //camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraBoatPosition.rotation, lerpSpeed * Time.deltaTime);

        //if(distanceToTarget <= 0.1f) 
        //{
        //    isLerpingToBoat = false;
        //    camera.transform.SetParent(cameraBoatPosition.transform);
        //    // Assicurati che la camera non tremi pi�
        //    camera.transform.position = cameraBoatPosition.position;
        //    camera.transform.rotation = cameraBoatPosition.rotation;
        //}
        // ---------------------------

    }



    private void CameraBoatOff()
    {

        camera.transform.position = Vector3.Lerp(camera.transform.position, originalCameraPos.position, cameraLerpSpeed * Time.deltaTime);  //player.transform.position // originalCameraPos
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, originalCameraPos.rotation, cameraLerpSpeed * Time.deltaTime); // originalCameraRot

        if (Vector3.Distance(camera.transform.position, originalCameraPos.position) < 0.1f) // originalCameraPos
        {
            isLerpingToOrigin = false;
            camera.transform.SetParent(originalCameraPos); // player.transform           
            CameraManager.canLook = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            isPlayerOnBoard = true;
            Debug.Log("PRESS E TO ENTER");
            Debug.Log("player on board : " + isPlayerOnBoard);
        }

    }

    


}
