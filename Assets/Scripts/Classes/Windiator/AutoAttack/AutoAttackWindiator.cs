using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AutoAttackWindiator extends AutoAttackBase
/// This is obviously the script that launch the AutoAttacks
/// This script is attached to the Windiator Character
/// </summary>
public class AutoAttackWindiator : AutoAttackBase {

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
        _anim = GetComponent<Animator>();
        _wsa = GetComponentInChildren<WindiatorSimpleAttack>();

        _wsa.AttributeAutoAttack(this);

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
            base.AutoAttack();
            _wsa.SwapEnableArmeCol();
            _anim.SetBool("AutoAttack", true);
            StartCoroutine(CoroutineOneSec());
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
        AnimatorStateInfo animationState = _anim.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = _anim.GetCurrentAnimatorClipInfo(0);
        float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
        yield return new WaitForSeconds(myTime);

        if (_wsa.GetValueColArme())
        {
            _wsa.SwapEnableArmeCol();
        }

        _anim.SetBool("AutoAttack", false);
    }

    /// <summary>
    /// ApplyEffect, protected void method
    /// Used by the scripts attached to the weapon and deals damaged to the target
    /// Try also to apply the WindSpiritStatus to an ally
    /// </summary>
    /// <param name="hit">Enemy</param>
    public override void ApplyEffect(EntityLivingBase hit)
    {
        hit.DamageFor(Damages[0]);

        PassiveWindiator passive = GetComponent<PassiveWindiator>();
        if (hit.tag != "Player")
        {
            // Need table of player so I apply status on Windiator before new features
            passive.ProcPassive(this.gameObject);
        }
    }
}
