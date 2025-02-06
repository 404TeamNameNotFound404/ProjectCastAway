using UnityEngine;


public class Player : MonoBehaviour, IDamageble
{
    // RB
    private Rigidbody rb;

    // COLLIDER
    private CapsuleCollider capsuleCollider;

    //CONTROLS
    private PlayerController playerController;

    // CAMERA
    [SerializeField] private Camera playerCamera;

    // MOVEMENT
    [SerializeField] private float speedWalk = 10f; // player's speed 
    [SerializeField] private float maxSafeWalkDistance = 10f; // max distance before the drunk's effect will be strong
    [SerializeField] private float maxExtensionOfTheElastic = 20f;
    [SerializeField] private float weightMultiplier = 0.5f; // slowdown factor when weight is applied
    [SerializeField] private float elasticDamping = 2f; // reduce the strength of the elastic when the player returns
    [SerializeField] private float elasticForceMultiplier = 5f; // Intensity of the elastic force ( it will push player to the origin position) 
    [SerializeField] private float randomOscillationStrength = 1f; // random swing strength
    [SerializeField] private float oscillationSpeed = 3f;

    private Vector3 originPlayerPosition; // the last point where the player was 
    private Vector3 randomOffset; //random offset for swing

    private bool hasStopped = false;
    private bool isDraggingWeight = false;

    //THROW
    [SerializeField] private Stun stunPrefab;
    [SerializeField] private Transform originThrow;
    [SerializeField] private float throwDelay = 1;

    private float throwCooldown;


    //HEALTH
    [SerializeField] private float health = 100f;

    private float maxHealth = 100f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        
        originPlayerPosition = transform.position;

        
    }

    // Update is called once per frame
    void Update()
    {
        //if (daynightcycle.isDayTime == true)
        //{
        //    DrunkEffect();
        //}

        CheckIfPlayerStopped();

       // DrunkEffect();

    }



    private void FixedUpdate()
    {
        Move();
        Throw();

    }

    // MOVEMENT

    private void Move()
    {
        Vector2 input = playerController.GetMovement();

        Vector3 cameraForward = playerCamera.transform.forward;

        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f; 

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 movement = new Vector3(input.x, 0, input.y);

        Vector3 movementDirection = (cameraForward * movement.z +  cameraRight * movement.x).normalized;

        float finalSpeed = isDraggingWeight ? speedWalk * weightMultiplier : speedWalk;

        Vector3 newPos = rb.position + movementDirection * finalSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPos);
    }

    // DRUNK EFFECT
    private void DrunkEffect() 
    {
        ElasticForceBefore();
        RandomOscillation();
    }

    private void ElasticForceBefore()
    {
        Vector3 toOrigin = originPlayerPosition - transform.position;

        float distance = toOrigin.magnitude;

        if(distance > maxSafeWalkDistance) 
        {
            
            // The force increases as the distance approaches the maximum extension of the elastic
            float elasticForceMagnitude = Mathf.Clamp((distance - maxSafeWalkDistance) / (maxExtensionOfTheElastic - maxSafeWalkDistance), 0f,1f) * elasticForceMultiplier;

            // If the player is within the max extension of the elastic, scale the force further based on how far 
            if (distance < maxExtensionOfTheElastic) 
            {
                elasticForceMagnitude *= (distance - maxSafeWalkDistance) / maxSafeWalkDistance;
            }

            //the final elastic force vector by multiplying the normalized direction by the force magnitude
            Vector3 elasticForce = toOrigin.normalized * elasticForceMagnitude;

            rb.AddForce(elasticForce, ForceMode.Acceleration);


            if(distance > maxExtensionOfTheElastic) 
            {
                isDraggingWeight = true;
            }
           
        }
        else
        {
            isDraggingWeight = false;
        }

        if (isDraggingWeight) 
        {
            if (rb.linearVelocity.magnitude > 0.1f)
            {
                rb.linearVelocity *= weightMultiplier;
            }
        }

    }

    private void ElasticForceNOW()
    {
        Vector3 toOrigin = originPlayerPosition - transform.position;
        float distance = toOrigin.magnitude;

        
        if (distance > maxSafeWalkDistance)
        {
            
            float elasticForceMagnitude = Mathf.Clamp((distance - maxSafeWalkDistance) / (maxExtensionOfTheElastic - maxSafeWalkDistance), 0f, 1f) * elasticForceMultiplier;

           
            if (distance > maxExtensionOfTheElastic)
            {
                elasticForceMagnitude = elasticForceMultiplier; 
                isDraggingWeight = true; 
            }

            
            Vector3 elasticForce = toOrigin.normalized * elasticForceMagnitude;

            
            rb.AddForce(elasticForce, ForceMode.Acceleration);
        }
        else
        {
          
            isDraggingWeight = false;
        }

        
        if (isDraggingWeight && rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity *= weightMultiplier;
        }
    }

    private void RandomOscillation()
    {
        float randomX = Mathf.PerlinNoise(Time.time * oscillationSpeed, 0f) * 2f - 1f;
        float randomZ = Mathf.PerlinNoise(0f, Time.time * oscillationSpeed) * 2f - 1f;

        randomOffset = new Vector3(randomX, 0f, randomZ) * randomOscillationStrength;

        rb.AddForce(randomOffset, ForceMode.Acceleration);
    }

    private void CheckIfPlayerStopped()
    {
        
        float playerStop = 0.06f; // max 0.06f not over

        
        if (rb.linearVelocity.magnitude < playerStop)
        {
            if (!hasStopped) 
            {
                //Reset the origin point if the player has stopped
                ResetOrigin();
                hasStopped = true;
            }
            else
            {
                hasStopped = false; 
            }

        }
    }

    private void ResetOrigin()
    {
        originPlayerPosition = transform.position;
    }


    // THROW
    public void Throw()
    {
        if (playerController.GetThrow() > 0)
        {
            if (throwCooldown >= throwDelay)
            {
               // init the throw direction
               Vector3 throwDirection = playerCamera.transform.forward;
               RaycastHit hit;

                // if the ray hits an object, the throw direction is adjusted to aim at the hit location.
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 500f))  
                {
                    Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f, Color.blue, 2f); // DEBUG
                    throwDirection = (hit.point - originThrow.position).normalized;
                }

                Stun stunInstance = Instantiate(stunPrefab, originThrow.position + playerCamera.transform.forward, playerCamera.transform.rotation);

                // set the throw direction of the instantiated stun projectile
                stunInstance.SetDirection(throwDirection);

                throwCooldown = 0;
            }
            else
            {
                throwCooldown += Time.deltaTime;
            }
        }
        else
        {
            throwCooldown = throwDelay;
        }
    }


    // HEALTH
    public void TakeDamage(float damage) 
    {
        health -= damage;

        if (health <= 0)
        {
            // VFX EYES CLOSE
            Debug.Log("Player has died.");
            Destroy(gameObject);
        }
        else if (health <= 50f)
        {
            // VFX POISON
            Debug.Log("Player has 50% of health.");
        }
    }



    private void OnDrawGizmos()
    {
        // Area sicura (verde)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(originPlayerPosition, maxSafeWalkDistance);

        // Area massima estensione elastico (rosso)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(originPlayerPosition, maxExtensionOfTheElastic);

        // Elastico (linea)
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, originPlayerPosition);
        }

        
    }
}
