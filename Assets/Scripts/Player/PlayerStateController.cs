using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState {Neutral, Attacking, Parrying, Dodging}
public class PlayerStateController : MonoBehaviour
{
    PlayerMovement movement;
    PlayerJump jump;
    PlayerAttack attack;
    PlayerParry parry;
    PlayerDodge dodge;

    PlayerState myState = PlayerState.Neutral;
    public ColliderTrigger groundDetector;

    public Animator animator;

    private void Start()
    {
        movement = transform.GetComponent<PlayerMovement>();
        jump = transform.GetComponent<PlayerJump>();
        attack = transform.GetComponent<PlayerAttack>();
        parry = transform.GetComponent<PlayerParry>();
        dodge = transform.GetComponent<PlayerDodge>();
    }

    //Inputs
    //read movement inputs
    private Vector2 movementVector;
    private Vector2 previousMovementVector;

    bool canUpdateMovement = true;
    private void OnMove(InputValue value)
    {
        // TC: store the movement vector in one single get and normalize call
        movementVector = value.Get<Vector2>();

        //movement.x = value.Get<Vector2>().x;
        //movement.z = value.Get<Vector2>().y;
        //movementVector = movementVector.normalized;
    }

    private void Update()
    {
        //Update movement
        if(canUpdateMovement) previousMovementVector = movementVector;

        if(myState == PlayerState.Neutral)
        {
            canUpdateMovement = true;
            movement.UpdateMovement(previousMovementVector);

            if(animator != null)
            {
                if(movementVector.magnitude != 0 && jump.groundDetector.isTriggered)
                {
                    animator.SetFloat("MoveSpeed", movementVector.magnitude);
                    if(movementVector.magnitude > 0.8f)
                    {
                        animator.SetBool("IsRunning", true);
                    } else
                    {
                        animator.SetBool("IsRunning", false);
                        animator.SetBool("IsWalking", true);
                    }
                } else
                {
                    animator.SetBool("IsRunning", false);
                    animator.SetBool("IsWalking", false);
                }
            }

            jump.UpdateJump();
            if(InputHandler.Instance.GetInputs(0).Contains("Attack"))
            {
                myState = PlayerState.Attacking;
                attack.InitializeAttack(true);
            }
            if(InputHandler.Instance.GetInputs(0).Contains("Defend"))
            {
                InputHandler.Instance.RemoveInput("Defend");
                if(previousMovementVector.x == 0f && previousMovementVector.y == 0f)
                {
                    myState = PlayerState.Parrying;
                } else
                {
                    if(dodge.dodgeComplete == true && dodge.canDodge == true)
                    {
                        jump.yVelocity = 0f;
                        dodge.dodgeComplete = false;
                        myState = PlayerState.Dodging;
                    }
                }
            }
        }
        if(myState == PlayerState.Attacking)
        {
            canUpdateMovement = false;
            if(groundDetector.isTriggered)
            {
                previousMovementVector = Vector2.zero;
            } else
            {
                jump.Hover();
            }
            movement.UpdateMovement(previousMovementVector);
            jump.UpdateJump();
            attack.UpdateAttack();
            if(attack.GetIsFinished())
            {
                myState = PlayerState.Neutral;
            }
        }
        if(myState == PlayerState.Parrying)
        {
            Debug.Log("Parrying");
            myState = PlayerState.Neutral;
        }
        if(myState == PlayerState.Dodging)
        {
            canUpdateMovement = false;
            Vector2 tempMovementVector = new Vector2(movement.CalculateMovementVector(movementVector).x, movement.CalculateMovementVector(movementVector).z);
            movement.RotatePlayerGFX(movement.CalculateMovementVector(previousMovementVector), true);
            dodge.UpdateDodge(tempMovementVector);
            if(dodge.dodgeComplete) myState = PlayerState.Neutral;
        }
    }
}
