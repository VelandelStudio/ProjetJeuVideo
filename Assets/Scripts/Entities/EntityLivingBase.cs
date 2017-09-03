using UnityEngine;
using System.Collections;

/** EntityLivingBase Abstract Class
 * This abstract class should ALWAYS be extended by classes which represents living entities.
 * At the moment it is able to handle HP Based behaviour such as damage, heals and death of the entity.
 * This class always need a collider to correctly apply damage.
 **/
[RequireComponent(typeof(Collider))]
public abstract class EntityLivingBase : MonoBehaviour
{
    [SerializeField] private int HP;
    [SerializeField] private int HPMax;

    /** InitializeLivingEntity public method.
     * This method is used when an entity is created. It will set the parameters HP and HPMax of the entity.
     **/
    public void InitializeLivingEntity(int HP, int HPMax)
    {
        this.HP = HP;
        this.HPMax = HPMax;
    }

    public void DamageFor(int amount)
    {
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
        HP += amount;
        HP = HP > HPMax ? HPMax : HP;
    }

    /** InstantKill public method.
     * This method is used by other elements kill the entity in an instant.
     * This can be used by mechanisms (traps for example)
     **/
    public void InstantKill() 
    {
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
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
        yield return new WaitForSeconds(5);
        gameObject.AddComponent<Rigidbody>();

        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
