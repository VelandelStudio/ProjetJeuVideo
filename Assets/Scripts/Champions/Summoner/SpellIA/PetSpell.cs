using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetSpell : EntityLivingBase
{
    private Transform _posPet;
    private Transform _posTarget;
    UnityEngine.AI.NavMeshAgent nav;

    public GameObject _target;
   
    void Start ()
    {
        _posTarget = _target.transform;
        _posPet = transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
	protected override void Update ()
    {

        if (Vector3.Distance(_posPet.position, _posTarget.position) > 3.0f)
        {
            _posPet.transform.position = Vector3.Lerp(_posPet.position, _posTarget.position, Time.deltaTime);
            //nav.SetDestination(_target.transform.position);
        }


        var lookPos = Target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
        
        base.Update();
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
}
