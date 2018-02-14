using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackSummonerNeutral : AutoAttackBase
{

    private GameObject _throwable;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;

    
	protected override void Start ()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _throwable = LoadResource("AutoAttackSummonerNeutral");
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }

    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            base.AutoAttack();
            Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);
        }
    }

    public void OnAttackHit(EntityLivingBase eHit)
    {

        eHit.DamageFor(Damages[0]);
    }

}
