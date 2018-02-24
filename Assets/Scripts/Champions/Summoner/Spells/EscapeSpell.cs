using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSpell : Spell {

    private GameObject _summoner;
    private GameObject _pet;
    private bool _spellIsUsed;

    protected override void Start()
    {
        _summoner = gameObject;
        _pet = gameObject.GetComponent<SummonerInterface>().Pet;

        _spellIsUsed = false;

        base.Start();
    }

    protected override void Update()
    {

        if (Vector3.Distance(_pet.transform.position, _summoner.transform.position) < 5.0f && _spellIsUsed == true)
        {
            ApplyEffect(_summoner);
            _spellIsUsed = false;
        }

        base.Update();
    }

    public override void LaunchSpell()
    {
        // change current position of the player and pet then apply status "velocity" to them
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            _pet.GetComponent<PetSummoner>().Target = _summoner;
            _spellIsUsed= true;
            

            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }

    public void ApplyEffect (GameObject gameObject)
    {
        ApplyStatus(Status[0], gameObject.transform);
    }
}
