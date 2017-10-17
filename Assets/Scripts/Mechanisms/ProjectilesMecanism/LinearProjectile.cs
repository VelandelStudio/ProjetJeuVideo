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
public abstract class LinearProjectile : MonoBehaviour, IProjectile {

    protected Transform launcher;       // To get the GameObject whitch launch the projectile
    protected float timeOfFly = 2f;     // Time before Destruction of the Projectile (refacto later)

    /// <summary>
    /// Start method from Unity to initialize a ProjectileThe LauncheSpell
    /// called when the player press the key associated to the spell.
    /// This method store the parent at the origin of the Instantiation of the projectil.
    /// Detach itself from the parent to have a good linear pathway
    /// And Start the Launch method before being destroy by the time
    /// </summary>
    protected void Start()
    {
        launcher = transform.parent;
        GetComponent<Collider>().isTrigger = true;
        transform.parent = null;

        LaunchProjectile();
        Destroy(gameObject, timeOfFly);
    }

    /// <summary>
    /// OnTriggerEnter method from Unity called after a collision between two colliders
    /// the idea of a projectile is to check if the type of collision is type of EntityLivingBase
    /// In this case it launches the applyEffect implemented in the child Class
    /// In any type of collision the projectile will be destroyed at the end
    /// </summary>
    /// <param name="col"></param>
    protected void OnTriggerEnter(Collider col)
    {
        EntityLivingBase eHit = col.gameObject.GetComponent<EntityLivingBase>();

        if (eHit != null && eHit.gameObject.tag != "player")
        {
            ApplyEffect(col);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// ApplyEffect abstract method implemented by IProjectile
    /// Necessary to implement this method in a child Class
    /// This method describe the effects applied to the target
    /// It can also apply effect to the Launcher gameObject in some cases
    /// </summary>
    /// <param name="col"></param>
    public abstract void ApplyEffect(Collider col);

    /// <summary>
    /// LaunchProjectile method implemented by IProjectile
    /// This target can be a HitPoint from a raycast or a point on the line from the player to the
    /// Camera.trasnform.forward (i.e. is the raycast does not intercept an entity)
    /// After that, we instantiate a Projectile, make it look at the target
    /// Apply a force to it and launch the particle system associated to the prefab (if exists)
    /// </summary>
    public void LaunchProjectile()
    {
        Camera cameraPlayer = launcher.GetComponentInChildren<Camera>();
        Vector3 target;
        RaycastHit hit;

        bool hasFoundHitPoint = Physics.Raycast(cameraPlayer.GetComponent<GameObjectDetector>().OriginPoint,
                                                cameraPlayer.transform.forward,
                                                out hit,
                                                Mathf.Infinity,
                                                Physics.DefaultRaycastLayers,
                                                QueryTriggerInteraction.Ignore);

        if (hasFoundHitPoint)
        {
            target = hit.point;
        }
        else
        {
            target = transform.position + cameraPlayer.transform.forward * 10;
        }

        transform.LookAt(target);
        GetComponent<Rigidbody>().AddForce(transform.forward * 1000);

        ParticleSystem particles = GetComponent<ParticleSystem>();

        if (particles != null)
        {
            particles.Play();
        }
    }
}
