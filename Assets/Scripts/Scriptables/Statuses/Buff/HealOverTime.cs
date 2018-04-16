using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : Status {

    int healAmount = 10;

    public override IEnumerator ApplyEffect(Fighter target)
    {
        Debug.Log("HOT launched");
        GameObject effect = Instantiate(this.effect);
        Destroy(effect, 2f);

        yield return target.Heal(healAmount);
    }
}
