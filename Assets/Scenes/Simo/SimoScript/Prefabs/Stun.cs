using UnityEngine;

public class Stun : MonoBehaviour
{

    //RB
    private Rigidbody rb;

    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifeTime = 5f;

    private Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    

    private void FixedUpdate()
    {      
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime); 

        lifeTime -= Time.fixedDeltaTime;

        if(lifeTime < 0) 
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 newDirection)  
    {      
        direction = newDirection.normalized; 
        transform.forward = direction; // rotate the object towards the direction of the throw
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("EnemyHit");
            Destroy(gameObject);

        }
        else
        {
            Debug.Log("HitSmething");
            Destroy(gameObject);
        }


    }
}
