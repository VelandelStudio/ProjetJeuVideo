
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShieldSpell : Spell
{

    // private GameObject _summoner;
    // private ShieldDecreaseDamageStatus decreaseDamageStatus;

    public GameObject _shieldExplosion;


    private GameObject _summoner;


    protected override void Start()
    {

        // _shieldExplosion = (GameObject)Resources.Load("Standard Assets/ParticleSystems/Prefabs/Explosion");
        //_explosion = LoadResource("Explosion");
        _summoner = gameObject;
        base.Start();
    }
   /* protected override void Update()
    {

            ApplyEffect(_summoner);
            _spellIsUsed = false;

        base.Update();
    }*/

    public override void LaunchSpell()
    
    {
        // restore hp and increase defense of the player for a limited time then explode and make damages to monster around the player
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
           
            //Debug.Log(" Dammage dealt to the player reduced by" + int.Parse(OtherValues[0]));  // % dammage reduction
            // ApplyStatus(GetComponent<EnergyShieldSpell>().Status[2], transform);
            Debug.Log("Shield Explode");
            Debug.Log("Damage --");
            ApplyStatus(Status[0], _summoner.transform);            // ApplyStatus(GetComponent<EnergyShieldSpell>().Status[0], transform);
            Debug.Log("Def ++");
            ApplyStatus(Status[1], _summoner.transform);
            //  _shieldExplosion = Instantiate(_shieldExplosion, transform.position, transform.rotation,this.transform) as GameObject;
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }


    }
    
    


}
