using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState {Neutral, Attacking, Parrying}
public class PlayerStateController : MonoBehaviour
{
    PlayerMovement movement;
    PlayerJump jump;
    PlayerAttack attack;
    PlayerParry parry;

    PlayerState myState = PlayerState.Neutral;

    private void Start()
    {
        movement = transform.GetComponent<PlayerMovement>();
        jump = transform.GetComponent<PlayerJump>();
        attack = transform.GetComponent<PlayerAttack>();
        parry = transform.GetComponent<PlayerParry>();
    }

    //Inputs
    //read movement inputs
    private Vector2 movementVector;
    private void OnMove(InputValue value)
    {
        // TC: store the movement vector in one single get and normalize call
        movementVector = value.Get<Vector2>();

        //movement.x = value.Get<Vector2>().x;
        //movement.z = value.Get<Vector2>().y;
        movementVector = movementVector.normalized;
    }

    private void Update()
    {
        if(myState == PlayerState.Neutral)
        {
            movement.UpdateMovement(movementVector);
            jump.UpdateJump();
            if(InputHandler.Instance.GetInputs(0).Contains("Attack")) myState = PlayerState.Attacking;
            if(InputHandler.Instance.GetInputs(0).Contains("Parry")) myState = PlayerState.Parrying;
        }
        if(myState == PlayerState.Attacking)
        {
            Debug.Log("Attacking");
            myState = PlayerState.Neutral;
        }
        if(myState == PlayerState.Parrying)
        {
            Debug.Log("Parrying");
            myState = PlayerState.Neutral;
        }
    }
}
