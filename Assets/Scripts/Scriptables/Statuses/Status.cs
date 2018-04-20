using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public abstract class Status : ScriptableObject
{
    public enum StatusType
    {
        BUFF,
        DEBUFF,
        MODIFICATOR
    }

    public string statusName;
    public int duration;
    public StatusType type;
    public GameObject effect;

    public bool isDone = false;

    public IEnumerator Apply(Fighter target)
    {
        yield return target.StartCoroutine(ApplyEffect(target));
        
        while (!CombatLogManager.instance.doneDisplaying)
        {
            yield return null;

        }

        LowerDuration();
    }

    public abstract IEnumerator ApplyEffect(Fighter target);

    public void DisplayLog(string log)
    {
        EventManager.TriggerEvent(EventList.DISPLAY_TEXT.ToString(), new { text = log });
    }

    public void LowerDuration()
    {
        duration--;
        if(duration == 0)
        {
            isDone = true;
        }
    }
}
