using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStatus : StatusBase, IBuff
{
    public override void OnStatusApplied()
    {
        Debug.Log("DefaultStatus applied");
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
