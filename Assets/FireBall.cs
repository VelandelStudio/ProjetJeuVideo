using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();
        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            entityHit.DamageFor(100);
            IgniteStatus ignite = entityHit.gameObject.GetComponent<IgniteStatus>();
            if (ignite != null)
                Destroy(ignite);

            ignite = entityHit.gameObject.AddComponent<IgniteStatus>();
            transform.parent.GetComponent<ConflagrationSpell>().targets.Add(ignite);
        }

        Destroy(this.gameObject);
    }
}