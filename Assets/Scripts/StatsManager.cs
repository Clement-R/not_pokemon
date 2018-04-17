using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

    private Fighter _fighter;

	void Awake ()
    {
        _fighter = GetComponent<Fighter>();
    }

    public int ComputeDexterity()
    {
        int dexterity = 0;

        // Apply stuff dexterity
        if (_fighter.frame != null)
            dexterity += _fighter.frame.dexterity;

        if (_fighter.pilot != null)
            dexterity += _fighter.pilot.dexterity;

        if (_fighter.leftShoulder != null)
            dexterity += _fighter.leftShoulder.dexterity;

        if (_fighter.rightShoulder != null)
            dexterity += _fighter.rightShoulder.dexterity;

        if (_fighter.leftArm != null)
            dexterity += _fighter.leftArm.dexterity;

        if (_fighter.rightArm != null)
            dexterity += _fighter.rightArm.dexterity;

        // TODO : Apply statuses (buff/debuff)
        foreach (Status buff in _fighter.buffs.FindAll(e => e.type == Status.StatusType.MODIFICATOR))
        {
            ModificatorStatus modificator = (ModificatorStatus)buff;
            dexterity += modificator.dexterity;
        }

        foreach (Status debuff in _fighter.debuffs.FindAll(e => e.type == Status.StatusType.MODIFICATOR))
        {
            ModificatorStatus modificator = (ModificatorStatus)debuff;
            dexterity += modificator.dexterity;
        }

        return dexterity;
    }

    public int ComputeStrength()
    {
        int strength = 0;

        // Apply stuff strength
        if(_fighter.frame != null)
            strength += _fighter.frame.strength;

        if (_fighter.pilot != null)
            strength += _fighter.pilot.strength;

        if (_fighter.leftShoulder != null)
            strength += _fighter.leftShoulder.strength;

        if (_fighter.rightShoulder != null)
            strength += _fighter.rightShoulder.strength;

        if (_fighter.leftArm != null)
            strength += _fighter.leftArm.strength;

        if (_fighter.rightArm != null)
            strength += _fighter.rightArm.strength;

        // TODO : Apply statuses (buff/debuff)

        return strength;
    }

    public int ComputeAccuracy()
    {
        int accuracy = 0;

        // Apply stuff accuracy
        if (_fighter.frame != null)
            accuracy += _fighter.frame.accuracy;

        if (_fighter.pilot != null)
            accuracy += _fighter.pilot.accuracy;

        if (_fighter.leftShoulder != null)
            accuracy += _fighter.leftShoulder.accuracy;

        if (_fighter.rightShoulder != null)
            accuracy += _fighter.rightShoulder.accuracy;

        if (_fighter.leftArm != null)
            accuracy += _fighter.leftArm.accuracy;

        if (_fighter.rightArm != null)
            accuracy += _fighter.rightArm.accuracy;

        // TODO : Apply statuses (buff/debuff)

        return accuracy;
    }
}
