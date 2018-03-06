using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStatus : StatusBase, IDebuff
{
    private EntityLivingBase _entity;
    //private EnemyMonster _scriptParent;
    //private GameObject _explosion;

    protected override void Start()
    {
        base.Start();
        //_scriptParent = GetComponentInParent<EnemyMonster>();
        _entity = GetComponentInParent<EntityLivingBase>();

    }

    public override void OnStatusApplied()
    {
        Debug.Log("LightningStatus Created !");
    }


    public override void StatusTickBehaviour()
    {
        _entity.DamageFor(Damages[0]);
    }
}