using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** AutoAttackFireMageBehaviour public class.
 * This script is associated with a AutoAttackFireMage instance prefab.
 **/
public class AutoAttackFireMageBehaviour : MonoBehaviour
{

    /** OnCollisionEnter private void.
	 * When the AutoAttackFireMage prefabs hits a collider, if it is an EntityLivingBase, the script will notify the AutoAttackFireMage by launching its OnAttackHit method
	 * Then the prefab is destroyed.
	 **/
    private void OnCollisionEnter(Collision collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();
        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            entityHit.DamageFor(5);
            transform.parent.GetComponent<AutoAttackFireMage>().OnAttackHit();
        }
        Destroy(gameObject);
    }
}