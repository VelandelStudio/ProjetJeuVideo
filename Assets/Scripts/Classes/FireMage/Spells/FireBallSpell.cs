using UnityEngine;

/** FireBallSpell Class, extends Spell
 * This spell is associated with the FireMageClass
 * The objectif of this spell is to Instantiate a prefab (FireBall) and apply a force on it.
 **/
public class FireBallSpell : Spell
{
    private Camera _cameraPlayer;
    private GameObject _throwable;
    private Transform _launcherTransform;

    /** Start : protected override void Method
	 * The Start Method is used here to get the prefab of the fireball.
	 * Then, the scripts is looking for the origin point of the instantiation (i.e. the hand of our character).
	 * Once it is done, we apply ce CD of the spell and laucnhe the mother Method to initialize the spell.
	 **/
    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _throwable = (GameObject)Resources.Load("FireMage/FireBall", typeof(GameObject));
        Transform[] transformTab = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform tr in transformTab)
        {
            if (tr.gameObject.name == "EthanLeftHand")
            {
                _launcherTransform = tr;
                break;
            }
        }
        spellCD = 3.0f;
        base.Start();
    }

    /** LaunchSpell : public override void Method
	 * The LauncheSpell Method is called by the abstract Class Classe when the player press the key associated to the spell.
	 * First at alt, we launch the mother method to initialize the spell launching. If the spell is Launcheable, we find a target point for our projectile.
	 * This target can be a HitPoint from a raycast or a point on the line from the player to the Camera.trasnform.forward (i.e. is the raycast does not intercept an entity).
	 * After that, we instantiate a fireball, make it look at the target, apply a force to it and launche the particle system associated to the prefab.
	 * Final, we call the OnSpellLaunched method in the mother class.
	 **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (!IsSpellLauncheable())
        {
            return;
        }

        RaycastHit hit;
        bool hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(transform), _cameraPlayer.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        Vector3 target;
        if (hasFoundHitPoint)
        {
            target = hit.point;
        }
        else
        {
            target = _launcherTransform.position + _cameraPlayer.transform.forward * 10;
        }

        GameObject throwableInstance = Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);

        throwableInstance.transform.LookAt(target);
        throwableInstance.GetComponent<Rigidbody>().AddForce(throwableInstance.transform.forward * 1000);
        ParticleSystem particles = throwableInstance.GetComponent<ParticleSystem>();
        particles.Play();
        base.OnSpellLaunched();
    }
}