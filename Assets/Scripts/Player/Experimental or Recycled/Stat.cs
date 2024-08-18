using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public string statName;
    public float baseValue;

    public List<StatModifier> modifiers = new List<StatModifier>();

    public float GetValue()
    {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x.value);
        return finalValue;
    }
    public void RunTimers()
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].timer -= Time.deltaTime;
            if(modifiers[i].timer <= 0f) RemoveModifier(modifiers[i]);
        }
    }
    public void AddModifier(StatModifier modifier)
    {
        if(modifier.value != 0f)
        {
            modifiers.Add(modifier);
        }
    }
    public void RemoveModifier(StatModifier modifier)
    {
        if(modifier.value != 0f)
        {
            modifiers.Remove(modifier);
        }
    }
}