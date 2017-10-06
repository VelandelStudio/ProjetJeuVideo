﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** SolarBurnSpell Class, extends Spell
 * This spell is associated with the FireMageClass
 * The objectif of this spell is to Instantiate a prefab (FireBall) and apply a force on it.
 * This spell also has the particularity to apply quadrupled critical damages.
 **/
public class SolarBurnSpell : Spell
{
    private Camera _cameraPlayer;

    /** Start : protected override void Method
	 * The Start Method is used here to get the camera and the transform associated to the player.
	 * Once it is done, we apply the CD of the spell and launch the mother Method to initialize the spell.
	 **/
    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        SpellCD = 7f;
        base.Start();
    }

    /** LaunchSpell : public override void Method
	 * The LauncheSpell Method is called by the abstract Class Classe when the player press the key associated to the spell.
	 * First at all, we launch the mother method to initialize the spell launching. If the spell is Launcheable, we find a target point for our projectile.
	 * If the RayCast does not intercept a collider, the spell can not be launched.
	 * After that, we instantiate a fireball, make it look at the target, apply a force to it and launche the particle system associated to the prefab.
	 * The particularity of this instantiation is that the fireball appears on a circle in the sky at random coords, upper the target Point.
	 * Finally, we call the OnSpellLaunched method in the mother class.
	 **/
    public override void LaunchSpell()
    {
        if (!IsSpellLauncheable())
            return;

        RaycastHit hit;
        bool hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(transform), _cameraPlayer.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        Vector3 target;
        if (hasFoundHitPoint)
        {
            base.LaunchSpell();
            target = hit.point;
        }
        else
        {
            return;
        }

        GameObject throwable = (GameObject)Resources.Load("SpellPRefabs/FireBall", typeof(GameObject));

        Vector2 pointInCircle = Random.insideUnitCircle.normalized * 8;
        Vector3 v = new Vector3(target.x + pointInCircle.x, target.y + 10, target.z + pointInCircle.y);
        GameObject throwableInstance = Instantiate(throwable, v, new Quaternion(), this.transform);

        throwableInstance.transform.LookAt(target);
        throwableInstance.GetComponent<Rigidbody>().AddForce(throwableInstance.transform.forward * 1000);
        ParticleSystem particles = throwableInstance.GetComponent<ParticleSystem>();
        particles.Play();

        base.OnSpellLaunched();
    }
}