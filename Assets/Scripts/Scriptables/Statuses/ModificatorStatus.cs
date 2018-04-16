using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificatorStatus : Status
{
    public int dexterity;
    public int strength;
    public int accuracy;

    public override IEnumerator ApplyEffect(Fighter target)
    {
        //GameObject effect = Instantiate(this.effect);
        //Destroy(effect, 2f);

        yield return null;
    }
}
