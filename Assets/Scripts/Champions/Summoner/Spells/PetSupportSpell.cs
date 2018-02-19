using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSupportSpell : Spell {

    private GameObject PetMonsterSupp;

    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        PetMonsterSupp = LoadResource("PetMonsterSupp");
        base.Start();
    }

    public override void LaunchSpell()
    {

        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
            // Invoke("Ally_monster", 2);           
            Instantiate(PetMonsterSupp, new Vector3(-75, 270, -75), Quaternion.identity);
            base.OnSpellLaunched();
        }
    }
}
