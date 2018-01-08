using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpiritFavorStatus : StatusBase, IBuff
{
    private EntityLivingBase _entity;
    private List<GameObject> _statusApplier;

    /// <summary>
    /// Strat method from Unity
    /// At the start, we just want to get the EntityLivingBase parent of the Status applied
    /// Needed this GameObject to interract with it after OnStatusApplied
    /// </summary>
    protected override void Start()
    {
        _entity = GetComponentInParent<EntityLivingBase>();
    }


    /// <summary>
    /// OnStatusApplied method
    /// this method is here to apply the effects of the Status itself
    /// </summary>
    public override void OnStatusApplied()
    {
        Debug.Log("WindSpiritFavorStatus successfully appllied");
    }

    /// <summary>
    /// StatusTickBehaviour method
    /// Do nothing cause WindSpiritFavorStatus is not tickable
    /// </summary>
    public override void StatusTickBehaviour()
    {
        // No Ticks
    }
}
