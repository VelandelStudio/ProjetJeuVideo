using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerMono : Champion, SummonerInterface
{
    private GameObject _pet = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void AutoAttack()
    {
        base.AutoAttack();
    }

    public GameObject Pet
    {
        get
        {
            return _pet;
        }

        set
        {
            _pet = value;
        }
    }
}
