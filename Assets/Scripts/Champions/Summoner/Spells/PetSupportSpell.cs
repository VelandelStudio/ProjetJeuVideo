using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetSupportSpell : Spell {

    private GameObject PetMonsterSupp;
    private GameObject SummonerNeutral = (GameObject)Resources.Load("Champion/SummonerNeutral");
    private bool _follow = false;       // determine if the AllyMonster follow or not the player
    private NavMeshAgent _agent;        // This object give the faculty to walk along walkable ground

    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        PetMonsterSupp = LoadResource("PetMonsterSupp");
        _agent = GetComponent<NavMeshAgent>();
        base.Start();
    }
    public Vector3 pospet;

    public override void LaunchSpell()
    {

        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
            // Invoke("Ally_monster", 2);   
            pospet = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2);
            Instantiate(PetMonsterSupp, pospet, Quaternion.identity);
            //Pet = true;
            base.OnSpellLaunched();
        }
    }
    protected override void Update()
    {

        base.Update();
        PetMonsterSupp.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 2);
        // PetMonsterSupp.transform.position = SummonerNeutral.transform.position;
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
        if (SummonerNeutral != null && _follow)      // The Ally is following you
        {
            Debug.Log("I'm already at your service Sir !");
        }
        else if (SummonerNeutral == null)            // First interaction with the
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
    /// MonsterAutoAttack method from IMonster
    /// Is called to give movement to the monster
    /// as an Ally it will automatically move juste behing the player
    /// </summary>
    public override void MonsterMove()
    {
        if (SummonerNeutral != null && _follow)
        {
            Vector3 destination = SummonerNeutral.transform.position - new Vector3(2.5f, 0, 2.5f);
            _agent.SetDestination(destination);
        }
    }
    
}

