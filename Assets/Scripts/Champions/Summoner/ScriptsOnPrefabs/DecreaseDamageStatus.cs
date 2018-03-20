using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseDamageStatus : StatusBase, IDebuff
{

    private Characteristics _entityCharacteristics;
    private float _defenseDebuff;


    public override void OnStatusApplied()
    {
        Debug.Log("Defense :  " + GetComponentInParent<EntityLivingBase>().GetComponent<Characteristics>().Defense);
        GetComponentInParent<EntityLivingBase>().GetComponent<Characteristics>().Defense += float.Parse(OtherValues[0]);
        Debug.Log("Defense :  " + GetComponentInParent<EntityLivingBase>().GetComponent<Characteristics>().Defense);
    }

    public override void DestroyStatus()
    {
        GetComponentInParent<EntityLivingBase>().GetComponent<Characteristics>().Defense += float.Parse(OtherValues[0]);
        Debug.Log("Defense :  " + GetComponentInParent<EntityLivingBase>().GetComponent<Characteristics>().Defense);
       
        base.DestroyStatus();
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
