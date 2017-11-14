using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindiatorSimpleAttack : MeleAttack {

    [SerializeField]
    private int _damages = 50;

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello");

        EntityLivingBase entityHit = other.gameObject.GetComponent<EntityLivingBase>();

        if (entityHit != null && entityHit.tag != "player")
        {
            entityHit.DamageFor(_damages);
            Debug.Log(_damages + " damages on " + entityHit.name);

            _arme.enabled = false;
        }
    }
}
