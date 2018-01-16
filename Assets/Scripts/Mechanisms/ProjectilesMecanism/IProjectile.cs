using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IProjectile Interface
/// This interface describe as well the behavior of a projectile :
/// A method to launch it
/// A method to apply the effect of a projectile
/// There are also two properties (SpellRange and ProjectileSpeed), working with two methods (AccelerateProjectile and SlowDownProjectile).
/// These elements ensures that a projectile will always have a maximumRange and a speed that can be accelerated or slowed down.
/// </summary>
public interface IProjectile
{
    float SpellRange { get; }
    float ProjectileSpeed { get; }

    void LaunchProjectile();
    void ApplyEffect(Collider col);
    void AttributeSpeedAndRange();
    void AccelerateProjectile(float speed);
    void SlowDownProjectile(float speed);
}