using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character/Stats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField]private float multiplier;
    [SerializeField]private List<Stat> stats = new List<Stat>();

    public List<Stat> GetStats()
    {
        List<Stat> returnStats = stats;
        for (int i = 0; i < returnStats.Count; i++)
        {
            returnStats[i].baseValue *= multiplier;
        }
        return returnStats;
    }
}
