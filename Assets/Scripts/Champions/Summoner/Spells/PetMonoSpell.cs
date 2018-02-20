using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMonoSpell : Spell {


    private GameObject PetMonster = (GameObject)Resources.Load("SummonerNeutral/PetMonster");
    private GameObject SummonerNeutral = (GameObject)Resources.Load("Champion/SummonerNeutral");

    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
        {
            PetMonster = LoadResource("PetMonster");
            base.Start();
        }

    // public int count = 0;
    public Vector3 pospet;

    public override void LaunchSpell()
        {
            base.LaunchSpell();
            if (IsSpellLauncheable())
            {
           //pospet=SummonerNeutral.transform.position;
            pospet = new Vector3(transform.position.x+2, transform.position.y, transform.position.z+2);
            Debug.Log("sort lancé");
                // Invoke("Ally_monster", 2);           
                Instantiate(PetMonster, pospet, Quaternion.identity);
                base.OnSpellLaunched();
               
             }
      
    }
/*
    protected override void Update()
    {
         if (this.count == 4)
        {
            Destroy(PetMonster, 3f);
        }
    }*/
}
