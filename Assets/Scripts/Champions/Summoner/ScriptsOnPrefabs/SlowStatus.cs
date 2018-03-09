using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowStatus : StatusBase, IBuff
{
    /* status that boost the defense, it's applied on the summonerAOE and his PET by the PassiveSummonerPetAOE */
    public override void OnStatusApplied()
    {
        int SpeedBase = 10;
        int SpeedDecreased = System.Convert.ToInt32(SpeedBase *float.Parse(OtherValues[0])); // decrease the speed of the target
        Debug.Log("slowStatus applied = vitesse diminuée de " + SpeedBase + " à " + SpeedDecreased);
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}

