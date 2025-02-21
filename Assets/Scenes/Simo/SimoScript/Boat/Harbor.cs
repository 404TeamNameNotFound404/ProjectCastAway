using UnityEngine;

public class Harbor : MonoBehaviour
{
    public bool canPlayerExit { get; set; }

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
            Debug.Log("PRESS E TO EXIT THE BOAT ");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BuoyantObject"))
        {
            canPlayerExit = false;
        }
    }

    public void DisableCollider()
    {
        canPlayerExit = false;
        BoxCollider.enabled = false;
    }

    public void EnableCollider()
    {
        canPlayerExit = true;
        BoxCollider.enabled = true;
    }

}
