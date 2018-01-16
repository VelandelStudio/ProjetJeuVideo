using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** GustSpell, public class
 * @extends : Spell
 * This spell is associated to the Windiator Champion. 
 * When used, it Applies a GustStatus to all Players in the range of the spell.
 **/
public class GustSpell : Spell
{

    /** LaunchSpell : public override void Method
	 * When launched, we get all the colliders inside an OverlapSphere.
	 * Then, we parse the Array to find Players and attribute to each one a GustStatus.
	 **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (IsSpellLauncheable())
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, float.Parse(OtherValues[0]));
            for (int i = 0; i < cols.Length; i++)
            {
                if (!cols[i].isTrigger && cols[i].gameObject.tag == "Player")
                {
                    ApplyStatus(Status[0], cols[i].gameObject.transform);
                }
            }

            base.OnSpellLaunched();
        }
    }
}