using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownBeforeTheBoomStatus : StatusBase, IBuff
{
    
    public override void OnStatusApplied()
    {
          

            Debug.Log("CountDownBeforeTheBoomStatus applied ! be careful the explosion is near get the fu***ing out of here !!!!");
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}

