using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoostStatus : StatusBase, IBuff
{
    public override void OnStatusApplied()
    {
        int defenseBase = 10;
        int nbEnemyMonsterTouched = GetComponentInParent<DeflagrationSpell>().TargetsTouched.Count;
        int defenseIncreased = defenseBase * nbEnemyMonsterTouched;
        Debug.Log("DefenseBoostStatus applied = Défense augmentée de "+defenseBase+" à "+ defenseIncreased);
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
