using UnityEngine;

/** DungeonLauncher Class
 * @Inherits MechanismBase
 * This Mecanism allows the player to launch a dunjon.
 * This Script should only be attached to the DungeonLauncher Prefab.
 **/
public class DungeonLauncher : MechanismBase
{
    private MapGenerator generator;     //Used to create the dungeon

    /** Override Start Method
     * Adding the operations to get and build the MapGenerator with a Seed for a random Dungeon Shape
     */
    protected override void Start()
    {
        base.Start();
        isActivable = false;

        generator = GetComponent<MapGenerator>();
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
    public override void ActivateInterractable()
    {
        if (!isActivated && isActivable)
        {
            Debug.Log("Launching new Dunjon");

            // Generation du dungeon
            generator.GenerationMap();

            // Placement of the Player
            SpawnPos();

        }
    }

    /** SpawnPos Method
     * This method will get the transform position of a random room in the MapGenerator
     * The center of this room is the spawn position of the player and will teleport this one inside.
     * Then the room is removed from the RoomsList
     */
    private void SpawnPos()
    {
        //int rand = Random.Range(0, generator.rooms.Count);

        //Transform spawnRoom = generator.rooms[rand].transform;
        //generator.rooms.RemoveAt(rand);

        GameObject player = GameObject.FindWithTag("Player");
        //player.transform.position = spawnRoom.position;
    }
}
