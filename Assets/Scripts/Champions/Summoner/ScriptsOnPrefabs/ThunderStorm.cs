using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** ThunderStorm public class.
 * This script is associated with a ThunderStorm instance prefab.
 **/
public class ThunderStorm : LinearProjectile {


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //GetComponentInChildren<ParticleSystem>().transform.parent = null;
    }

    public override void ApplyEffect(Collider col)
    {
        EntityLivingBase entityHit = col.gameObject.GetComponent<EntityLivingBase>();
        if (entityHit != null && entityHit.gameObject.tag == "Monster")
        {
            launcher.GetComponent<ThunderstormSpell>().ApplyEffectOnHit(entityHit);
            Debug.Log("Target Touched by ThunderStorm Projectile");
        }
    }

    public override void AttributeSpeedAndRange()
    {
        SpellRange = 20;
        ProjectileSpeed = 1000f;
    }
}
