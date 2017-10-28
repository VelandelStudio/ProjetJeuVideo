using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** PassiveFireMage Class
 * This PassiveClasse is associated with the FireMageClass
 * The main aim of this passive is to attribute to the FireMage a Critical Chance bonus for every targets affected by a IgniteStatus
 * The bonus has for value 5% per igniteStatus in the game and can not exceed 25%. 
 **/
public class PassiveFireMage : MonoBehaviour
{
    public float CritChanceToAdd;

    private ConflagrationSpell _conflagrationSpell;
    private float _numberOfIgnites = 0f;

    /** Start : protected override void Method
	 * The Start Method is used here to get the ConflagrationSpell associated to the player and the NumBer Of Ignites in the game.
	 **/
    private void Start()
    {
        _conflagrationSpell = GetComponent<ConflagrationSpell>();
    }

    /** Update : private void Method
	 * The Update Method is used here to get the NumBer Of Ignites in the game at every moments. 
	 * After that, it sets the critical chance bonus value clamped between 0 and 25% 
	 * The default value is +5% per IgniteStatus in the game.
	 **/
    private void Update()
    {
        _numberOfIgnites = _conflagrationSpell.Targets.Count;
        CritChanceToAdd = Mathf.Clamp(5 * _numberOfIgnites, 0, 25);
    }
}