using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoostPetStatus : StatusBase, IBuff
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
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        DefenseToAdd = (float)System.Math.Round(double.Parse(OtherValues[0])* Player.GetComponent<DeflagrationSpell>().TargetsTouched.Count, 1); // calcul of the Defense to add 
        DefenseAdded += DefenseToAdd; // maybe not usefull now
        // value of defense to increase depending of the number of monsters with TouchStatus

        GetComponentInParent<Characteristics>().Defense += DefenseToAdd; // increase defense of Pet charactéristics
        Debug.Log("Player DefenseToAdd: " + DefenseToAdd);
        Debug.Log("Summoner characteristics.Defense : " + characteristics.Defense); 

    }

    private void OnDestroy()
    {
        GetComponentInParent<Characteristics>().Defense -= DefenseAdded; // decrease defense of Pet charactéristics
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}


