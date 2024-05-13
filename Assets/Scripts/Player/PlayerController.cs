using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterState{neutral, attacking, hit, ko};
public class PlayerController : MonoBehaviour
{
    //references
    [SerializeField]private PlayerManager playerManager;
    private Transform playerModel;
    private Rigidbody rb;
    [SerializeField]private LayerMask groundLayer;
    //movement variables
    private Vector3 movement;
    [SerializeField]private float moveSpeed = 10f;
    [SerializeField]private float rotationSpeed = 20f;
    [SerializeField]private float maxForce = 1f;
    [SerializeField]private float airMovementFactor = 1f;
    //jump variables
    [SerializeField]private bool isGrounded = true;
    [SerializeField]private bool isJumpPressed = false;
    [SerializeField]private bool isJumpStarted = false;
    [SerializeField]private float jumpHeight;
    private bool isFalling = false;
    [SerializeField]float fallFactor = 2f;

    private void OnEnable()
    {
        PlayerManager.m_OnSwitch += UpdatePlayerCharacter;
    }
    private void OnDisable()
    {
        PlayerManager.m_OnSwitch -= UpdatePlayerCharacter;
    }

    private void UpdatePlayerCharacter()
    {
        rb = playerManager.GetTargetCharacter().GetRigidbody();
        playerModel = playerManager.GetTargetCharacter().GetPlayerModel();
    }

    private void Start()
    {
        UpdatePlayerCharacter();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement.x = context.ReadValue<Vector2>().x;
        movement.z = context.ReadValue<Vector2>().y;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            isJumpPressed = true;
            isJumpStarted = true;

            StartCoroutine(CloseStartJump());
        }
        if(context.phase == InputActionPhase.Performed)
        {
            isJumpPressed = true;
        } else
        {
            isJumpPressed = false;
        }
    }

    private IEnumerator CloseStartJump()
    {
        yield return new WaitForSeconds(0.1f);
        isJumpStarted = false;
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
        HandleFall();
    }

    private void HandleMovement()
    {
        //find current velocity
        if(rb == null) return;
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(movement.x, 0, movement.z);
        targetVelocity *= moveSpeed;
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;
        //currentVelocity = Quaternion.LookRotation(cameraForward) * currentVelocity;
        targetVelocity = Quaternion.LookRotation(cameraForward) * targetVelocity;

        //calculate force
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);
        velocityChange = AlignMovementToSlope(velocityChange);

        //set force limits
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        //apply force
        if(isGrounded)
        {
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        } else
        {
            rb.AddForce(velocityChange * airMovementFactor);
        }

        //rotate player model to movement
        RotateModelToMovement(targetVelocity);
    }

    private Vector3 AlignMovementToSlope(Vector3 movement)
    {
        Ray ray = new Ray(rb.transform.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 1.5f))
        {
            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Vector3 newMovement = slopeRotation * movement;

            return newMovement;

            /*movement.x *= 1f - Mathf.Abs(slopeRotation.z);
            movement.z *= 1f - Mathf.Abs(slopeRotation.x);*/
        }
        //Debug.Log(movement);
        return movement;
    }

    private void RotateModelToMovement(Vector3 movement)
    {
        if(playerModel == null) return;
        movement = movement.normalized;
        if(movement.x != 0 || movement.z != 0)
        {
            // Rotate the player model towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z), Vector3.up);
            playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void CheckGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 1f;

        bool middleGrounded = false;
        if(Physics.Raycast(new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z), Vector3.down, out hit, raycastDistance, groundLayer))
        {
            middleGrounded = true;
        } else
        {
            middleGrounded = false;
        }
        bool lowerRightGrounded = false;
        if(Physics.Raycast(new Vector3(rb.transform.position.x + 0.4f, rb.transform.position.y, rb.transform.position.z - 0.4f), Vector3.down, out hit, raycastDistance, groundLayer))
        {
            lowerRightGrounded = true;
        } else
        {
            lowerRightGrounded = false;
        }
        bool upperRightGrounded = false;
        if(Physics.Raycast(new Vector3(rb.transform.position.x + 0.4f, rb.transform.position.y, rb.transform.position.z + 0.4f), Vector3.down, out hit, raycastDistance, groundLayer))
        {
            upperRightGrounded = true;
        } else
        {
            upperRightGrounded = false;
        }
        bool lowerLeftGrounded = false;
        if(Physics.Raycast(new Vector3(rb.transform.position.x - 0.4f, rb.transform.position.y, rb.transform.position.z - 0.4f), Vector3.down, out hit, raycastDistance, groundLayer))
        {
            lowerLeftGrounded = true;
        } else
        {
            lowerLeftGrounded = false;
        }
        bool upperLeftGrounded = false;
        if(Physics.Raycast(new Vector3(rb.transform.position.x - 0.4f, rb.transform.position.y, rb.transform.position.z + 0.4f), Vector3.down, out hit, raycastDistance, groundLayer))
        {
            upperLeftGrounded = true;
        } else
        {
            upperLeftGrounded = false;
        }
        if(middleGrounded || lowerRightGrounded || upperRightGrounded || lowerLeftGrounded || upperLeftGrounded)
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
    }

    float jumpForce = 0f;
    private void HandleJump()
    {
        // Handle player jumps here
        // Use jumpForce to apply upward force to the player's rb
        if(rb == null) return;
        if(isGrounded && isJumpStarted)
        {
            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y));

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        HandleFall();
    }
    
    private void HandleFall()
    {
        if(rb == null) return;
        if(rb.velocity.y < jumpForce * 0.75f && !isGrounded)
        {
            isFalling = true;
        } else
        {
            isFalling = false;
        }
        if(isFalling || (!isJumpPressed && !isGrounded))
        {
            rb.AddForce(Vector3.up * Physics.gravity.y * fallFactor);
        }
    }
}
