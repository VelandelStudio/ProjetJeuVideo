using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void LaunchProjectile();
    void ApplyEffect(Collider col);
}
