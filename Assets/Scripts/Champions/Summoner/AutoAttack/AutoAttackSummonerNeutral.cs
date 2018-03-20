using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** AutoAttackSummonerNeutral Class, extends AutoAttackBase
 * This Auto-attack is associated with the SummonerNeutralClass
 * The objectif of this auto-attack is to Instantiate a prefab (AutoAttackSummonerNeutral) and apply a force on it.
 **/
public class AutoAttackSummonerNeutral : AutoAttackBase
{

    private GameObject _throwable;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;

    /** Start : protected override void Method
    * First at all, we override the GCD of the mother class.
    * Then we get the prefab of the AutoAttackSummonerNeutral.
    * Then, the scripts is looking for the origin point of the instantiation (i.e. the hand of our character).
    **/
    protected override void Start ()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _throwable = LoadResource("AutoAttackSummonerNeutral");
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }

    /** AutoAttack : public override void Method
	 * The AutoAttack Method is called by the abstract Class Classe when the player press the key associated to the auto-attack.
	 * First at alt, we check if the auto-attack is ready.
	 * After that, we instantiate a AutoAttackSummonerNeutral, make it look at the target, apply a force to it and launche the particle system associated to the prefab.
	 * Final, we call the AutoAttack method in the mother class.
	 **/
    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            base.AutoAttack();
            Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);
        }
    }

    /** OnAttackHit : public void Method
    * The OnAttackHit Method should be called by every AutoAttackSummonerNeutral prefabs when they collide an EntityLivingBase.
    **/
    public void OnAttackHit(EntityLivingBase eHit)
    {

        eHit.DamageFor(Damages[0]);
    }

}
