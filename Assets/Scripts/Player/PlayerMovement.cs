using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //stats like moveSpeed, turnSpeed, etc.
    public float moveSpeed = 10f;
    public float rotationSpeed = 60f;

    public Transform playerModel;
    public ColliderTrigger groundDetector;

    private Vector3 movement;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    //read movement inputs
    private void OnMove(InputValue value)
    {
        movement.x = value.Get<Vector2>().x;
        movement.z = value.Get<Vector2>().y;
        movement = movement.normalized;
    }

    //move
    private void Update()
    {
        //Apply movement
        AlignMovement(movement);
        controller.Move(new Vector3(movement.x, 0f, movement.z) * moveSpeed * Time.deltaTime);

        if(movement != Vector3.zero && playerModel != null)
        {
            //Rotate the player model towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z), Vector3.up);
            playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void AlignMovement(Vector3 movementToAlign)
    {
        AlignMovementToCamera(movementToAlign);
        //AlignMovementToSlope(movementToAlign);
    }

    private void AlignMovementToCamera(Vector3 movementInput)
    {
        Transform camera = Camera.main.transform;

        // Get camera's forward and right vectors:
        Vector3 forward = camera.forward;
        Vector3 right = camera.right;

        // Project forward and right vectors on the horizontal plane (y = 0):
        forward.y = 0f;
        right.y = 0f;

        // Normalize vectors:
        forward = forward.normalized;
        right = right.normalized;

        // Calculate desired movement direction relative to camera:
        Vector3 desiredMoveDirection = (forward * movementInput.z) + (right * movementInput.x);
        movement = desiredMoveDirection;
    }

    private void AlignMovementToSlope(Vector3 movementInput)
    {
        //this is supposed to adjust the movement vector based on the slope of the ground where the raycast hits it
        //the debug.log is returning (0, 1, 0) instead of the slope for some reason
        Ray ray = new Ray(transform.position, Vector3.down);
        if(groundDetector.isTriggered)
        {
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 10f))
            {
                Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                movementInput.x *= 1f - Mathf.Abs(slopeRotation.z);
                movementInput.z *= 1f - Mathf.Abs(slopeRotation.x);

                /*if(groundDetector.isTriggered)
                {
                    movementInput.y -= 10000f;
                }*/
                Debug.Log(slopeRotation);
                movement = movementInput;
            }
        }
    }
}
