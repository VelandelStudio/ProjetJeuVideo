using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityStatus : StatusBase, IBuff
{
    private float variable;

    public override void OnStatusApplied()
    {
        Debug.Log("Player's move speed : " + this.characteristics.MovementSpeedFactor);
        this.characteristics.MovementSpeedFactor += float.Parse(OtherValues[0]);
        Debug.Log("Player's move speed : " + this.characteristics.MovementSpeedFactor);
    }

    public override void DestroyStatus()
    {
       this.characteristics.MovementSpeedFactor -= float.Parse(OtherValues[0]);
        Debug.Log("Player's move speed : " + this.characteristics.MovementSpeedFactor);

        base.DestroyStatus();
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
