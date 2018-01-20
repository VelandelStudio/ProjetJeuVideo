using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WindiatorSimpleAttack extends MeleeAttack
/// This script need to be attached to the spear of the Windiator
/// It ensure the Collision to an enemy and applies the damages to it.
/// </summary>
public class WindiatorSimpleAttack : MeleeAttack {

    private AutoAttackWindiator _autoAttackWindiator;

    /// <summary>
    /// Start Method used to start parameters in the mother Class
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Update check the state of the collider of the weapon.
    /// If the attack touch nothing, the collider stay enable, so we need to disable it.
    /// But we need to do it at the moment where the autoAttack stopped
    /// </summary>
    private void Update()
    {
        if (_arme.enabled)
        {
            Invoke("SwapEnableArmeCol", _autoAttackWindiator.CoolDownValue);
        }
        else
        {
            CancelInvoke();
        }
    }

    /// <summary>
    /// AttributeAutoAttack method
    /// Build the MeleeAttack with the corresponding AutoAttack in parents
    /// </summary>
    /// <param name="autoAttack">Parent AutoAttack</param>
    public void AttributeAutoAttack(AutoAttackWindiator autoAttack)
    {
        _autoAttackWindiator = autoAttack;
    }

    /// <summary>
    /// override OnTriggerEnter method of MeleeAttack Class
    /// This method Checks if there is a collision between the weapon and the enemy
    /// If true it applies damages and disable the Collider of the weapon
    /// </summary>
    /// <param name="other">is an Enemy (EntityLivingBase)</param>
    protected new void OnTriggerEnter(Collider other)
    {
        EntityLivingBase entityHit = other.gameObject.GetComponent<EntityLivingBase>();
  
        if (entityHit != null && entityHit.tag != "player")
        {
            _autoAttackWindiator.ApplyEffect(entityHit);
            _arme.enabled = false;
            colArme = false;
        }
    }
}
