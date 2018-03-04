using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMonoSpell : Spell
{


    //private GameObject PetMonster = (GameObject)Resources.Load("SummonerNeutral/PetMonster");
    private GameObject _pet;
    private GameObject _target;
    private GameObject _swapMonoPS;
    public Vector3 _posPet;
    // private GameObject SummonerNeutral = (GameObject)Resources.Load("Champion/SummonerNeutral");

    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        _pet = LoadResource("ElementalMono");
        _swapMonoPS = (GameObject)Resources.Load("ParticleSystems/SummonerMonoInvoke/SwapSummonerMonoPS");

        base.Start();
    }

    // public int count = 0;

    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {

            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerMono");
            Camera.main.transform.parent = null;
            newChampionObj = Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
            Instantiate(_swapMonoPS, newChampionObj.transform);

            Debug.Log("Hello world");
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

            //pospet=SummonerNeutral.transform.position;
            Debug.Log("sort lancé");
            // Invoke("Ally_monster", 2);           
            _pet = Instantiate(_pet, _posPet, Quaternion.identity);
            _pet.GetComponent<PetSummoner>().Target = _target;
            _pet.GetComponent<PetSummoner>().Summoner = newChampionObj;
            newChampionObj.GetComponent<SummonerInterface>().Pet = _pet;

            Destroy(oldChampion.gameObject);
            base.OnSpellLaunched();
            //change de forme 
            //Destroy(oldChampion.gameObject);
            // gameObject.SetActive(false);

        }

    }

    public void Destruc(GameObject PetMonster)
    {
        Destroy(PetMonster.gameObject);
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
