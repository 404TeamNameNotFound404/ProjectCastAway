using UnityEngine;


public class Player : MonoBehaviour
{
    // RB
    private Rigidbody rb;

    // COLLIDER

    private CapsuleCollider capsuleCollider;

    //CONTROLS
    private PlayerController playerController;

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

    // DAY-NIGHT CYCLE
    private bool isDaytime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        originPlayerPosition = transform.position;

        isDaytime = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isDaytime) 
        //{
        //    DrunkEffect();
        //}

        CheckIfPlayerStopped();

        DrunkEffect();

    }



    private void FixedUpdate()
    {
        Move();

        
    }


    private void Move()
    {
        Vector2 input = playerController.GetMovement();

        Vector3 movement = new Vector3(input.x, 0, input.y);

        float finalSpeed = isDraggingWeight ? speedWalk * weightMultiplier : speedWalk;

        //Vector3 newPos = rb.position + movement * speedWalk * Time.fixedDeltaTime;

        Vector3 newPos = rb.position + movement * finalSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPos);
    }

    private void DrunkEffect() 
    {
        ElasticForce();
        RandomOscillation();
    }

    private void ElasticForce()
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
        // Soglia minima di movimento per considerare il player fermo
        float playerStop = 0.1f;

        // Controlla se il player si è fermato
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
                hasStopped = false; // Reset quando il player si muove di nuovo
            }

        }
    }

    private void ResetOrigin()
    {
        originPlayerPosition = transform.position;
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
