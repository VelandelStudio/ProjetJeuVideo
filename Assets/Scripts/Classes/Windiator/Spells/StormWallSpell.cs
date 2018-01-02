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
        Debug.Log(champion.Name + "/StormWall");
        Instantiate(_throwable, transform.position + new Vector3(5, 0, 0), transform.rotation, this.transform);
    }

    /** ApplyEffect, public void Method
	 * @Params : Collider
	 * When something passes through the wall or stay inside, this method is launched.
	 * If we detect a Player, the Status[1] is applied on it (InsideStormStatus).
	 * If we detect a non-player Entity, the Status[0] is applied on it (BackWindStatus).
	 * If we detect a Player Projectile, the Status[1] is applied on it (InsideStormStatus).
	 * If we detect a non-Player Projectile, the Status[0] is applied on it (BackWindStatus).
	 **/
    public void ApplyEffect(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            ApplyStatus(Status[1], collider.transform);
            return;
        }

        EntityLivingBase entity = collider.GetComponent<EntityLivingBase>();
        if (entity != null && !collider.isTrigger)
        {
            ApplyStatus(Status[0], entity.transform);
            return;
        }

        IProjectile projectile = collider.GetComponent<IProjectile>();
        if (projectile != null)
        {
            if (collider.gameObject.tag == "Player")
            {
                ApplyStatus(Status[1], collider.transform);
            }
            else
            {
                ApplyStatus(Status[0], collider.transform);
            }
        }
    }
}
