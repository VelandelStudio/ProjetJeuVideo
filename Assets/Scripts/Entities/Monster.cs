using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Represents the abstract Class of Monster
/// A monster has a target and can switch target    
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public abstract class Monster : EntityLivingBase, IMonster
{
    protected GameObject _target;
    protected NavMeshAgent _agent;
    protected Vector3 originalPosition;
    protected Quaternion originalRotation;
    [SerializeField] protected float RangeOfAttack;
    [SerializeField] protected float attackCD;
    protected float nextAttackTimer;

    protected override void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        Target = other.gameObject;
    }

    public bool IsPlayerDetected
    {
        get { return (!_target); }
    }

    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public void ResetTarget()
    {
        Target = null;
        OnLoseTarget();
    }

    protected override void Update()
    {
        base.Update();
        if(IsDead)
        {
            return;
        }

        if (Target)
        {
            MonsterMove();
            if (DistanceToTarget <= RangeOfAttack && nextAttackTimer == 0f)
            {
                ActionMonster();
            }
        }
        else
        {
            if (transform.position != originalPosition)
            {
                OnLoseTarget();
            }
        }

        if (nextAttackTimer != 0f)
        {
            nextAttackTimer -= Time.deltaTime;
            nextAttackTimer = Mathf.Clamp(nextAttackTimer, 0f, nextAttackTimer);
        }
    }

    protected virtual void OnLoseTarget()
    {
        var lookPos = originalPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        _agent.SetDestination(originalPosition);
    }

    public double DistanceToTarget
    {
        get
        {
            if (IsPlayerDetected)
            {
                return Vector3.Distance(transform.position, Target.transform.position);
            }

            return 0f;
        }
    }

    protected virtual void ActionMonster()
    {
        nextAttackTimer = attackCD;
        MonsterAutoAttack();
    }

    // Implements IMonster...
    public virtual void MonsterMove()
    {
        var lookPos = Target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        Vector3 destination = Target.transform.position;
        _agent.SetDestination(destination);
    }

    public virtual void MonsterAutoAttack() { }
    public virtual void MonsterLaunchSpell() { }
}