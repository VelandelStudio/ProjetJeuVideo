using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Harpoon, public class
 * @extends LinearProjectile
 * This script is attached to harpoon's prefabs and applies the effects of the harpoon.
 **/
public class Harpoon : LinearProjectile
{
    private HarpoonSpell _parentSpell;
    private BoxCollider _collider;

    /** ApplyEffect, public override void method
     * Checks if the gameObject hited is a Monster and if this is the case applies the harpoon's effects.
     **/
    public override void ApplyEffect(Collider collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();

        if (entityHit.gameObject.tag == "Monster")
        {
            launcher.GetComponent<HarpoonSpell>().ApplyEffectOnHit(entityHit);
        }
    }

    /** AttributeSpeedAndRange, public override void
     * Initializes the range and the speed of the projectile. 
     **/
    public override void AttributeSpeedAndRange()
    {
        SpellRange = 20;
        ProjectileSpeed = 1000f;
    }
}
