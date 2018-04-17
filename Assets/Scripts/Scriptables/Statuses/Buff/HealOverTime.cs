using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : Status {

    public int healAmount = 10;

    public override IEnumerator ApplyEffect(Fighter target)
    {
        GameObject effect = Instantiate(this.effect, new Vector2(target.transform.position.x, target.transform.position.y), Quaternion.identity);
        Destroy(effect, 2f);

        DisplayLog(target.name + " heal itself for " + healAmount);

        yield return target.StartCoroutine(target.Heal(healAmount));
    }
}
