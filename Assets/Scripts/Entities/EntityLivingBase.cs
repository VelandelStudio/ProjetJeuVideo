using UnityEngine;
using System.Collections;
/** EntityLivingBase Abstract Class
 * This abstract class should ALWAYS be extended by classes which represents living entities.
 * At the moment it is able to handle HP Based behaviour such as damage, heals and death of the entity.
 * This class always need a collider to correctly apply damage.
 * Please note that all of these methos can be called ONLY if the entity is living (i.e. IsDead = false).
 **/
[RequireComponent(typeof(Collider))]
public abstract class EntityLivingBase : MonoBehaviour
{
    [SerializeField] private int HP;
	[SerializeField] private int RegenHpPerSec = 3;
    [SerializeField] private int HPMax;
	private float tick = 0;
	private bool IsDead;
	private bool StartDespawn;
    /** InitializeLivingEntity public method.
     * This method is used when an entity is created. It will set the parameters HP and HPMax of the entity.
     **/
    public void InitializeLivingEntity(int HP, int HPMax)
    {
        this.HP = HP;
        this.HPMax = HPMax;
		IsDead = false;
		StartDespawn = false;
    }

	/** DamageFor public method.
     * This method is used by other elements to apply Damages on the living Entity.
     * If the HP value reaches 0, the entity dies.
     **/
    public void DamageFor(int amount)
    {
		if(IsDead)
			return;
			
        HP -= amount;
        if (HP <= 0)
            EntityDies();
    }

    /** HealFor public method.
     * This method is used by other elements to apply heal on the living Entity.
     * The HP value can't be greater than HPMax
     **/
    public void HealFor(int amount)
    {
		if(IsDead)
			return;
			
        HP += amount;
        HP = HP > HPMax ? HPMax : HP;
    }
	
    /** Update protected method.
     * This method only count time ticks. Every seconds, it calls the HealFor Method in order to apply a RegenHpPerSec
     **/
	protected virtual void Update() {
		if(!IsDead) {
			tick += Time.deltaTime;
			if(tick >= 1) {
				tick --;
				HealFor(RegenHpPerSec);
			}
		}
		else
			if(StartDespawn)
				gameObject.transform.position = gameObject.transform.position - (Vector3.up * Time.deltaTime); 
	}
	
    /** InstantKill public method.
     * This method is used by other elements kill the entity in an instant.
     * This can be used by mechanisms (traps for example)
     **/
    public void InstantKill() 
    {
		if(IsDead)
			return;
			
        HP = 0;
        EntityDies();
    }

    /** EntityDies protected method.
     * This method is called when HP reaches 0.
     * When launched, this method should launch the death animation of the element.
     * Then it starts the coroutine DespawnEntity.
     **/
    protected void EntityDies()
    {
		if(IsDead)
			return;
			
		IsDead = true;	
        /// TODO : We need to add HERE the death Animation.
        StartCoroutine(DespawnEntity());
    }

    /** DespawnEntity protected coroutine.
     * This coroutine is called the entity should start to despawn.
     * When launched, this coroutine makes the entity triggerable which allows the player to walk on it (it is not an obstacle anymore).
     * After that (5sec), it applies a rigidbody on the entity : that make the entity go through the floor and then, it destroys the gameobject.
     **/
    protected IEnumerator DespawnEntity()
    {
	    Rigidbody rb = gameObject.GetComponent<Rigidbody>();
		if(rb != null)
			Destroy(rb);
			
        Collider col = gameObject.GetComponent<Collider>();
		if(col == null)
			col = gameObject.AddComponent<Collider>();
			
        col.isTrigger = true;
		
        yield return new WaitForSeconds(5);
        StartDespawn = true;
        yield return new WaitForSeconds(5);
		
        Destroy(this.gameObject);
    }
}
