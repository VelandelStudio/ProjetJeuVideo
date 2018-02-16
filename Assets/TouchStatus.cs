using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchStatus : StatusBase, IBuff
{
    public override void OnStatusApplied()
    {
        Debug.Log("TouchStatus applied ! the end is near !!!!");
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
