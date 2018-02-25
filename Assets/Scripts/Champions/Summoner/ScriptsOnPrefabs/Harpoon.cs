using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Harpoon : LinearProjectile
{
    private HarpoonSpell _parentSpell;
    private BoxCollider _collider;

    public override void ApplyEffect(Collider collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();

        if (entityHit.gameObject.tag == "Monster")
        {
            launcher.GetComponent<HarpoonSpell>().ApplyEffectOnHit(entityHit);
        }
    }

    public override void AttributeSpeedAndRange()
    {
        SpellRange = 20;
        ProjectileSpeed = 1000f;
    }
}
