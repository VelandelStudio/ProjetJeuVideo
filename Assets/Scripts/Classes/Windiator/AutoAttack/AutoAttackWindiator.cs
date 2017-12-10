﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AutoAttackWindiator extends AutoAttackBase
/// This is obviously the script that launch the AutoAttacks
/// This script is attached to the Windiator Character
/// </summary>
public class AutoAttackWindiator : AutoAttackBase {

    private float _WindiatorGCD = 2f;
    private Animator _anim;
    private WindiatorSimpleAttack _wsa;

    /// <summary>
    /// Start Method
    /// Setting the GCD of the AutoAttack
    /// Getting the Animator of the Character
    /// And the WindiatorSimpleAttack, a prefab script attached to the weapon.
    /// </summary>
    protected override void Start()
    {
        GCD = _WindiatorGCD;
        _anim = GetComponent<Animator>();
        _wsa = GetComponentInChildren<WindiatorSimpleAttack>();

        base.Start();
    }

    /// <summary>
    /// AutoAttack method
    /// This method is called each time the played click the right button
    /// and if the AutoAttack is ready
    /// Then the collider of the weapon is set to true and the animation of the attack is played.
    /// then a coroutine is launched to disable the weapon if nothing was touched.
    /// </summary>
    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _wsa.SwapEnableArmeCol();
                _anim.SetBool("AutoAttack", true);
                StartCoroutine(CoroutineOneSec());
            }

            base.AutoAttack();
        }
    }

    /// <summary>
    /// CoroutineOneSec Coroutine...
    /// Wait for 1 sec before disable the Collider's weapon
    /// Put to false the anim to return animator to the Idle State
    /// </summary>
    /// <returns>1 sec waiting</returns>
    IEnumerator CoroutineOneSec()
    {
        yield return new WaitForSeconds(1f);

        if (_wsa.GetValueColArme())
        {
            _wsa.SwapEnableArmeCol();
        }

        _anim.SetBool("AutoAttack", false);
    }
}
