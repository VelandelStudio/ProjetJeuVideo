using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedStatus : StatusBase
{
	[SerializeField] ParticleSystem ShieldSurface;
	[SerializeField] ParticleSystem ShieldTrail;
	[SerializeField] ParticleSystem ShieldExplosion;
	/**  OnStatusApplied, public override void
	 * First of all, we get the Main module of the particle system in order to re-attribute its Duration dynamically to fit with the Status Duration.
	 * Then, we launch the ParticleSystem.
	 **/
	public override void OnStatusApplied()
	{
		var main = ShieldSurface.main;
		main.duration = Duration;
		
		main = ShieldTrail.main;
		main.duration = Duration;
		
		ShieldSurface.Play();
		ShieldTrail.Play();
	}
	
	public override void StatusTickBehaviour()
	{
	}
	
	/** OnDestroy, private void
	 * This built-in method is launched when the Status is going to destriy itself.
	 * At this moment, we set the ShieldExplosion particleSystem parent to null, be cause we do not want this element to be destroyed yet.
	 * After that, we play the particleSystem and ask to unity to Destroy it to 2 seconds
	 **/
	private void OnDestroy()
	{
		ShieldExplosion.transform.parent = null;
		ShieldExplosion.Play();
		Destroy(ShieldExplosion.gameObject,2);
	}
}
