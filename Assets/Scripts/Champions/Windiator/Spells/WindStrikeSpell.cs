using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindStrikeSpell : Spell
{

    private GameObject _throwable;
    private GameObject _throwableInst;

    protected override void Start()
    {
        _throwable = (GameObject)Resources.Load(champion.Name + "/WindStrikePivot", typeof(GameObject));
        base.Start();
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (IsSpellLauncheable())
        {
            _throwableInst = Instantiate(_throwable, transform.position + (transform.forward * 0.5f)
                + (transform.up * 1.2f), transform.rotation, transform);
            base.OnSpellLaunched();
        }
    }

    public void ApplyEffect(Collider collider)
    {
        Debug.Log(collider.name + " is touched !");
    }
}
