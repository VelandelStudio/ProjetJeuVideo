using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAOESpell : Spell
{

    private GameObject _pet;
    private GameObject _target;
    public Vector3 _posPet;

    private GameObject _swapAOEPS;

    //public int count = 0;
    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        _swapAOEPS = (GameObject)Resources.Load("ParticleSystems/SummonerAOEInvoke/SwapSummonerAOEPS");
        _pet = LoadResource("ElementalAoe");
        base.Start();
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerAOE");

            Camera.main.transform.parent = null;
            newChampionObj = Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation); // newChampionObj becomes GameObject in the scene not in the prefab !!!!!
            Instantiate(_swapAOEPS, newChampionObj.transform.position, newChampionObj.transform.rotation, newChampionObj.transform);

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

            //gameObject.SetActive(false);
            Destroy(oldChampion.gameObject);
            Debug.Log("sort lancé");
            _pet = Instantiate(_pet, _posPet, Quaternion.identity);

            _pet.GetComponent<PetSummoner>().Target = _target;
            _pet.GetComponent<PetSummoner>().Summoner = newChampionObj;

            newChampionObj.GetComponent<SummonerInterface>().Pet = _pet;
            base.OnSpellLaunched();
        }
    }

    /* protected override void Update()
     {
         if (count == 2)
         {
             Destroy(PetMonsterAOE);
         }
     }*/
}

