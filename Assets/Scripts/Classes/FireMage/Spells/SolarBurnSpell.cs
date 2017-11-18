using System.Collections;
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
    private GameObject throwable;
    private bool _playerTargetingFloor;
    public Vector3 TargetOfSolarBurn;
    /** Start : protected override void Method
	 * The Start Method is used here to get the camera and the transform associated to the player.
	 * Once it is done, we apply the CD of the spell and launch the mother Method to initialize the spell.
	 **/
    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        throwable = (GameObject)Resources.Load("FireMage/SolarBall", typeof(GameObject));
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
        if (hasFoundHitPoint)
        {
            base.LaunchSpell();
            TargetOfSolarBurn = hit.point;
        }
        else
        {
            return;
        }

        Vector2 pointInCircle = Random.insideUnitCircle.normalized * 8;

        Vector3 v = new Vector3(TargetOfSolarBurn.x + pointInCircle.x, TargetOfSolarBurn.y + 15, TargetOfSolarBurn.z + pointInCircle.y);
        Instantiate(throwable, v, new Quaternion(), this.transform);

        base.OnSpellLaunched();
    }

    /** ApplyEffectOnHit, public void Method
	 * @Params : EntityLivingBase
	 * When the instance of SolarBurn hits an entity, this method is launched.
	 * It applies damages on the target.
	**/
    public void ApplyEffectOnHit(EntityLivingBase entityHit)
    {
        entityHit.DamageFor(SpellDefinition.BaseDamage);
    }

    /** ApplyAdditionalEffect, public void Method
	 * @Params : EntityLivingBase
	 * When the instance of SolarBurn hits the floor, an explosion occurs.
	 * This method is called for every target that are caught by the Explosion.
	 * It applies damaes to the target.
	**/
    public void ApplyAdditionalEffect(EntityLivingBase entityHit)
    {
        entityHit.DamageFor(SpellDefinition.AdditionalDamages[0]);
    }

    /** getDescriptionVariables, protected override object[]
	 * Return an array of objects that represents the current variables displayed on the GUI
	**/
    protected override object[] getDescriptionVariables()
    {
        return new object[] { SpellDefinition.BaseDamage, SpellDefinition.AdditionalDamages[0] };
    }

    /** IsSpellLauncheable(), public override bool method
     * The solar burn spell is launcheable only on ground surfaces.
     * First we check in a Raycastall if the player is aiming at the ground.
     * If it is, and if the base.IsSpellLauncheable() is true, then you can cast the spell.
     **/
    public override bool IsSpellLauncheable()
    {
        _playerTargetingFloor = false;
        RaycastHit[] hits = Physics.RaycastAll(PosHelper.GetOriginOfDetector(transform), _cameraPlayer.transform.forward,Mathf.Infinity);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.tag == "Floor" || hits[i].collider.gameObject.name == "Ground")
            {
                _playerTargetingFloor = true;
                break;
            }
        }

        return (base.IsSpellLauncheable() && _playerTargetingFloor);
    }
    /** AvailableForGUI(), public override bool method
     * The Spell is available on the GUI if the player is currently targeting at the floor.
     **/
    public override bool AvailableForGUI()
    {
        return _playerTargetingFloor;
    }
}