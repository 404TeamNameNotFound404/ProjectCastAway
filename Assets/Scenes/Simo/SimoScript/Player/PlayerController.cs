using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    private Controls inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction throwAction;
    private InputAction interactAction;


    private void Awake()
    {
        inputActions = new Controls();
    }


    private void OnEnable()
    {
        inputActions.Enable();

        moveAction = inputActions.FindAction("Move");
        moveAction.performed += OnMove;

        lookAction = inputActions.FindAction("Look");
        lookAction.performed += OnLook;

        throwAction = inputActions.FindAction("Throw");
        throwAction.performed += OnThrow;

        interactAction = inputActions.FindAction("Interact");
        interactAction.performed += OnInteract;

        moveAction.Enable();
        lookAction.Enable();
        throwAction.Enable(); 
        interactAction.Enable();


    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;

        lookAction.performed -= OnLook;

        throwAction.performed -= OnThrow;

        interactAction.performed += OnInteract;

        moveAction.Disable();
        lookAction.Disable();
        throwAction.Disable();
        interactAction.Disable();
    }

    public Vector2 GetMovement() 
    {
        return moveAction.ReadValue<Vector2>();

    }

    public Vector2 GetMouseLook()
    {
       Vector2 mousePos = inputActions.Player.Look.ReadValue<Vector2>();
       return mousePos;
       
    }

    public float GetThrow()
    {
        return throwAction.ReadValue<float>();

    }

    public float GetInteract()
    {
        return interactAction.ReadValue<float>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        bool shootInput = context.ReadValueAsButton();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        bool InteractInput = context.ReadValueAsButton();
    }


}
