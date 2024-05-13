using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float smoothTime = 0.05f;
    public float gravityMultiplier = 1f;
    public float jumpForce = 5f;

    [SerializeField] InputActionMap inputActions;

    CharacterController characterController;

    float currentVelocity, velocity;
    float gravity = -9.81f;
    Vector2 moveInput;
    Vector3 movement;
    bool isSprinting, isJumping;

    void OnEnable()
    {
        characterController = GetComponent<CharacterController>();

        inputActions["Move"].performed += OnMove;
        inputActions["Move"].canceled += OnMove;

        inputActions["Sprint"].performed += OnSprint;
        inputActions["Sprint"].canceled += OnSprint;

        inputActions["Jump"].performed += OnJump;

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions["Move"].performed -= OnMove;
        inputActions["Move"].canceled -= OnMove;

        inputActions["Sprint"].performed -= OnSprint;
        inputActions["Sprint"].canceled -= OnSprint;

        inputActions["Jump"].performed -= OnJump;

        inputActions.Disable();
    }

    void Update()
    {
       ApplyRotation();
       ApplyMovement();
       ApplyGravity();

    }

    void ApplyRotation()
    {
        if(moveInput.sqrMagnitude == 0) return;
        var targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    void ApplyMovement()
    {
        float speed = isSprinting ? sprintSpeed : moveSpeed;
        characterController.Move(movement * Time.deltaTime * speed);
    }

    void ApplyGravity()
    {
        if(IsGrounded() && velocity < 0.0f) velocity = -1.0f;
        velocity += gravity * gravityMultiplier * Time.deltaTime;
        movement.y += velocity;
    }

    bool IsGrounded() {return characterController.isGrounded;}

    #region InputAction Methods

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        movement = new Vector3(moveInput.x, 0f, moveInput.y);
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed || !IsGrounded()) return;
        velocity += jumpForce;
    }

    #endregion
}
