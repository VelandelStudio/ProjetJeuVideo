﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSummonerPetSupport : PassiveBase {

    float dist;
    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(GetComponent<SummonerInterface>().Pet.transform.position, transform.position); // process the distance between the SummonerAOE and his Pet

        /* if the SummonerSupport (Player) is in a certain range of his Pet apply RegenerationStatus to the Player and the Pet */
        if (dist <= float.Parse(OtherValues[0]))
        {
            ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], GetComponent<SummonerInterface>().Pet.transform); //Problem with the GUi the Pet being children of the his Summoner it shoud not be a problem now !
            ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], transform); 
        }
        // TODO regen for the pet 

         }
    }
}
