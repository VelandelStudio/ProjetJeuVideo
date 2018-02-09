using UnityEngine;

/** DungeonLauncher Class
 * @Inherits MechanismBase
 * This Mecanism allows the player to launch a dunjon.
 * This Script should only be attached to the DungeonLauncher Prefab.
 **/
public class DungeonLauncher : ActivableMechanism
{
    [SerializeField]private GameObject Dungeon;
    /** Override Start Method
     * Adding the operations to get and build the MapGenerator with a Seed for a random Dungeon Shape
     */
    protected void Start()
    {
        isActivable = false;
    }

    /** ActivateDungeonLauncher, public void method
     * As we use a boolean to check if this Activable is ready or not, we need to use this method to make the Mechanisme activable
     * This Method should be called by the Summoning area only.
     **/
    public void ActivateDungeonLauncher()
    {
        isActivable = true;
        GetComponentInChildren<ParticleSystem>().Play();
    }

    /** DeactivateDungeonLauncher, public void method
     * As we use a boolean to check if this Activable is ready or not, we need to use this method to make the Mechanisme not activable
     * This Method should be called by the Summoning area only.
     **/
    public void DeactivateDungeonLauncher()
    {
        isActivable = false;
        GetComponentInChildren<ParticleSystem>().Stop();
    }

    /** ActivateInterractable Method
     * This Method overrides the parent one.
     * It detects if the mechanism as not been activated yet.
     * After activation it launches the dunjon and then Destroyes itself to provide multiple launches.
     * Warning ! Only the script will be Destroyed, not the GameObject
     */
    public override void ActivateInterractable(Collider other)
    {
        if (isActivable)
        {
            Debug.Log("Launching new Dunjon");
            Champion champion = other.gameObject.GetComponentInParent<Champion>();
            if (champion)
            {
                champion.ChampionDestroyable = false;
                Dungeon = Instantiate(Dungeon);
            }
        }
    }
}
