using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// AllyMonster Class, inherits Monster, implement IInterractableEntity
/// An AllyMonster is a monster who fight or follow a Player.
/// It implements IInterractableEntity because it needed an first interaction with it
/// for following or lauch secondary quest...
/// 
/// for now an AllyMonster is able to ask the player for following action
/// </summary>
public class AllyMonster : Monster, IInterractableEntity
{
    private NavMeshAgent _agent;        // This object give the faculty to walk along walkable ground
    private bool _follow = false;       // determine if the AllyMonster follow or not the player

    /// <summary>
    /// The start method get the NavMeshAgent component
    /// </summary>
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Update method : Call also the update of EntityLivingBase and Monster Class
    /// Call also the MonsterMove from IMonster
    /// </summary>
    protected override void Update()
    {
        base.Update();

        MonsterMove();
    }

    /// <summary>
    /// OnTriggerStay callback method
    /// Same function as SimpleNPC make the Ally looking at the Player
    /// </summary>
    /// <param name="other">The collider of another object : the Player</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
            return;

        Transform target = other.transform;
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }

    /// <summary>
    /// ActivateInterractable method from IInterractableEntity
    /// Activation give to AllyMonter the faculty to follow the Player
    /// </summary>
    public void ActivateInterractable()
    {
        if (Target != null && _follow)      // The Ally is following you
        {
            Debug.Log("I'm already at your service Sir !");
        } 
        else if (Target == null)            // First interaction with the
        {
            Debug.Log("Come closer");
        }
        else
        {
            Debug.Log("Sir ! I'm at your service !");
            _follow = true;
        }
    }

    /// <summary>
    /// DisplayTextOfInterractable method from IInterractableEntity
    /// Display what says the AllyMonster
    /// </summary>
    public void DisplayTextOfInterractable()
    {
        Debug.Log("Hello Sir !");
    }

    /// <summary>
    /// MonsterAutoAttack method from IMonster
    /// Is called when the monster attack an enemy
    /// </summary>
    public override void MonsterAutoAttack()
    {
        Debug.Log("I'm using a capacity !");
    }

    /// <summary>
    /// MonsterAutoAttack method from IMonster
    /// Is called when the monster use a capacity to an enemy
    /// </summary>
    public override void MonsterLaunchSpell()
    {
        Debug.Log("Engaging target !");
    }

    /// <summary>
    /// MonsterAutoAttack method from IMonster
    /// Is called to give movement to the monster
    /// as an Ally it will automatically move juste behing the player
    /// </summary>
    public override void MonsterMove()
    {
        if (Target != null && _follow)
        {
            Vector3 destination = Target.transform.position - new Vector3(2.5f, 0, 2.5f);
            _agent.SetDestination(destination);
        }
    }
}
