using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WindiatorSimpleAttack extends MeleAttack
/// This script need to be attached to the spear of the Windiator
/// It ensure the Collision to an enemy and applies the damages to it.
/// </summary>
public class WindiatorSimpleAttack : MeleAttack {

    private AutoAttackWindiator autoAttackWindiator;

    protected override void Start()
    {
        base.Start();

        autoAttackWindiator = GetComponentInParent<AutoAttackWindiator>();
    }

    /// <summary>
    /// override OnTriggerEnter method of MeleAttack Class
    /// This method Checks if there is a collision between the weapon and the enemy
    /// If true it applies damages and disable the Collider of the weapon
    /// </summary>
    /// <param name="other">is an Enemy (EntityLivingBase)</param>
    protected override void OnTriggerEnter(Collider other)
    {
        EntityLivingBase entityHit = other.gameObject.GetComponent<EntityLivingBase>();
  
        if (entityHit != null && entityHit.tag != "player")
        {
            // Apply something to the enemy
            autoAttackWindiator.ApplyEffect(entityHit);
            _arme.enabled = false;
        }
    }
}
