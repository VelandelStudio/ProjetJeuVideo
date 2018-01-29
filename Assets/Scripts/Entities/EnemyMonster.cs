using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/** EnemyMonster, public class
 * @extends Monster
 * This script is used to set the Behaviour of an ennemy monster that is able to locate a player, follow and attack him.
 **/
public class EnemyMonster : Monster
{
    private NavMeshAgent _agent;
    [SerializeField] private float range;
    [SerializeField] private float attackCD;
    private float nextAttackTimer;

    private Vector3 originalPosition;
    /// <summary>
    /// The start method get the NavMeshAgent component
    /// </summary>
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        nextAttackTimer = 0f;
        originalPosition = transform.position;
    }

    /** Update, protected override void method.
     * Firs of all, we ensure that the base method is launched. Then, if your monster has a tagrt (Player),
     * We update his position and his rotation to make him follow and watch the Player.
     * If the Monster is close enought to the playe, the monster attacks.
     **/
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

    /** MonsterAutoAttack, public override void method.
     * Reset the CD of the attacks and displays a Debug.Log. 
     * This script will evole to apply damages or status to a Player.
     **/
    public override void MonsterAutoAttack()
    {
        nextAttackTimer = attackCD;
        Debug.Log("Monster is defoncing " + Target);
    }


    /** OnLoseTarget, public override void method.
     * When the mosnter looses the target, he goes back to its initial position.
     **/
    public override void OnLoseTarget()
    {
        var lookPos = originalPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        _agent.SetDestination(originalPosition);
    }

    public override void OnTargetSelected() {}
}
