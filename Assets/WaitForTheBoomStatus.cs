using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTheBoomStatus : StatusBase, IBuff {
    {
    public override void OnStatusApplied()
    {
            Debug.Log("WaitForTheBoomStatus applied ! be careful the explosion is near !!!!");
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
