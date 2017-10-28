using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IProjectile Interface
/// This interface describe as well the behavior of a projectile :
/// A method to launch it
/// A method to apply the effect of a projectile
/// </summary>
public interface IProjectile
{
    void LaunchProjectile();
    void ApplyEffect(Collider col);
}
