using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** PassiveSummonerPetMono : public class
 * @extends PassiveBase
 * This script is attached to the SummonerMono and allows to apply the VoltageStacks if the pet is in range.
 **/
public class PassiveSummonerPetMono : PassiveBase
{

    private bool _summonerIsInRange;
    private GameObject _voltage;

    /** Start, protected override void Method
     * Checks if the VoltageStatus is already applied. If this is not the case VoltageStatus is applied.
     **/
    protected override void Start()
    {
        _voltage = transform.Find("Status/VoltageStatus").gameObject;

        if (_voltage == null)
        {
            _voltage = ApplyStatus(Status[0], transform);
        }

        base.Start();
    }

    /** Update, private void method
     * Call the SummonerDistance to check if the pet is in range.
     **/
    private void Update()
    {
        _summonerIsInRange = SummonerDistance();
    }

    /** SummonerDistance, private bool Method
     * This method allow  to check if the distance between summoner and pet allows to generate stacks of voltage.
     **/
    private bool SummonerDistance()
    {
        if (Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<SummonerInterface>().Pet.transform.position) > 10.0f)
        {
            return false;
        }

        return true;
    }

    /*
    public void VoltageStacksEnhancer(int value)
    {
        if (_summonerIsInRange)
        {
            _voltage.GetComponent<VoltageStatus>.(value);
        }
    }
    */
}
