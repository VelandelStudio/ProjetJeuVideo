using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpiritFavorStatus : StatusBase, IBuff
{
    private EntityLivingBase _entity;
    private List<GameObject> _statusApplier;

    protected override void Start()
    {
        _entity = GetComponentInParent<EntityLivingBase>();
    }

    public override void OnStatusApplied()
    {
        Debug.Log("WindcSpiritFavorStatus successfully appllied");
    }

    public override void StatusTickBehaviour()
    {
        // No Ticks
    }
}
