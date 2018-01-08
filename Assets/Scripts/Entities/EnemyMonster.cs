using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMonster : Monster
{
    private NavMeshAgent _agent;
    [SerializeField] private float range;
    [SerializeField] private float attackCD;
    private float nextAttackTimer;
    /// <summary>
    /// The start method get the NavMeshAgent component
    /// </summary>
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        nextAttackTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();
        if (Target != null)
        {
            
            var lookPos = Target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

            Vector3 destination = Target.transform.position;
            _agent.SetDestination(destination);

            if (nextAttackTimer == 0f && DistanceToTarget <= range)
            {
                MonsterAutoAttack();
            }
        }

        if(nextAttackTimer != 0f)
        {
            nextAttackTimer -= Time.deltaTime;
            nextAttackTimer = Mathf.Clamp(nextAttackTimer, 0f, nextAttackTimer);
        }
    }

    public override void MonsterAutoAttack()
    {
        nextAttackTimer = attackCD;
        Debug.Log("Monster is defoncing " + Target);
    }

    public override void OnLoseTarget() {}

    public override void OnTargetSelected() {}
}
