using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** IgniteStatus public class.
 * This script is associated with a IgniteStatus instance prefab.
 * It aims to apply a damage debuff on the target.
 **/
public class IgniteStatus : MonoBehaviour
{
    private int _maxDuration = 5;
    private float _tickInterval = 1;
    private int _damage = 5;
    private EntityLivingBase _entity;
    private GameObject _particles;

    /** Start private void
	 * We get the prefab of the Ignite status and instantiate it on the target. 
	 * Then we invoke the Endstatus method which will destroy the debuff in a fixed amount of time (maxDuration).
	 * Finally we launch an InvokeRepeating that will call the DamageFor method every _tickInterval.
	 **/
    private void Start()
    {
        GameObject obj = (GameObject)Resources.Load("FireMage/IgniteStatus", typeof(GameObject));
        _particles = Instantiate(obj, transform.position, transform.rotation, transform);

        _entity = GetComponent<EntityLivingBase>();
        Invoke("EndStatus", _maxDuration);
        InvokeRepeating("DamageFor", _tickInterval, _tickInterval);
    }

    /** DamageFor private void
	 * This Method is used with the InvokeRepeating Coroutine. That will call the DamageFor() Method in the target.
	 **/
    private void DamageFor()
    {
        _entity.DamageFor(_damage);
    }

    /** ResetStatus public void
	 * This Method reset invokes in order to apply a fresh IngiteStatus on the target.
	 **/
    public void ResetStatus()
    {
        CancelInvoke("EndStatus");
        CancelInvoke("DamageFor");
        Invoke("EndStatus", _maxDuration);
        InvokeRepeating("DamageFor", _tickInterval, _tickInterval);
    }

    /** EndStatus public void
	 * This Method is used to instantly destroy the Status and the associated GameObject.
	 **/
    public void EndStatus()
    {
        Destroy(_particles);
        Destroy(this);
    }
}