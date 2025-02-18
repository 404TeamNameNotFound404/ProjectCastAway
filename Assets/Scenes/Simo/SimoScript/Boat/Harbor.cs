using UnityEngine;

public class Harbor : MonoBehaviour
{
    public bool canPlayerExit {  get; private set; }

    public BoxCollider BoxCollider { get; private set; }

    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.enabled = true;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BuoyantObject")) 
        {
            canPlayerExit = true;
   
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BuoyantObject"))
        {
            canPlayerExit = false;
           
        }
    }

}
