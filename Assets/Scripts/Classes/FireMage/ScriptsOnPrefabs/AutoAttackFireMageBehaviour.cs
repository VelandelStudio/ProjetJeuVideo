﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** AutoAttackFireMageBehaviour public class.
 * This script is associated with a AutoAttackFireMage instance prefab.
 **/
public class AutoAttackFireMageBehaviour : LinearProjectile
{
    /// <summary>
    /// ApplyEffect method -> implementation of the abstract method in LinearProjectile mother Class
    /// Deal damage to the target
    /// Call the OnAtackHit for the launcher with AutoAttackFireMage attached. Shield the launcher
    /// </summary>
    /// <param name="col">>is the collider touch by the projectile</param>
    public override void ApplyEffect(Collider col)
    {
<<<<<<< HEAD
=======
		Debug.Log(col.gameObject.name);
>>>>>>> refs/remotes/origin/Develop
        if (eHit != null && eHit.gameObject.tag != "Player")
        {
            eHit.DamageFor(5);

            launcher.GetComponent<AutoAttackFireMage>().OnAttackHit();
        }
    }
}