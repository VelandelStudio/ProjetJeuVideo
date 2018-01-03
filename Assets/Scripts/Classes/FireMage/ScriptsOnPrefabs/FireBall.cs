using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FireBall public class.
 * This script is associated with a Fireball instance prefab.
 **/
public class FireBall : LinearProjectile
{
    /** OnCollisionEnter private void.
	 * When the fireball is colliding something if it is an entity (not a player),
	 * it notifies the ApplyEffectOnHit method from the FireBallSpell of its launcher.
	 **/
    public override void ApplyEffect(Collider collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();

        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            launcher.GetComponent<FireBallSpell>().ApplyEffectOnHit(entityHit);
        }
    }

    /** AttributeSpeedAndRange, public override void,
     * This method is used to attribute an initial SpellRange and ProjectileSpeed
     **/
    public override void AttributeSpeedAndRange()
    {
        SpellRange = 20;
        ProjectileSpeed = 1000f;
    }
}