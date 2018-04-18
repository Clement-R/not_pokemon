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
        
        // TODO : Move that code in a dedicated manager that communicate through event manager
        //bool waitForPlayerAction = true;
        //while (waitForPlayerAction)
        //{
        //    yield return null;

        //    if (Input.anyKeyDown)
        //    {
        //        waitForPlayerAction = false;
        //    }
        //}

        LowerDuration();
    }

    public abstract IEnumerator ApplyEffect(Fighter target);

    public void DisplayLog(string log)
    {
        // TODO : Trigger event to launch combat log animation
        Debug.Log(log);
        EventManager.TriggerEvent("displayText", new { text = log });
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
