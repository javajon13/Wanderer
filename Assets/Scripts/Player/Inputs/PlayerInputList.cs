using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Inputs", fileName = "New Inputs")]
public class PlayerInputList : ScriptableObject
{
    public List<Button> buttons = new List<Button>();
}

[System.Serializable]
public class Button
{
    public string buttonName;
    public float customBufferTime;
    public InputAction inputAction;
}
