using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private SoundOutput soundOutput;
        
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private Rigidbody rb;

    public float walkSpeed;
    public float deadzone;

    private float speedChange;
    private float forwardDot;
    private float rightDot;

    private Vector2 moveAmount;
    private Vector2 lookAmount;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    void Update()
    {
        moveAmount = moveAction.ReadValue<Vector2>();
        lookAmount = lookAction.ReadValue<Vector2>();

        forwardDot = Vector3.Dot(transform.forward, new Vector3(moveAmount.x,0,moveAmount.y));
        rightDot = Vector3.Dot(transform.right, new Vector3(moveAmount.x,0,moveAmount.y));
        //Input values into the animator
        animator.SetFloat("xValue", rightDot);
        animator.SetFloat("yValue", forwardDot);

        //SPEED CHANGES
        //Walking Backwards
        if (forwardDot < -deadzone)
        {
            speedChange = 0.7f;
        }
        //Walking normal
        else
        {
            speedChange = 1;
        }

        Movement(speedChange);
        Turn();
    }

    public void Movement(float speedMultiplier)
    {
        Vector3 newPosition = rb.position;
        //X movement
        if (moveAmount.x > deadzone)
        {
            newPosition.x += moveAmount.x * walkSpeed * speedMultiplier * Time.deltaTime;
        }
         //-X movement
        if (moveAmount.x < -deadzone)
        {
            newPosition.x += moveAmount.x * walkSpeed * speedMultiplier * Time.deltaTime;
        }
        //Y movement
        if (moveAmount.y > deadzone)
        {
            newPosition.z += moveAmount.y * walkSpeed * speedMultiplier * Time.deltaTime;
        }
        //-Y movement
        if (moveAmount.y < -deadzone)
        {
            newPosition.z += moveAmount.y * walkSpeed * speedMultiplier * Time.deltaTime;
        }
        rb.MovePosition(newPosition);
    }
    public void Turn()
    {
        if (lookAmount.x > deadzone || lookAmount.x < -deadzone || lookAmount.y > deadzone || lookAmount.y < -deadzone)
        {
            transform.forward = new Vector3(lookAmount.x, 0, lookAmount.y);
        }
    }
}
