using UnityEngine;

public class FireBallSpell : Spell
{
    public GameObject throwable;
    public Camera cameraPlayer;
    public Transform launcherTransform;

    private RaycastHit hit;
    private bool hasFoundHitPoint;
    private Vector3 target;
    private ParticleSystem particles;
    private GameObject throwableInstance;

    protected override void Start()
    {
        cameraPlayer = this.GetComponentInChildren<Camera>();
        throwable = (GameObject)Resources.Load("SpellPrefabs/FireBall", typeof(GameObject));
        Transform[] transformTab = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform tr in transformTab)
        {
            if (tr.gameObject.name == "EthanLeftHand")
            {
                launcherTransform = tr;
                break;
            }
        }
        SpellCD = 3.0f;
        base.Start();
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (!IsSpellLauncheable())
            return;

        hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(transform), cameraPlayer.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        if (hasFoundHitPoint)
            target = hit.point;
        else
            target = launcherTransform.position + cameraPlayer.transform.forward;
        throwableInstance = Instantiate(throwable, launcherTransform.position + cameraPlayer.transform.forward * 2, launcherTransform.rotation, this.transform);

        throwableInstance.transform.LookAt(target);
        throwableInstance.GetComponent<Rigidbody>().AddForce(throwableInstance.transform.forward * 1000);
        particles = throwableInstance.GetComponent<ParticleSystem>();
        particles.Play();
        base.OnSpellLaunched();
    }
}