using UnityEngine;

/** SimpleNPC class
 * This script should be attached to every PNJ in the game. As a matter of fact, we consider that a PNJ in a neutral Entity.
 * It does not attack the player and does not run away from him.
 * The SimpleNPC extends the abstract class EntityLivingBase and implements the IInterractableEntity.
 * In this way, a PNJ can be killed or healed (in Escort quest for example) and the player can interract whith him (for shops, quests etc..). 
 * Finally, a PNJ Requires a Component SphereCollider to detect the nearest player. 
 **/
[RequireComponent(typeof(SphereCollider))]
public class SimpleNPC : EntityLivingBase, IInterractableEntity
{

    /** Awake protected override method
    * Before everything starts, we ensure that the SphereCollider is a trigger. 
    * In that way, the player can be detected when he goes through the trigger.
	* We obviously call the mother Awake.
    **/
    protected override void Awake()
    {
        base.Awake();
        GetComponent<SphereCollider>().isTrigger = true;
    }

    /** Start private method
     * This Method calls the InitializeLivingEntity from the EntityLivingBase class.
     * It initialize the HP and HPMax of the entity 
     **/
    private void Start()
    {
        InitializeLivingEntity(100000, 100000);
    }

    /** OnTriggerStay private method
     * This Method is called by the Collider Trigger every frame it detects something inside the trigger volume.
     * If the entity detected is a player, the NPC rotates (Only around the Y Axis) in order to watch the player.
     * If the entity is something else, the NPC does nothing.
     **/
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
            return;

        Transform target = other.transform;
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }

    /** ActivateInterractable Method
     * The method is created by the implementation of IInterractableEntity
     * This method can be launched by another script in order to Interract with the NPC
     * A.T.M., the only way to launch this method is when the GameObjectDetector detects the Entity and if the Player press the Action button.
     **/
    public void ActivateInterractable()
    {
        Debug.Log("Hello Sir ! I'm the first PNJ in this game !");
    }

    /** DisplayTextOfInterractable Method
    * The method is created by the implementation of IInterractableEntity
    * This method can be launched by another script in order to display an information on the screen.
    * For example, the Action button to interract with the NPC or a bubble with some text above the NPC. 
    **/
    public void DisplayTextOfInterractable()
    {
        Debug.Log("Press " + InputsProperties.Activate.ToString() + " to activate.");
    }
}
