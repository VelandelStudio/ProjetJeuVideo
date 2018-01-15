using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormWallSpell : Spell
{
    private GameObject _stormWallPrefab;

    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        _stormWallPrefab = LoadResource("StormWall");
        base.Start();
    }

    /// <summary>
    /// LaunchSpell method :
    /// Start calling the mother method
    /// then pop the wall in front of the played who lauch the wall and call OnSpellLaunched to start the cooldown
    /// and disable the using of the spell
    /// </summary>
    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (IsSpellLauncheable())
        {
            Instantiate(_stormWallPrefab, transform.position + transform.forward * 2, transform.rotation, transform);
            base.OnSpellLaunched();
        }
    }

    /** ApplyEffect, public void Method
    * @Params : Collider
    * When something passes through the wall or stay inside, this method is launched.
    * First of all, we detect if the collider is a trigger or not. Projectiles are triggers where as players and monsters are solids.
    * Then, is we detect something Ally (Projectile or Player), we apply the Status[1]
    * is we detect something Enemy (Projectile or Monster), we apply the Status[0]
    **/
    public void ApplyEffect(Collider collider)
    {
        if (collider.isTrigger)
        {
            if (collider.gameObject.tag == "AllyEntity")
            {
                ApplyStatus(Status[0], collider.transform);
                return;
            }

            if (collider.gameObject.tag == "EnemyEntity")
            {
                ApplyStatus(Status[1], collider.transform);
                return;
            }
        }
        else
        {
            if (collider.gameObject.tag == "Player")
            {
                ApplyStatus(Status[0], collider.transform);
                return;
            }

            if (collider.gameObject.tag == "Monster")
            {
                ApplyStatus(Status[1], collider.transform);
                return;
            }
        }
    }
}