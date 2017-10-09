using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Imonster Interface
/// It is created to give to a GameObject the Monster Behaviour
/// It contains 3 methods :
///     - MonsterMove for all movements of a monster
///     - MonsterAutoAttack for basic attacks
///     - MonsterLaunchSpell if the monster as some special capacities
/// </summary>
public interface IMonster {

    void MonsterMove();

    void MonsterAutoAttack();

    void MonsterLaunchSpell();
}
