using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** AutoAttackFireMageBehaviour public class.
 * This script is associated with a AutoAttackFireMage instance prefab.
 **/
public class AutoAttackFireMageBehaviour : LinearProjectile
{
    public override void ApplyEffect(Collider col)
    {
        if (eHit != null && eHit.gameObject.tag != "Player")
        {
            eHit.DamageFor(5);

            launcher.GetComponent<AutoAttackFireMage>().OnAttackHit();
        }
    }
}