using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormSpell : Spell
{
    private Camera _cameraPlayer;
    private GameObject _throwable;
    private Transform _launcherTransform;
    private int stacks;

    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _throwable = LoadResource("ThunderStorm");
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            //apply damages on one target depending of the player stacks 
            Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }

    public void ApplyEffectOnHit(EntityLivingBase entityHit)
    {
        stacks = GetComponentInChildren<VoltageStatus>().GetNumberOfStacks();
        if (stacks < 5)
        {
            entityHit.DamageFor(Damages[0]);
        }
        if (stacks >= 5 && stacks < 10 )
        {
            entityHit.DamageFor(Damages[1]);
        }
        if (stacks >= 10 && stacks < 20)
        {
            entityHit.DamageFor(Damages[2]);
        }
        if (stacks >= 20)
        {
            entityHit.DamageFor(Damages[3]);
        }

        GetComponentInChildren<VoltageStatus>().RemoteStacks();

        ThunderStormStatus thunderStormStatus = entityHit.GetComponentInChildren<ThunderStormStatus>();

        if (thunderStormStatus != null)
        {
            thunderStormStatus.ResetStatus();
            Debug.Log("ThunderStormStatus successfully reset");
        }
        else
        {
            ApplyStatus(Status[0], entityHit.transform);
            Debug.Log("ThunderStormStatus successfully applied : "  + Status[0]);
        }

        //gameObject.GetComponent<PassiveSummonerPetMono>().VoltageStacksEnhancer(Int32.Parse(OtherValues[0]));
    }
}
