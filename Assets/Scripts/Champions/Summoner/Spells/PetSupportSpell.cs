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
    public Vector3 pospet;

    public override void LaunchSpell()
    {

        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
            // Invoke("Ally_monster", 2);   
            pospet = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2);
            Instantiate(PetMonsterSupp, pospet, Quaternion.identity);
            base.OnSpellLaunched();
        }
    }
    /*protected override void Update()
    {
        if (count == 2)
        {
            Destroy(PetMonsterSupp);
        }
    }*/
}
