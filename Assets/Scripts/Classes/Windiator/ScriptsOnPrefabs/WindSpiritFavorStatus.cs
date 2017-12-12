using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpiritFavorStatus : StatusBase, IBuff
{
    public override void OnStatusApplied()
    {
        Debug.Log("Buffed");
    }

    public override void StatusTickBehaviour()
    {
        // No Ticks
    }
}
