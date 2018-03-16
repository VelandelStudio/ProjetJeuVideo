using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownBeforeTheBoomStatus : StatusBase, IBuff
{
  
    public override void OnStatusApplied()
    {
        GetComponentInParent<Characteristics>().MovementSpeedFactor = 0; // Pet can't move before THE EXPLOSION 
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}

