using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** IgniteStatus public class.
 * This script is associated with a IgniteStatus instance prefab.
 * It aims to apply a damage debuff on the target.
 **/
public class IgniteStatus : MonoBehaviour
{
    private int _maxDuration = 500;
    private float _tickInterval = 1;
    private int _damage = 5;
    private EntityLivingBase _entity;
    private GameObject _particles;
	private GameObject _explosion;
	
    /** Start private void
	 * We get the prefab of the Ignite status and instantiate it on the target. We also get the prefab of all animations. 
	 * Then we invoke the Endstatus method which will destroy the debuff in a fixed amount of time (maxDuration).
	 * Finally we launch an InvokeRepeating that will call the DamageFor method every _tickInterval.
	 **/
    private void Start()
    {
		_entity = GetComponent<EntityLivingBase>();
		
        GameObject obj = (GameObject)Resources.Load("FireMage/IgniteStatus", typeof(GameObject));
		_explosion = (GameObject)Resources.Load("FireMage/ExplosionIgniteStatus", typeof(GameObject));

		float verticalPosition = transform.position.y + transform.lossyScale.y * 2  ;
		Vector3 instPos = new Vector3(transform.position.x, verticalPosition, transform.position.z);
        _particles = Instantiate(obj, instPos, obj.transform.rotation, transform);
		
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
	/** ExplodeIgniteStatus public void
	 * This method should only be launched by the ConflagrationSpell.
	 * When launched, we display the Explosion animation of the ignite. Then, we launch the EndStatus();
	 **/
	public void ExplodeIgniteStatus() {
		_explosion = Instantiate(_explosion, _particles.transform.position, _particles.transform.rotation, transform);
		EndStatus();
	}
}