using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.AI;
/** EntityLivingBase Abstract Class
* This abstract class should ALWAYS be extended by classes which represents living entities.
* At the moment it is able to handle HP Based behaviour such as damage, heals and death of the entity.
* This class always need a collider to correctly apply damage.
* Please note that all of these methos can be called ONLY if the entity is living (i.e. IsDead = false).
**/
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Characteristics))]

public abstract class EntityLivingBase : MonoBehaviour
{
    [SerializeField] private int _HP;
    [SerializeField] private int _regenHpPerSec = 3;
    [SerializeField] private int _HPMax;
    public Transform damageTransform;
    public GameObject damagePrefab;
    public bool IsDead { get { return _HP <= 0; } }
    public bool IsAlive { get { return !IsDead; } }
    private bool _startDespawn;
    protected Characteristics characteristics;
    public Characteristics Characteristics { get { return characteristics; } }

    /** Awake, protected virtual void
	 * First of all, we are checking that all LivingEntities that are not players have a Rigidbody that fits with the game rules. Which are.
	 * Living Entities must have a rigidbody which uses gravity and is not kinematic.
	 * Position are not frozen.
	 * Only X and Z Rotation are frozen.
     * Be aware that RigidbodyConstraints are sum of bytes, this is why we always use a simple | in test.
	 **/
    protected virtual void Awake()
    {
        if (gameObject.tag != "Player")
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = (Rigidbody)gameObject.AddComponent(typeof(Rigidbody));
                Debug.Log(gameObject.name + " est une EntityLivingBase sans Rigidbody. Il a été ajouté automatiquement. Merci de corriger le prefab.");
            }

            if (!rb.useGravity)
            {
                rb.useGravity = true;
                Debug.Log(gameObject.name + " porte un Rigidbody sans gravité. Gravité activée automatiquement. Merci de corriger le prefab.");
            }

            if (rb.isKinematic)
            {
                rb.isKinematic = false;
                Debug.Log(gameObject.name + " porte un Rigidbody kinematic. Kinematic désactivé automatiquement. Merci de corriger le prefab.");
            }

            if (rb.constraints != (RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ))
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                Debug.Log(gameObject.name + " porte un Rigidbody dont l'une des Constraints est érronée. Les positions ont été Defreeze automatiquement."
                         + "Seuls les roations en X et Z ont été Freeze. Merci de corriger le prefab.");
            }
        }
        characteristics = GetComponent<Characteristics>();
    }

    protected virtual void Start() { }

    /** InitializeLivingEntity public method.
    * This method is used when an entity is created. It will set the parameters HP and HPMax of the entity.
    **/
    public void InitializeLivingEntity(int HP, int HPMax)
    {
        _HP = HP;
        _HPMax = HPMax;
        _startDespawn = false;
        InvokeRepeating("AutoRegenHP", 1f, 1f);
    }


    /** DamageFor public method.
    * This method is used by other elements to apply Damages on the living Entity.
    * If the HP value reaches 0, the entity dies.
    * This method also displays damages on the screen.
    **/ 
    public void DamageFor(int amount)
    {
        Debug.Log("Amount = " + amount);

        if (IsAlive)
        {
            _HP -= (int)(amount - characteristics.Defense);
            if (IsDead)
            {
                EntityDies();
            }
        }

        GameObject damagePopup = Instantiate(damagePrefab, damageTransform.position, damageTransform.rotation, damageTransform);
        damagePopup.GetComponentInChildren<Text>().text = amount.ToString();
        Destroy(damagePopup, 1f);
    }

    /** HealFor public method.
    * This method is used by other elements to apply heal on the living Entity.
    * The HP value can't be greater than HPMax
    **/
    public void HealFor(int amount)
    {
        if (IsAlive)
        {
            _HP += amount;
            _HP = _HP > _HPMax ? _HPMax : _HP;
        }
    }

    protected virtual void AutoRegenHP()
    {
        if (IsAlive)
        {
            HealFor(_regenHpPerSec);
        }
        else
        {
            CancelInvoke("AutoRegenHP");
        }
    }

    /** Update protected method.
    * This method only count time ticks. Every seconds, it calls the HealFor Method in order to apply a RegenHpPerSec
    **/
    protected virtual void Update()
    {
    }

    /** InstantKill public method.
     * This method is used by other elements kill the entity in an instant.
     * This can be used by mechanisms (traps for example)
     **/
    public void InstantKill()
    {
        if (IsAlive)
        {
            _HP = 0;
            EntityDies();
        }
    }

    /** EntityDies protected method.
     * This method is called when HP reaches 0.
     * When launched, this method should launch the death animation of the element and clear all Status present on the entity.
     * Then it starts the coroutine DespawnEntity.
     **/
    protected void EntityDies()
    {
        IStatus[] status = GetComponentsInChildren<IStatus>();
        foreach (IStatus s in status)
        {
            s.DestroyStatus();
        }
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
        NavMeshAgent navMesh = GetComponent<NavMeshAgent>();
        navMesh.enabled = false;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Vector3 force = new Vector3(transform.forward.x * -20, transform.up.y * 10, transform.forward.z * -20);
        GetComponent<Rigidbody>().AddForceAtPosition(force, transform.position, ForceMode.Impulse);

        Collider col = gameObject.GetComponent<Collider>();
        if (col == null)
        {
            col = gameObject.AddComponent<Collider>();
        }
        col.isTrigger = true;

        yield return new WaitForSeconds(5);
        _startDespawn = true;
        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }
}