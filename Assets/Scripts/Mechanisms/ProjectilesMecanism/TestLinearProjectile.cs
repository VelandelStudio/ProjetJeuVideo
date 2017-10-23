using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TestLinearProjectile Class inherits LinearProjectile
/// This is a simple Class in order to test easily the LinearProjectile mecanism 
/// </summary>
public class TestLinearProjectile : LinearProjectile
{
    /// <summary>
    /// ApplyEffect method -> implementation of the abstract method in LinearProjectile mother Class
    /// Just show that it works with the Debug function
    /// </summary>
    /// <param name="col">is the collider touch by the projectile</param>
    public override void ApplyEffect(Collider col)
    {
        Debug.Log("Touché " + col.gameObject.name + " from " + launcher.name);
    }
}
