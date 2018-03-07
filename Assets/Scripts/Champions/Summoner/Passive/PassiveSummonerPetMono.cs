using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSummonerPetMono : PassiveBase
{

    private bool _summonerIsInRange;
    private GameObject _voltage;

    private void Start()
    {
        _voltage = transform.Find("Status/VoltageStatus").gameObject;

        if (_voltage == null)
        {
            _voltage = ApplyStatus(Status[0], transform);
        }
    }

    private void Update()
    {
        _summonerIsInRange = SummonerDistance();
    }

    private bool SummonerDistance()
    {
        if (Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<SummonerInterface>().Pet.transform.position) > 10.0f)
        {
            return false;
        }

        return true;
    }

    public void VoltageStacksEnhancer(int value)
    {
        if (_summonerIsInRange)
        {
            _voltage.GetComponent<VoltageStatus>./*nom methode*/(value);
        }
    }
}
