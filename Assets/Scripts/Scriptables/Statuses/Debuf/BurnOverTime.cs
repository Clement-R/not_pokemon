using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnOverTime : Status {

    int damageAmount = 10;

    public override IEnumerator ApplyEffect(Fighter target)
    {
        Debug.Log("HOT launched");
        GameObject effect = Instantiate(this.effect);
        Destroy(effect, 2f);

        yield return target.TakeDamage(damageAmount);
    }
}
