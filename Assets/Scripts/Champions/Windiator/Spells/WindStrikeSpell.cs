using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** WindStrikeSpell, public class
 * @extends : Spell
 * This spell is associated to the Windiator champion. 
 * The spell is a slash attack that applies a WindPushStatus on targets caught in the slash.
 **/
public class WindStrikeSpell : Spell
{

    private GameObject _slash;

    /** Start, protected override void method,
	 * Forst at all, we try to get the prefab of the pivot that will handle the WindStrikeSwing.
	 * Then we call the base Start.
	 **/
    protected override void Start()
    {
        _slash = (GameObject)Resources.Load(champion.Name + "/WindStrikeSwing", typeof(GameObject));
        base.Start();
    }

    /** LaunchSpell, public override void method,
	 * When launched, we instantiate the Slash prefab.
	 **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (IsSpellLauncheable())
        {
            Instantiate(_slash, transform.position + (transform.forward * 0.5f)
                + (transform.up * 1.2f), transform.rotation, transform);
            base.OnSpellLaunched();
        }
    }

    /** ApplyEffect, public override void method,
	 * @param : Collider
	 * This public method should always be called by the WindStrikePivot prefab when it hits a Monster.
	 * We get the EntityLivingBase Instance on the monster and we apply on it damages and a WindPushStatus.
	 **/
    public void ApplyEffect(Collider collider)
    {
        collider.GetComponent<EntityLivingBase>().DamageFor(Damages[0]);
        ApplyStatus(Status[0], collider.transform);
    }
}