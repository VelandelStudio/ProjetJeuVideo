using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonedDebuffStatus : StatusBase, IDebuff
{
    private EntityLivingBase _entity;
    private EnemyMonster _scriptParent;


    protected override void Start()
    {

        base.Start();

        Debug.Log("HarpoonedDebuffStatus Created !");
        _scriptParent = GetComponentInParent<EnemyMonster>();
    }
    

    public override void OnStatusApplied()
    {
        Debug.Log("HarpoonedStatus Created !");

    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
    
}
