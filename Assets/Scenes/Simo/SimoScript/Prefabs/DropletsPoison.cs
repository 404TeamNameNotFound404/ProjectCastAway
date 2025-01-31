using Unity.VisualScripting;
using UnityEngine;

public class DropletsPoison : Poison
{
    // RB
    private Rigidbody rb;

    [SerializeField] private float speedDrop = 5f;
    [SerializeField] private float lifeTime = 5f;

    private Vector3 direction = Vector3.down;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject); // Distruggi la goccia dopo che il tempo è scaduto
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speedDrop * Time.fixedDeltaTime);

    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageble damageable = collision.gameObject.GetComponent<IDamageble>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    
   


}
