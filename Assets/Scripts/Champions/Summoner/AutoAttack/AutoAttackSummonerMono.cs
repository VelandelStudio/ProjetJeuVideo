using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackSummonerMono : AutoAttackBase
{

    private GameObject _throwable;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;

    /** Start : protected override void Method
    
    **/
    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _throwable = LoadResource("AutoAttackSummonerMono");
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }

    /** AutoAttack : public override void Method
	 
	 **/
    public override void AutoAttack ()
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
       gameObject.GetComponent<PassiveSummonerMono>().VoltageStacksEnhancer(Int32.Parse(OtherValues[0]));
    }

}
