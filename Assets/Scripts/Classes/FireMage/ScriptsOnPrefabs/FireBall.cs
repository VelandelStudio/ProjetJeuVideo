using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FireBall public class.
 * This script is associated with a Fireball instance prefab.
 **/
public class FireBall : MonoBehaviour
{
    /** OnCollisionEnter private void.
	 * When the fireball is colliding something if it is an entity (not a player), it applies damage and a fresh IgniteStatus on the target.
	 * then the fireball (gameObject) is destroyed.
	 **/
    private void OnCollisionEnter(Collision collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();
        Debug.Log(collision.gameObject);
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
                transform.parent.GetComponent<ConflagrationSpell>().Targets.Add(ignite);
            }
        }
        Destroy(this.gameObject);
    }
}