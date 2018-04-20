using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class BurnOverTime : Status {

    public int damageAmount = 10;

    public override IEnumerator ApplyEffect(Fighter target)
    {
        GameObject effect = Instantiate(this.effect, new Vector2(target.transform.position.x, target.transform.position.y), Quaternion.identity);
        Destroy(effect, 2f);
        
        string log = target.name + " is hurt for " + damageAmount;
        EventManager.TriggerEvent(EventList.DISPLAY_TEXT.ToString(), new { text = log });

        yield return target.StartCoroutine(target.TakeDamage(damageAmount));
    }
}
