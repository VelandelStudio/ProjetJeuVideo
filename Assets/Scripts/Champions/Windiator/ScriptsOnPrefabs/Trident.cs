using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Trident, public class
 * @extends : LinearProjectile
 * This script should always be attached to the Trident prefab of the Windiator.
 **/
public class Trident : LinearProjectile
{
    public override void AttributeSpeedAndRange()
    {
        SpellRange = 30;
        ProjectileSpeed = 200;
    }

    /** OnCollisionEnter private void.
	 * When the Trident is colliding something if it is an entity (not a player),
	 * it notifies the ApplyEffectOnHit method from the TridentLaunchSpell of its launcher.
	 **/
    public override void ApplyEffect(Collider collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();

        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            launcher.GetComponent<TridentLaunchSpell>().ApplyEffectOnHit(entityHit);
        }
    }
}