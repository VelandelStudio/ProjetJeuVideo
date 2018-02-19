using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoostStatus : StatusBase, IBuff
{
    /* status that boost the defense, it's applied on the summonerAOE and his PET by the PassiveSummonerPetAOE */ 
    public override void OnStatusApplied()
    {
        int defenseBase = 10;
        int nbEnemyMonsterTouched = GetComponentInParent<DeflagrationSpell>().TargetsTouched.Count; // count of the number of EnemyMonster entities with TouchStatus 
        int defenseIncreased = defenseBase * nbEnemyMonsterTouched; // Increased the defense boost 
        Debug.Log("DefenseBoostStatus applied = Défense augmentée de "+defenseBase+" à "+ defenseIncreased);
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
