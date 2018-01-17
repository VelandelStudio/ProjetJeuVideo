using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPassive : PassiveBase
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("Default Passive Loaded !");
    }
}