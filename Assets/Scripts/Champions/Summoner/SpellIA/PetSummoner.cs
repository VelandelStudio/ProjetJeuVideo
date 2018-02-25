using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetSummoner : EntityLivingBase
{
    UnityEngine.AI.NavMeshAgent nav;

    private Transform _posPet;
    private Transform _posTarget;
    private GameObject _summoner;
    private float _autoAttackTimer;
    private float _autoAttackCD = 3.0f;

    public GameObject _target;
   
    void Start ()
    {
        _posTarget = _target.transform;
        _posPet = transform;
        _autoAttackTimer = 0.0f;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
	protected override void Update ()
    {
        _posTarget = _target.transform;
        if (Vector3.Distance(_posPet.position, _posTarget.position) > 2.0f)
        {
            _posPet.transform.position = Vector3.Lerp(_posPet.position, _posTarget.position, Time.deltaTime);
            //nav.SetDestination(_target.transform.position);
        }


        var lookPos = Target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

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

        //if (!_summoner.GetComponent<EntityLivingBase>().IsAlive)
        //{
        //    base.InstantKill();
        //}

        base.Update();
	}

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
