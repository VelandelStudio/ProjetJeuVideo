using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShieldSpell : Spell
{
    private GameObject SummonerSupport = (GameObject)Resources.Load("Champion/SummonerSupport");
    private GameObject _monBouclier = (GameObject)Resources.Load("SummonerSupport/Bouclier");


    protected override void Start()
    {
        _monBouclier = LoadResource("Bouclier");
        base.Start();
    }

    public Vector3 posShield;

    public override void LaunchSpell()
    
    {
        // restore hp and increase defense of the player for a limited time then explode and make damages to monster around the player
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
             GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerSupport");
    
            posShield = new Vector3(transform.position.x+1 , transform.position.y+1, transform.position.z+1 );

            _monBouclier = Instantiate(_monBouclier, posShield, Quaternion.identity); 
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
