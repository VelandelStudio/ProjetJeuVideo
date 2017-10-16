using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : LinearProjectile
{
    public override void ApplyEffect(Collider col)
    {
        Debug.Log("Touché " + col.name + " from " + launcher.name);
    }
}
