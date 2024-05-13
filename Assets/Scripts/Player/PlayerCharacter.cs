using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]private CharacterStats myStats;
    private Rigidbody rb;
    [SerializeField]private Transform playerModel;

    private CharacterState myState = CharacterState.neutral;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public CharacterStats GetCharacterStats()
    {
        return myStats;
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
    public Transform GetPlayerModel()
    {
        return playerModel;
    }
    public CharacterState GetCharacterState()
    {
        return myState;
    }
    public void SetCharacterState(CharacterState newState)
    {
        myState = newState;
    }
}
