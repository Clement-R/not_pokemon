using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnOverTime : Status {

    int damageAmount = 10;

    public override IEnumerator ApplyEffect(Fighter target)
    {
        GameObject effect = Instantiate(this.effect, new Vector2(target.transform.position.x, target.transform.position.y), Quaternion.identity);
        Destroy(effect, 2f);

        DisplayLog(target.name + " is hurt for " + damageAmount);

        yield return target.TakeDamage(damageAmount);
    }
}
