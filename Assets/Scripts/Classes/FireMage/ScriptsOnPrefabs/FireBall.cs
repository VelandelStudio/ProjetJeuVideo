using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FireBall public class.
 * This script is associated with a Fireball instance prefab.
 **/
public class FireBall : LinearProjectile
{
    /** OnCollisionEnter private void.
	 * When the fireball is colliding something if it is an entity (not a player), it applies damage and a fresh IgniteStatus on the target.
	 * then the fireball (gameObject) is destroyed.
	 **/
    public override void ApplyEffect(Collider collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();
        
        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            entityHit.DamageFor(100);
            IgniteStatus ignite = entityHit.gameObject.GetComponent<IgniteStatus>();
            if (ignite != null)
            {
                ignite.ResetStatus();
            }
            else
            {
                ignite = entityHit.gameObject.AddComponent<IgniteStatus>();
                launcher.GetComponent<ConflagrationSpell>().Targets.Add(ignite);
            }
        }
    }
}