﻿using System.Collections;
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

    public enum AbilityTarget
    {
        SINGLE,
        MULTI
    }

    public enum AbilityAttackType
    {
        FIRE,
        ELECTRIC,
        WATER,
    }

    public string abilityName = "Move 1";
    public AbilityType type = AbilityType.ATTACK;

    public int damage = 50;

}