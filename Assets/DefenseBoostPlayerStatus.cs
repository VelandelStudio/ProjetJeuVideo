using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoostPlayerStatus : StatusBase, IBuff
{
    private float DefenseToAdd;
    private float DefenseAdded = 0; // maybe not usefull now

    protected override void Start()
    {
        base.Start();
        transform.localPosition += new Vector3(0f, transform.parent.lossyScale.y * 2f, 0f);
    }

    /* status that boost the defense, it's applied on the summonerAOE and his PET by the PassiveSummonerPetAOE */
    public override void OnStatusApplied()
    {
        DefenseToAdd = (float)System.Math.Round(double.Parse(OtherValues[0])*GetComponentInParent<DeflagrationSpell>().TargetsTouched.Count, 1); // calcul of the defense to add
        DefenseAdded += DefenseToAdd; // maybe not usefull now
        // value of defense to increase depending of the number of monsters with TouchStatus
        Debug.Log("Pet characteristics.Defense avant boost : " + characteristics.Defense);
        GetComponentInParent<Characteristics>().Defense += DefenseToAdd; // increase defense of Player charactéristics
        Debug.Log("Pet DefenseToAdd: " + DefenseToAdd);
        Debug.Log("Pet characteristics.Defense après boost :" + characteristics.Defense);
    }

    private void OnDestroy()
    {
        GetComponentInParent<Characteristics>().Defense -= DefenseAdded; // decrease defense of Player charactéristics
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}


