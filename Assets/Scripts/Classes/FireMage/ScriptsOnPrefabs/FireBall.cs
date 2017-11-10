using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FireBall public class.
 * This script is associated with a Fireball instance prefab.
 **/
public class FireBall : LinearProjectile
{
    /** OnCollisionEnter private void.
	 * When the fireball is colliding something if it is an entity (not a player), it applies damage.
     * It also instantiate or refresh an IgniteStatus Prefab (GameObject) as the child of the target gameObject.
	 * then the fireball (gameObject) is destroyed.
	 **/
    public override void ApplyEffect(Collider collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();
        GameObject igniteToApply = (GameObject)Resources.Load("FireMage/IgniteStatus", typeof(GameObject));

        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            entityHit.DamageFor(100);
            IgniteStatus igniteStatus = entityHit.GetComponentInChildren<IgniteStatus>();
            if (igniteStatus != null)
            {
                igniteStatus.ResetStatus();
            }
            else
            {
                GameObject ignite = Instantiate(igniteToApply, entityHit.transform);
                launcher.GetComponent<ConflagrationSpell>().Targets.Add(ignite.GetComponent<IgniteStatus>());
            }
        }
    }
}