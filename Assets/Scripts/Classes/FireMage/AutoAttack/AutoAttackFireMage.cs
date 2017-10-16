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
        Transform[] transformTab = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform tr in transformTab)
        {
            if (tr.gameObject.name == "EthanLeftHand")
            {
                _launcherTransform = tr;
                break;
            }
        }
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

            Debug.Log("AutoAttackFireMage Launched");
            base.AutoAttack();
        }
    }

    /** OnAttackHit : public void Method
	 * The OnAttackHit Method should be called by every AutoAttackFireMage prefabs when they collide an EntityLivingBase.
	 * When this method is launched, we cancel the RemoveShield invoke in order to reset the timer of the shield associated to the entity.
	 * Then, we add a shield to the FireMage and increase the shield value. Please note that the value should be increased by 5 points every hit, with a maximum of _maxValueShield.
	 * After that, we re-invoke the RemoveShield method that will occurs in _maxDurationShield seconds.
	 **/
    public void OnAttackHit()
    {
        CancelInvoke("RemoveShield");
        _shield = GetComponent<Shield>();
        if (_shield == null)
        {
            _shield = gameObject.AddComponent<Shield>();
        }
        _shield.AddShieldValue(Mathf.Clamp(5, 0, _maxValueShield - _shield.ShieldValue));
        Invoke("RemoveShield", _maxDurationShield);
    }

    /** RemoveShield : private void Method
	 * This method calls the RemoveShield method in the Shield instance.
	 **/
    private void RemoveShield()
    {
        _shield.RemoveShield();
    }
}