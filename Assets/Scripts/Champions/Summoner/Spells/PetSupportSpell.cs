using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetSupportSpell : Spell {

    private GameObject _pet;
    private GameObject _target;
    public Vector3 _posPet;
    //private GameObject SummonerNeutral = (GameObject)Resources.Load("Champion/SummonerNeutral");
    private GameObject _swapSupportPS;


    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        _swapSupportPS = (GameObject)Resources.Load("ParticleSystems/SummonerSupportInvoke/SwapSummonerSupportPS");
        _pet = LoadResource("ElementalSupport");

        base.Start();
    }
    //public Vector3 pospet;

    public override void LaunchSpell()
    {
        
        base.LaunchSpell();
        
        if (IsSpellLauncheable())
        {
        
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerSupport");
            Camera.main.transform.parent = null;
            newChampionObj = Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation); // newChampionObj becomes GameObject in the scene not in the prefab !!!!!
            Instantiate(_swapSupportPS, newChampionObj.transform.position, newChampionObj.transform.rotation, newChampionObj.transform);

            _posPet = new Vector3(transform.position.x + 2, transform.position.y + 1, transform.position.z + 2);
            _target = newChampionObj;

            if (GetComponent<SummonerInterface>().Pet != null)
            {
                _posPet = GetComponent<SummonerInterface>().Pet.transform.position;

                if (GetComponent<SummonerInterface>().Pet.GetComponent<PetSummoner>().Target != gameObject)
                {
                    _target = GetComponent<SummonerInterface>().Pet.GetComponent<PetSummoner>().Target;
                }

                Destroy(GetComponent<SummonerInterface>().Pet);
                Debug.Log("Destroy");
            }

            Destroy(oldChampion.gameObject);
            //gameObject.SetActive(false);

            Debug.Log("sort lancé");
            _pet = Instantiate(_pet, _posPet, Quaternion.identity);

            _pet.GetComponent<PetSummoner>().Target = _target;
            _pet.GetComponent<PetSummoner>().Summoner = newChampionObj;

            newChampionObj.GetComponent<SummonerInterface>().Pet = _pet;

            base.OnSpellLaunched();

        }
    }
    protected override void Update()
    {

        base.Update();
        //PetMonsterSupp.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 2);
        // PetMonsterSupp.transform.position = SummonerNeutral.transform.position;
        //MonsterMove();
    
    }
}
