using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDodge : MonoBehaviour
{
    public float dodgeDistance = 5f;
    public float dodgeCooldown = 1f;
    public bool canDodge = true;
    public bool dodgeComplete = true;

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void UpdateDodge(Vector2 movementVector)
    {
        if (canDodge && dodgeComplete == false)
        {
            PerformDodge(movementVector);
            InputHandler.Instance.RemoveInput("Defend");
        }
    }

    void PerformDodge(Vector2 movementVector)
    {
        if (canDodge)
        {
            StartCoroutine(Dodge(movementVector));
        }
    }

    IEnumerator Dodge(Vector2 movementVector)
    {
        canDodge = false;

        Vector3 dodgeDirection = new Vector3(movementVector.x, 0, movementVector.y).normalized;
        Vector3 dodgeTarget = transform.position + dodgeDirection * dodgeDistance;

        float elapsedTime = 0f;
        while (elapsedTime < 0.2f) // Dodge duration
        {
            controller.Move(dodgeDirection * (dodgeDistance / 0.2f) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dodgeComplete = true;

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
}