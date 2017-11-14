using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class MeleAttack : MonoBehaviour {

    protected Collider _arme;

    protected virtual void Start()
    {
        _arme = GetComponent<Collider>();

        _arme.enabled = false;
    }

    protected abstract void OnTriggerEnter(Collider other);

    public void SwapEnableArmeCol()
    {
        _arme.enabled = !_arme.enabled;
    }

    public bool GetValueColArme()
    {
        return _arme.enabled;
    }
}
