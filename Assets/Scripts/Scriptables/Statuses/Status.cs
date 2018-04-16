using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : ScriptableObject
{
    public enum StatusType
    {
        BUFF,
        DEBUFF
    }

    public string statusName;
    public int duration;
    public StatusType type;
    public GameObject effect;

    public bool isDone = false;

    public IEnumerator Apply(Fighter target)
    {
        yield return ApplyEffect(target);
        LowerDuration();
    }

    public abstract IEnumerator ApplyEffect(Fighter target);

    public void LowerDuration()
    {
        duration--;
        if(duration == 0)
        {
            isDone = true;
        }
    }
}
