using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindStrikeSwing : MonoBehaviour {

    private WindStrikeSpell _parentSpell;
    private Animator _anim;
    private BoxCollider _collider;
    private float timeCompare = 0f;

    void Start () {

        _parentSpell = GetComponentInParent<WindStrikeSpell>();
        _collider = GetComponent<BoxCollider>();
        _anim = GetComponent<Animator>();
        _anim.SetTrigger("WindStrike");
    }

    void Update()
    {
        if (_collider.enabled == false)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        EntityLivingBase entityHit = col.gameObject.GetComponent<EntityLivingBase>();

        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
            _parentSpell.ApplyEffect(col);
        }
    }
}
