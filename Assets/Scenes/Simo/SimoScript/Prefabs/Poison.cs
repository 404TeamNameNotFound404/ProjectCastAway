using UnityEngine;

public class Poison : MonoBehaviour
{
    
    [SerializeField] protected float damage = 50f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        IDamageble damageable = other.gameObject.GetComponent<IDamageble>();
        
        if(damageable != null) 
        {
            damageable.TakeDamage(damage);
        }
    }
}
