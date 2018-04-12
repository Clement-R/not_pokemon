using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "NotPokemon/Ability", order = 1)]
public class Ability : ScriptableObject
{
    public enum AbilityType
    {
        ATTACK,
        SUPPORT
    }

    public string abilityName = "Move 1";
    public AbilityType type = AbilityType.ATTACK;
    
}
