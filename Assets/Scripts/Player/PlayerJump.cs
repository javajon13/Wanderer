using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public float jumpVelocity;

    public float gravity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private float yVelocity;
    private bool canJump = true;

    private CharacterController controller;
    public ColliderTrigger groundDetector;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(InputHandler.Instance.GetInputs(0).Contains("Jump"))
        {
            if(groundDetector.isTriggered && canJump)
            {
                //Execute jump
                yVelocity = jumpVelocity;
                canJump = false;
            }
        }

        //Falling at different heights and gravity
        if(!groundDetector.isTriggered)
        {
            if(yVelocity < 0f)
            {
                yVelocity -= gravity * fallMultiplier * Time.deltaTime;
            } else if(yVelocity > 0f && !InputHandler.Instance.GetHeldInputs(0).Contains("Jump"))
            {
                yVelocity -= gravity * lowJumpMultiplier * Time.deltaTime;
            } else
            {
                yVelocity -= gravity * Time.deltaTime;
            }
        } else
        {
            canJump = true;
        }

        controller.Move(Vector3.up * yVelocity * Time.deltaTime);
    }
}
