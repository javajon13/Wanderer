using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]private List<PlayerCharacter> playerCharacters = new List<PlayerCharacter>();
    private PlayerCharacter targetCharacter;

    private int targetIndex = 0;

    public delegate void SwitchAction();
    public static event SwitchAction m_OnSwitch;

    [SerializeField]private Transform cameraFollowTarget;

    private void Awake()
    {
        targetCharacter = playerCharacters[0];
        SetTargetCharacter();
    }

    private void SetTargetCharacter()
    {
        targetCharacter = playerCharacters[targetIndex];

        if(cameraFollowTarget != null)
        {
            cameraFollowTarget.parent = targetCharacter.transform;
            //cameraFollowTarget.position = targetCharacter.transform.position;
        }
    }

    private void Update()
    {
        cameraFollowTarget.position = Vector3.Lerp(cameraFollowTarget.position, targetCharacter.transform.position, 0.1f);
    }

    public PlayerCharacter GetTargetCharacter()
    {
        return targetCharacter;
    }

    public void OnSwitchNext(InputAction.CallbackContext context)
    {
        if(playerCharacters.Count < 2) return;
        if(context.phase == InputActionPhase.Started)
        {
            SwitchTargetCharacter(true);
        }
    }
    public void OnSwitchPrevious(InputAction.CallbackContext context)
    {
        if(playerCharacters.Count < 2) return;
        if(context.phase == InputActionPhase.Started)
        {
            SwitchTargetCharacter(false);
        }
    }

    public void SwitchTargetCharacter(bool next)
    {
        if(next) targetIndex += 1;
        if(!next) targetIndex -= 1;
        if(targetIndex > playerCharacters.Count - 1) targetIndex = 0;
        if(targetIndex < 0) targetIndex = playerCharacters.Count - 1;

        SetTargetCharacter();

        if(m_OnSwitch != null)
        {
            m_OnSwitch();
        }
    }
}
