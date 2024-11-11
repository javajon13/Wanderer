using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackState {Active, Cooldown}
public class PlayerAttack : MonoBehaviour
{
    public GameObject hitbox;
    private bool isFinished = false;
    private PlayerAttackState myState;
    private float attackTimeAmount = 0.5f;

    private int comboCount = 3;
    private int comboIndex;

    //This is a placeholder variable. That's why it's ridiculously long.
    //Remove this when enabling hitbox via animation events.
    private float attackTimeAmountWhenHitboxTriggers = 0.1f;
    private float attackTimeAmountWhenHitboxEnds = 0.15f;

    private float attackTime;

    public void InitializeAttack(bool isFirstAttack = false)
    {
        //set combo index to 0
        //set attackTime to attackTimeAmount for current attack according to combo index
        InputHandler.Instance.RemoveInput("Attack");
        isFinished = false;
        attackTime = 0f;
        myState = PlayerAttackState.Active;
        if(isFirstAttack)
        {
            comboIndex = 1;
        } else
        {
            comboIndex++;
        }
    }

    public void UpdateAttack()
    {
        attackTime += Time.deltaTime;

        if(myState == PlayerAttackState.Active)
        {
            if(attackTime >= attackTimeAmountWhenHitboxTriggers)
            {
                hitbox.SetActive(true);
            } else
            {
                hitbox.SetActive(false);
            }
            if(attackTime >= attackTimeAmountWhenHitboxEnds)
            {
                myState = PlayerAttackState.Cooldown;
            }
        }
        if(myState == PlayerAttackState.Cooldown)
        {
            hitbox.SetActive(false);

            if(comboIndex < comboCount)
            {
                if(InputHandler.Instance.GetInputs(0).Contains("Attack"))
                {
                    InitializeAttack();
                }
            }
            if(attackTime >= attackTimeAmount)
            {
                isFinished = true;
                // if(InputHandler.Instance.GetInputs(0).Contains("Attack"))
                // {
                //     InitializeAttack();
                // } else
                // {
                //     isFinished = true;
                // }
            }
        }
    }

    public bool GetIsFinished()
    {
        return isFinished;
    }
}

