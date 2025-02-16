using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls; //Input Manager
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;

    public Vector2 movementInput; //Stores our vector2 from input manager for wasd
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;
    

    public float moveAmount;
    public float verticalInput; 
    public float horizontalInput;

    public bool sprintInput;
    public bool jumpInput;
    
    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();   
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprinting.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprinting.canceled += i => sprintInput = false;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
           // playerControls.PlayerActions.Jump.canceled += i => jumpInput = false;
        }

        playerControls.Enable();


    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
        //Handle action input
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x; 

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;


        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }
    private void HandleSprintingInput()
    {
        if (sprintInput && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;

        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }
    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerLocomotion.HandleJump();
        }
    }
}
