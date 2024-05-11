using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private static InputHandler instance;

    public static InputHandler Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    [SerializeField]private float defaultBufferTime = 0.2f;

    [SerializeField]private List<ActiveInputList> activePlayers = new List<ActiveInputList>();

    public List<string> GetInputs(int playerNumber)
    {
        List<string> returnInputs = new List<string>();
        for(int i = 0; i < activePlayers[playerNumber].activeInputs.Count; i++)
        {
            returnInputs.Add(activePlayers[playerNumber].activeInputs[i].GetInputName());
        }
        return returnInputs;
    }
    public List<string> GetHeldInputs(int playerNumber)
    {
        List<string> returnInputs = new List<string>();
        for(int i = 0; i < activePlayers[playerNumber].heldInputs.Count; i++)
        {
            returnInputs.Add(activePlayers[playerNumber].heldInputs[i].GetInputName());
        }
        return returnInputs;
    }

    public void CheckInputs()
    {
        //For each player
        for (int i = 0; i < activePlayers.Count; i++)
        {
            //For each input for each player
            for (int j = 0; j < activePlayers[i].playerInputs.buttons.Count; j++)
            {
                //If an input is being pressed
                if(activePlayers[i].playerInputs.buttons[j].inputAction.triggered)
                {
                    //Create new input list item and add it to the player's active input list
                    ActiveInputsItem newActiveInputsItem = new ActiveInputsItem();
                    newActiveInputsItem.SetInputName(activePlayers[i].playerInputs.buttons[j].buttonName);
                    if(activePlayers[i].playerInputs.buttons[j].customBufferTime != 0f)
                    {
                        newActiveInputsItem.SetTimer(activePlayers[i].playerInputs.buttons[j].customBufferTime);
                    } else
                    {
                        newActiveInputsItem.SetTimer(defaultBufferTime);
                    }
                    activePlayers[i].activeInputs.Add(newActiveInputsItem);
                }
                //If an input was pressed this frame
                if(activePlayers[i].playerInputs.buttons[j].inputAction.phase == InputActionPhase.Started)
                {
                    //Create new input list item and add it to input buffer
                    ActiveInputsItem newActiveInputsItem = new ActiveInputsItem();
                    newActiveInputsItem.SetInputName(activePlayers[i].playerInputs.buttons[j].buttonName);
                    bool canAdd = true;
                    //If there are currently no inputs in the buffer, then add the new input to index 0
                    if(activePlayers[i].heldInputs.Count == 0)
                    {
                        activePlayers[i].heldInputs.Add(newActiveInputsItem);
                        return;
                    }
                    //If there are currently some inputs in the buffer, and this input doesn't already exist in the buffer,
                    //then add the new input to the end
                    for(int k = 0; k < activePlayers[i].heldInputs.Count; k++)
                    {
                        if(activePlayers[i].heldInputs[k].GetInputName() == newActiveInputsItem.GetInputName())
                        {
                            canAdd = false;
                        }
                        if(k == activePlayers[i].heldInputs.Count - 1 && canAdd)
                        {
                            activePlayers[i].heldInputs.Add(newActiveInputsItem);
                        }
                    }
                }
                if(activePlayers[i].playerInputs.buttons[j].inputAction.phase == InputActionPhase.Waiting)
                {
                    for(int k = 0; k < activePlayers[i].heldInputs.Count; k++)
                    {
                        if(activePlayers[i].heldInputs[k].GetInputName() == activePlayers[i].playerInputs.buttons[j].buttonName)
                        {
                            activePlayers[i].heldInputs.Remove(activePlayers[i].heldInputs[k]);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < activePlayers.Count; i++)
        {
            for (int j = 0; j < activePlayers[i].activeInputs.Count; j++)
            {
                if(activePlayers[i].activeInputs[j].GetTimer() > 0f)
                {
                    activePlayers[i].activeInputs[j].CountTimer(Time.deltaTime);
                } else
                {
                    activePlayers[i].activeInputs.Remove(activePlayers[i].activeInputs[j]);
                }
            }
        }
    }

    public void OnEnable()
    {
        for (int i = 0; i < activePlayers.Count; i++)
        {
            for (int j = 0; j < activePlayers[i].playerInputs.buttons.Count; j++)
            {
                activePlayers[i].playerInputs.buttons[j].inputAction.Enable();
            }
        }
    }
    public void OnDisable()
    {
        for (int i = 0; i < activePlayers.Count; i++)
        {
            for (int j = 0; j < activePlayers[i].playerInputs.buttons.Count; j++)
            {
                activePlayers[i].playerInputs.buttons[j].inputAction.Disable();
            }
        }
    }
}

[System.Serializable]
public class ActiveInputList
{
    public PlayerInputList playerInputs;
    public List<ActiveInputsItem> activeInputs = new List<ActiveInputsItem>();
    public List<ActiveInputsItem> heldInputs = new List<ActiveInputsItem>();
}

[System.Serializable]
public class ActiveInputsItem
{
    [SerializeField]string inputName;
    float timer;

    public string GetInputName()
    {
        return inputName;
    }

    public void SetInputName(string newName)
    {
        inputName = newName;
    }
    public void SetTimer(float newTimer)
    {
        timer = newTimer;
    }

    public void CountTimer(float countNumber)
    {
        timer -= countNumber;
    }
    public float GetTimer()
    {
        return timer;
    }
}