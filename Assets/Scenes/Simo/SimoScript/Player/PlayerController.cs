using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    private Controls inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction shootAction;
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

        shootAction = inputActions.FindAction("Shoot");
        shootAction.performed += OnShoot;

        interactAction = inputActions.FindAction("Interact");
        interactAction.performed += OnInteract;

        moveAction.Enable();
        lookAction.Enable();
        shootAction.Enable(); 
        interactAction.Enable();


    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;

        lookAction.performed -= OnLook;

        shootAction.performed -= OnShoot;

        interactAction.performed += OnInteract;

        moveAction.Disable();
        lookAction.Disable();
        shootAction.Disable();
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

    public float GetShoot()
    {
        return shootAction.ReadValue<float>();

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

    private void OnShoot(InputAction.CallbackContext context)
    {
        bool shootInput = context.ReadValueAsButton();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        bool InteractInput = context.ReadValueAsButton();
    }


}
