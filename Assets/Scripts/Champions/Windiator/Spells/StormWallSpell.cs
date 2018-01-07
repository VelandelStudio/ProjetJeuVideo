using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormWallSpell : Spell
{
    protected override void Start()
    {
        base.Start();
        // @Veltouille : Tu verras dans cette méthode Start que j'ai ajouté une ionstanciation du mur, c'etait simplement pour tester le mur en lui même,
        // Tu peux faire péter ce block (ou le reprendre pour gérer ton instanciation + placement du wall depuis le spell).
        GameObject _throwable = (GameObject)Resources.Load(champion.Name + "/StormWall", typeof(GameObject));
        Instantiate(_throwable, transform.position + new Vector3(5, 0, 0), transform.rotation, this.transform);
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
            if (collider.gameObject.tag == "EnemyEntity")
            {
                ApplyStatus(Status[0], collider.transform);
                return;
            }

            if (collider.gameObject.tag == "AllyEntity")
            {
                ApplyStatus(Status[1], collider.transform);
                return;
            }
        }
        else
        {
            if (collider.gameObject.tag == "Monster")
            {
                ApplyStatus(Status[0], collider.transform);
                return;
            }

            if (collider.gameObject.tag == "Player")
            {
                ApplyStatus(Status[1], collider.transform);
                return;
            } 
        }
    }
}