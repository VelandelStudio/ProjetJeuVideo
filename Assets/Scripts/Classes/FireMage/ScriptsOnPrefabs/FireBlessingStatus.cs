using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlessingStatus : StatusBase, IBuff
{

    private ConflagrationSpell _conflagration;
    protected override void Start()
    {
        base.Start();
    }

    public override void OnStatusApplied()
    {
        Debug.Log("FireBlessingStatus Created !");
        _conflagration = GetComponentInParent<ConflagrationSpell>();
        _conflagration.CritSuccess = true;
    }

    public override void StatusTickBehaviour() { }

    /** DestroyStatus public virtual void
     * Instantly Destroy the gameObject that contains the Status
     **/
    public override void DestroyStatus()
    {
        _conflagration.CritSuccess = false;
        base.DestroyStatus();
    }
}
