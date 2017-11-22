using UnityEngine;

/** AutoAttackFireMage Class, extends AutoAttackBase
 * This Auto-attack is associated with the FireMageClass
 * The objectif of this auto-attack is to Instantiate a prefab (AutoAttackFireMage) and apply a force on it.
 * Also, a shield will be added to the player if the prefab hits an entity.
 **/
public class AutoAttackFireMage : AutoAttackBase
{
    private GameObject _throwable;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;

    private Shield _shield;
    private float _maxDurationShield = 5f;
    private float _maxValueShield = 50f;
    private float _fireMageGCD = 1.5f;
    private GameObject _shieldObject;
    private GameObject _shieldInstance;

    /** Start : protected override void Method
	 * First at all, we override the GCD of the mother class.
	 * Then we get the prefab of the AutoAttackFireMage.
	 * Then, the scripts is looking for the origin point of the instantiation (i.e. the hand of our character).
	 **/
    protected override void Start()
    {
        GCD = _fireMageGCD;
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _throwable = (GameObject)Resources.Load("FireMage/AutoAttackFireMage", typeof(GameObject));
        _shieldObject = (GameObject)Resources.Load("FireMage/FireMageShield", typeof(GameObject));
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform);

        base.Start();
    }

    /** AutoAttack : public override void Method
	 * The AutoAttack Method is called by the abstract Class Classe when the player press the key associated to the auto-attack.
	 * First at alt, we check if the auto-attack is ready, then, we find a target point for our projectile.
	 * This target can be a HitPoint from a raycast or a point on the line from the player to the Camera.trasnform.forward (i.e. is the raycast does not intercept an entity).
	 * After that, we instantiate a AutoAttackFireMage, make it look at the target, apply a force to it and launche the particle system associated to the prefab.
	 * Final, we call the AutoAttack method in the mother class.
	 **/
    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            base.AutoAttack();
            Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform);
        }
    }

    /** OnAttackHit : public void Method
    * The OnAttackHit Method should be called by every AutoAttackFireMage prefabs when they collide an EntityLivingBase.
    * When this method is launched, we cancel the RemoveShield invoke in order to reset the timer of the shield associated to the entity.
    * Then, we add a shield to the FireMage and increase the shield value. Please note that the value should be increased by 5 points every hit, with a maximum of _maxValueShield.
    * After that, we re-invoke the RemoveShield method that will occurs in _maxDurationShield seconds.
    **/
    public void OnAttackHit(EntityLivingBase eHit)
    {
        eHit.DamageFor(AutoAttackDefinition.Damages[0]);
        CancelInvoke("RemoveShield");

        _shield = GetComponent<Shield>();
        if (_shield == null)
        {
            _shield = gameObject.AddComponent<Shield>();
            _shieldInstance = Instantiate(_shieldObject, transform.position + _shieldObject.transform.position, transform.rotation, this.transform);
        }

        _shield.AddShieldValue(Mathf.Clamp(5, 0, _maxValueShield - _shield.ShieldValue));
        Invoke("RemoveShield", _maxDurationShield);
    }

    protected override object[] getDescriptionVariables()
    {
        return new object[] { AutoAttackDefinition.Damages[0] };
    }

    /** RemoveShield : private void Method
	 * This method calls the RemoveShield method in the Shield instance.
	 **/
    private void RemoveShield()
    {
        _shield.RemoveShield();
        Destroy(_shieldInstance);
    }
}