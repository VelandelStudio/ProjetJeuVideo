using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackSummonerNeutralBehaviour : LinearProjectile
{


    /// <summary>
    /// ApplyEffect method -> implementation of the abstract method in LinearProjectile mother Class
    /// Deal damage to the target
    /// Call the OnAtackHit for the launcher with AutoAttackFireMage attached. Shield the launcher
    /// </summary>
    /// <param name="col">>is the collider touch by the projectile</param>
    public override void ApplyEffect(Collider col)
    {
        launcher.GetComponent<AutoAttackSummonerNeutral>().OnAttackHit(eHit);
    }

    /** AttributeSpeedAndRange, public override void,
     * This method is used to attribute an initial SpellRange and ProjectileSpeed
     **/
    public override void AttributeSpeedAndRange()
    {
        SpellRange = 15;
        ProjectileSpeed = 1200f;
    }
}
