using UnityEngine;
using UnityEngine.InputSystem;

public class LifeboatController : MonoBehaviour
{
    private Controls inputActions;
    private InputAction moveAction;
    private InputAction interactAction;

    private void Awake()
    {
        inputActions = new Controls();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        inputActions.Enable();

        moveAction = inputActions.FindAction("Move");
        moveAction.performed += OnMove;

        
        interactAction = inputActions.FindAction("Interact");
        interactAction.performed += OnInteract;

        moveAction.Enable();     
        interactAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;

        interactAction.performed += OnInteract;

        moveAction.Disable();
  
        interactAction.Disable();
    }

    public Vector2 GetMovement()
    {
        return moveAction.ReadValue<Vector2>();

    }

    public float GetInteract()
    {
        return interactAction.ReadValue<float>();  //float
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        float interactInput = context.ReadValue<float>();  // float // .ReadValue //ReadValueAsButton();
    }


}
