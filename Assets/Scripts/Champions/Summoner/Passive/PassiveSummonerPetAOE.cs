using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSummonerPetAOE : PassiveBase {

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(GetComponentInChildren<AllyMonster>().transform.position, transform.position);

        if(dist<=float.Parse(OtherValues[0]) && GetComponentInChildren<DeflagrationSpell>().TargetsTouched.Count>0)
        {
            //ApplyStatus(GetComponent<DeflagrationSpell>().Status[0], GetComponentInChildren<AllyMonster>().transform);
            ApplyStatus(GetComponent<DeflagrationSpell>().Status[0], transform);
        }
    }
}
