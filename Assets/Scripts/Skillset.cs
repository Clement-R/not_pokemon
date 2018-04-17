using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillset : ScriptableObject {
    public Ability[] _abilities = new Ability[4];

    public void ChangeAbility(int index, Ability ability)
    {
        _abilities[index] = ability;
    }

    public Ability GetAbility(int index)
    {
        return _abilities[index];
    }
}
