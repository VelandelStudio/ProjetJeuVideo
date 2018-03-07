﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Lightning public class.
 * This script is associated with a Lightning instance prefab.
 **/
public class Lightning : LinearProjectile
{
 
    public override void ApplyEffect(Collider col)
    {
        EntityLivingBase entityHit = col.gameObject.GetComponent<EntityLivingBase>();

        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            launcher.GetComponent<LightningSpell>().ApplyEffectOnHit(entityHit);
            Debug.Log("cible touchée");
        }
    }

    public override void AttributeSpeedAndRange()
    {
        SpellRange = 20;
        ProjectileSpeed = 1000f;
    }
}