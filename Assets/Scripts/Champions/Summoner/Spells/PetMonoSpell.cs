using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMonoSpell : Spell {


        private GameObject PetMonster;

        /// <summary>
        /// Override Start method
        /// Load the wall from the ressources and call the mother method
        /// </summary>
        protected override void Start()
        {
            PetMonster = LoadResource("PetMonster");
            base.Start();
        }

    public int count = 0;

    public override void LaunchSpell()
        {
            base.LaunchSpell();
            if (IsSpellLauncheable())
            {
            count++;
           // GameObject CameraPlayer= (GameObject)Resources.Load("SummonerNeutra/Camera");

            Debug.Log("sort lancé");
                // Invoke("Ally_monster", 2);           
                Instantiate(PetMonster, new Vector3(-75, 270, -75), Quaternion.identity);
                base.OnSpellLaunched();
               
             }
      
    }

    protected override void Update()
    {
        if (this.count == 4)
        {
            Destroy(PetMonster, 3f);
        }
    }
}
