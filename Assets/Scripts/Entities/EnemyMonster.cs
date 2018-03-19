using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** EnemyMonster, public class
 * @extends Monster
 * This script is used to set the Behaviour of an ennemy monster that is able to locate a player, follow and attack him.
 **/
public class EnemyMonster : Monster
{
    /** MonsterAutoAttack, public override void method.
     * Reset the CD of the attacks and displays a Debug.Log. 
     * This script will evole to apply damages or status to a Player.
     **/
    public override void MonsterAutoAttack()
    {
        Debug.Log("Monster is defoncing " + Target);
    }
}
