using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowStatus : StatusBase, IBuff
{
    private float MoveSpeedToDecreased;

    /* status that decrease the movespeed of the target hit by the SummonerSupport's auto attack  */
    public override void OnStatusApplied()
    {
        MoveSpeedToDecreased = (float.Parse(OtherValues[0]));
        GetComponentInParent<Characteristics>().MovementSpeedFactor -= MoveSpeedToDecreased; // decrease the speed of the target
        Debug.Log("slowStatus applied = vitesse du monstre: " + GetComponentInParent<Characteristics>().MovementSpeedFactor);
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        GetComponentInParent<Characteristics>().MovementSpeedFactor += MoveSpeedToDecreased; // return the speed of the target
        Debug.Log("slowStatus destroyed = vitesse du monstre: " + GetComponentInParent<Characteristics>().MovementSpeedFactor);
    }
    
}

