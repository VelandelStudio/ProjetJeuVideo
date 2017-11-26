using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** IgniteStatus public class.
 * This script is associated with a IgniteStatus instance prefab.
 * It aims to apply a damage debuff on the target.
 **/
public class IgniteStatus : StatusBase
{
    private EntityLivingBase _entity;
	private GameObject _explosion;
	
    /** Start private void
	 * We get the prefab of the Ignite status and instantiate it on the target. We also get the prefab of all animations. 
	 * Then we invoke the Endstatus method which will destroy the debuff in a fixed amount of time (maxDuration).
	 * Finally we launch an InvokeRepeating that will call the DamageFor method every _tickInterval.
	 **/
    protected override void Start()
    {
        base.Start();

        ParticleSystem igniteParticules = GetComponent<ParticleSystem>();
        igniteParticules.Play();

		_entity = GetComponentInParent<EntityLivingBase>();
		_explosion = (GameObject)Resources.Load("FireMage/ExplosionIgniteStatus", typeof(GameObject));

		float verticalPosition = transform.position.y + _entity.transform.lossyScale.y * 2  ;
        transform.position = new Vector3(transform.position.x, verticalPosition, transform.position.z);
    }

	/** ExplodeIgniteStatus public void
	 * This method should only be launched by the ConflagrationSpell.
	 * When launched, we display the Explosion animation of the ignite. Then, we launch the EndStatus();
	 **/
	public void ExplodeIgniteStatus() {
		_explosion = Instantiate(_explosion, transform.position, transform.rotation, transform.parent);
		Destroy(gameObject);
	}

    public override void OnStatusApplied()
    {
        Debug.Log("IgniteStatus Created !");
    }

    public override void StatusTickBehaviour()
    {
        _entity.DamageFor(Damages[0]);

        Debug.Log("IgniteStatus Tick !");
    }
}