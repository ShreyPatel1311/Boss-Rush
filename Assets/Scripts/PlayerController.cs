using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;

    private Transform cameraTransform;
    private CharacterController characterController;
    private Animator animator;
    private InputReader inputReader;

    private Vector3 movement;

    private void Start()
    {
        cameraTransform = Camera.main?.transform;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputReader = GetComponent<InputReader>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing on this GameObject.");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject.");
        }
        if (inputReader == null)
        {
            Debug.LogError("InputReader component is missing on this GameObject.");
        }
    }

    private void Update()
    {
        if (inputReader == null || characterController == null || animator == null)
        {
            return;
        }

        Vector3 movementInput = CalculateMovement();
        movement = movementInput * moveSpeed * Time.deltaTime;

        characterController.Move(movement);

        UpdateAnimator(movementInput);

        if (movementInput != Vector3.zero)
        {
            SetDirection(movementInput, Time.deltaTime);
        }
    }

    private void SetDirection(Vector3 playerInput, float deltaTime)
    {
        if (playerInput.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, deltaTime * rotateSpeed);
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * inputReader.movement.y + right * inputReader.movement.x;
    }

    private void UpdateAnimator(Vector3 movementInput)
    {
        float movementMagnitude = movementInput.magnitude;
        animator.SetFloat("Movement", movementMagnitude, 0.1f, Time.deltaTime);
    }
}
