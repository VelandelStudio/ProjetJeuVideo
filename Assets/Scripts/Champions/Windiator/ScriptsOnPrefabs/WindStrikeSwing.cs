using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** WindStrikeSwing, public class
 * This script should only be attached to the WindStrikeSwing gameObject and instanciated by the WindStrikeSpell of the Windiator champion.
 * When launched, an animation is played. If the collider of the swing collides a monster, we contact the WindStrikeSpell.
 **/
public class WindStrikeSwing : MonoBehaviour
{

    private WindStrikeSpell _parentSpell;
    private Animator _anim;
    private BoxCollider _collider;

    /** Start, private void method
	 * When started, we get the parent spell and everything we need to launch the WindStrikeSwing anim
	 * When the trigger WindStrike is launched, the anim starts.
	 **/
    private void Start()
    {
        _parentSpell = GetComponentInParent<WindStrikeSpell>();
        _collider = GetComponent<BoxCollider>();
        _anim = GetComponent<Animator>();
        _anim.SetTrigger("WindStrike");
    }

    /** Update, private void Method
	 * We check if the collider is enabled. If it is not (the anim is over), then we destroy the gameObject
	 **/
    private void Update()
    {
        if (_collider.enabled == false)
        {
            Destroy(gameObject);
        }
    }

    /** OnTriggerEnter, private void
	 * @param : Collider
	 * When the WindStrikeSwing detects something inside its trigger, we check if the collider is solid and if it is a monster.
	 * If it is, we contact the parent spell in order to apply an effect.
	 **/
    private void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger && col.gameObject.tag == "Monster")
        {
            _parentSpell.ApplyEffect(col);
        }
    }
}