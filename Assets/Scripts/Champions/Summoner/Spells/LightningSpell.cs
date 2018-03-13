using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
    private Camera _cameraPlayer;
    private GameObject _throwable;
    private Transform _launcherTransform;

    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _throwable = LoadResource("Lightning");
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
           //apply damages on one target (short cooldown) and apply 2 stacks
           Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);
           Debug.Log("sort lancé");
           base.OnSpellLaunched();
        }
    }

    public void ApplyEffectOnHit(EntityLivingBase entityHit)
    {
        entityHit.DamageFor(Damages[0]);
        GetComponentInChildren<VoltageStatus>().AddStacks(NumberOfStacks);
    }
}