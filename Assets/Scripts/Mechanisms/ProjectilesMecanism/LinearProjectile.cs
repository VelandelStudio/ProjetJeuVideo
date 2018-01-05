using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LinearProjectile Abstract Class inherits MonoBehavior implements IProjectile
/// This class is the new mecanism of Projectile Behavior
/// The idea is to make a LinearProjectile independant from the gameObject whitch launch it
/// Storing all the data needed to apply effect to the correct gameObject
/// 
/// The Projectile gameObject requiers a collider to detect collision and a rigidbody to apply forces
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class LinearProjectile : MonoBehaviour, IProjectile
{

    protected Transform launcher;
    protected EntityLivingBase eHit;

    protected float timeOfFly;
    protected float spellRange;
    protected float projectileSpeed;

    public float SpellRange
    {
        get { return spellRange; }
        protected set { spellRange = value; }
    }

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
        protected set { projectileSpeed = value; }
    }

    protected Vector3 target;
    protected Vector3 origin;
    protected Rigidbody rb;
    protected Vector3 startVelocity;

    /// <summary>
    /// Start method from Unity to initialize a ProjectileThe LauncheSpell
    /// called when the player press the key associated to the spell.
    /// This method store the parent at the origin of the Instantiation of the projectile.
    /// Detach itself from the parent to have a good linear pathway and Start the Launch method.
    /// </summary>
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        AttributeSpeedAndRange();
        launcher = transform.parent;
        GetComponent<Collider>().isTrigger = true;
        origin = transform.position;
        transform.parent = null;

        if (target == Vector3.zero)
        {
            LaunchProjectile();
        }
        else
        {
            LaunchProjectile(target);
        }
        timeOfFly = CalculTimeOfFly(ProjectileSpeed, SpellRange);
    }

    /** Update, protected virtual void 
	 * The Update Method is used to check if the LinearProjectile has reached the SpellRange.
     * If the Distance between the origin and the current position is greater than the SpellRange, 
     * then, the GameObject is Destroyed.
	 **/
    protected virtual void Update()
    {
        if (Vector3.Distance(origin, transform.position) >= SpellRange)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// OnTriggerEnter method from Unity called after a collision between two colliders
    /// the idea of a projectile is to check if the type of collision is type of EntityLivingBase
    /// In this case it launches the applyEffect implemented in the child Class
    /// In any type of collision the projectile will be destroyed at the end
    /// </summary>
    /// <param name="col">is the collider touch by the projectile</param>
    protected void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger)
        {
            eHit = col.gameObject.GetComponent<EntityLivingBase>();

            if (eHit != null && eHit.gameObject.tag != "player")
            {
                ApplyEffect(col);
            }

            AdditionalEffects();
            Destroy(gameObject);
        }
    }

    /** AttributeSpeedAndRange, public abstract void,
	 * This abstract method should be implemented in all daughter classes. 
	 * This method is used to attribute an initial SpellRange and ProjectileSpeed
	 **/
    public abstract void AttributeSpeedAndRange();


    /// <summary>
    /// ApplyEffect abstract method implemented by IProjectile
    /// Necessary to implement this method in a child Class
    /// This method describe the effects applied to the target
    /// It can also apply effect to the Launcher gameObject in some cases
    /// </summary>
    /// <param name="col">is the collider touch by the projectile</param>
    public abstract void ApplyEffect(Collider col);

    /// <summary>
    /// LaunchProjectile method implemented by IProjectile
    /// This target can be a HitPoint from a raycast or a point on the line from the player to the
    /// Camera.trasnform.forward (i.e. is the raycast does not intercept an entity)
    /// After that, we instantiate a Projectile, make it look at the target
    /// Apply a force to it and launch the particle system associated to the prefab (if exists)
    /// </summary>
    public virtual void LaunchProjectile()
    {
        Camera cameraPlayer = launcher.GetComponentInChildren<Camera>();
        Vector3 target;
        RaycastHit hit;

        bool hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(launcher), cameraPlayer.transform.forward,
                                                out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        if (hasFoundHitPoint)
        {
            target = hit.point;
        }
        else
        {
            target = transform.position + cameraPlayer.transform.forward * 10;
        }

        transform.LookAt(target);
        GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        ParticleSystem particles = GetComponent<ParticleSystem>();

        if (particles != null)
        {
            particles.Play();
        }
    }

    /** LaunchProjectile protected virtual void 
	 * @params : Vector3 target
	 * This method is used to override the classical way of launching projectiles. 
	 * It is launched by daughter classes when they are linear projectiles bu follow a different way than the classical "From Hand to Point".
	 **/
    protected virtual void LaunchProjectile(Vector3 target)
    {
        transform.LookAt(target);
        GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        ParticleSystem particles = GetComponent<ParticleSystem>();

        if (particles != null)
        {
            particles.Play();
        }
    }
    /// <summary>
    /// CalculTimeOfFly method calculates automatically at the instanciation of a projectile the time of fly
    /// depending on speed and the distance
    /// </summary>
    /// <param name="speed">float value used also to apply force to the bullet</param>
    /// <param name="distance">float value, length that a projectile can travel </param>
    /// <returns>the time that travel a projectile before autodestuction</returns>
    protected float CalculTimeOfFly(float speed, float distance)
    {
        return distance / speed;
    }

    /** AccelerateProjectile, public void 
    * @param : float
    * This method should be called by toher scripts to accelerate the LinearProjectile
    **/
    public void AccelerateProjectile(float speed)
    {
        if (startVelocity == Vector3.zero) //|| startVelocity == Vector3.zero)
        {
            startVelocity = rb.velocity;
        }

        rb.velocity += startVelocity * (speed / 100.0f);
    }

    /** SlowDownProjectile, public void 
    * @param : float
    * This method should be called by toher scripts to slow down the LinearProjectile
    **/
    public void SlowDownProjectile(float speed)
    {
        if (startVelocity == Vector3.zero)
        {
            startVelocity = rb.velocity;
        }

        rb.velocity -= startVelocity * (speed / 100.0f);
    }

    protected virtual void AdditionalEffects() { }
}