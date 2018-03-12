using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchStatus : StatusBase, IBuff
{  
    public override void OnStatusApplied()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("TouchStatus applied ! the end is near !!!!");
        Player.GetComponent<PassiveSummonerPetAOE>().TouchCount += 1; // add one to the counter of TouchStatus in PassiveSummonerPetAOE
    }


    private void OnDestroy()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponentInParent<PassiveSummonerPetAOE>().TouchCount -= 1; //substrack one to the counter of TouchStatus in PassiveSummonerPetAOE
    }


    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
