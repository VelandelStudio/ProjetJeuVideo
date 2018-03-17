using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/** HarpoonSpell, public class
 * @extends EntityLivingBase
 * This script codes for the IA of the Summoner's pet.
 **/
public class PetSummoner : EntityLivingBase
{
    UnityEngine.AI.NavMeshAgent nav;

    private Transform _posPet;
    private Transform _posTarget;
    private GameObject _summoner;
    private float _autoAttackTimer;
    private float _autoAttackCD = 3.0f;
    public GameObject _target;

    /** Start, protected override void method
     * Get the _target's transform and the transform's pet and we initialize the _autoAttackTimer variable and the hps of the pet.
     **/
    void Start ()
    {
        _posTarget = _target.transform;
        _posPet = transform;
        _autoAttackTimer = 0.0f;
       // nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        InitializeLivingEntity(1000, 1000);
    }

    /** Update, protected virtual void Method.
	 * The Update method is used to check the distance between the pet and his target, and the nature of the target.
     * If the target is different from the player, the pet attacks.
	 **/
    protected override void Update ()
    {
        _posTarget = _target.transform;

        var lookPos = Target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        if (Vector3.Distance(_posPet.position, _posTarget.position) > 3.0f)
        {
            _posPet.transform.position = Vector3.Lerp(_posPet.position, _posTarget.position, Time.deltaTime);
            //nav.SetDestination(_target.transform.position);
        }

        else
        {

            if (_target.tag != "Player")
            {
                _autoAttackTimer += Time.deltaTime;
                _autoAttackTimer = Mathf.Clamp(_autoAttackTimer, 0f, _autoAttackTimer);
                //Debug.Log(_autoAttackTimer);

                if (_autoAttackTimer >= _autoAttackCD)
                {
                    PetAutoAttack();
                }
            }
        }
        //if (!_summoner.GetComponent<EntityLivingBase>().IsAlive)
        //{
        //    base.InstantKill();
        //}

        base.Update();
	}

    /** PetAutoAttack, public void Method.
	 * This method allows to the pet to attack his target and reset the _autoAttackTimer.
	 **/
    public void PetAutoAttack()
    {
        _autoAttackTimer = 0f;
       Debug.Log("Pet autoAttack " + Target);
    }
    
    public GameObject Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }
    
    public GameObject Summoner
    {
        get
        {
            return _summoner;
        }
        set
        {
            _summoner = value;
        }
    }
}
