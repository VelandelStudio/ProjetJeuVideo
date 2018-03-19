using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSummonerMono : PassiveBase
{
   
    private bool _summonerIsInRange;
    private VoltageStatus _voltage;

    private void Start()
    {
         _voltage = GetComponentInChildren<VoltageStatus>();

         if (_voltage == null)
         {
            ApplyStatus(Status[0], transform);
         }
    }

    private void Update()
    {


        if (GetComponent<SummonerInterface>().Pet != null)
        {
            _summonerIsInRange = SummonerDistance();
        }
    }

    private bool SummonerDistance()
    {
        if (Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<SummonerInterface>().Pet.transform.position) < 100.0f)
        {
            return true;
        }

        return false;
    }

    public void VoltageStacksEnhancer(int value)
    {
        if (_summonerIsInRange && _voltage != null)
        {
           // _voltage.GetComponent<VoltageStatus>().AddStacks(value);
        }
    }
    
}
