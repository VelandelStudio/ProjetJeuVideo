using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** TridentLaunchSpell, public class
 * @Extends StackableSpell
 * Spell associated to the Windiator Champion. When used, launch a Trident prefab in front of the player.
 * When the Trident hit something, the ApplyEffectOnHit method is launched.
 **/
public class TridentLaunchSpell : StackableSpell
{

    private Camera _cameraPlayer;
    private GameObject _throwable;
    private Transform _launcherTransform;

    protected override void Start()
    {
        base.Start();
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);
        _throwable = (GameObject)Resources.Load("Windiator/Trident", typeof(GameObject));
    }

    /** LaunchSpell : public override void Method
	 * The LauncheSpell Method is called by the abstract Class Classe when the player press the key associated to the spell.
	 * First at alt, we launch the mother method to initialize the spell launching. If the spell is Launcheable, we find a target point for our projectile.
	 * This target can be a HitPoint from a raycast or a point on the line from the player to the Camera.transform.forward (i.e. is the raycast does not intercept an entity).
	 * After that, we instantiate a fireball, make it look at the target, apply a force to it and launche the particle system associated to the prefab.
	 * Final, we call the OnSpellLaunched method in the mother class.
	 **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (IsSpellLauncheable())
        {
            Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);
            base.OnSpellLaunched();
        }
    }

    /** ApplyEffectOnHit, public void Method
	 * @Params : EntityLivingBase
	 * When the instance of Trident hits an entity, this method is launched.
	 * It applies damage to the target.
	 **/
    public void ApplyEffectOnHit(EntityLivingBase entityHit)
    {
        entityHit.DamageFor(Damages[0]);
    }
}