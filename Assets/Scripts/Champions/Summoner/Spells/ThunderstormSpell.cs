using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormSpell : Spell
{
    private Camera _cameraPlayer;
    private GameObject _thunderStormPS;
    private int stacks;

    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>();
        _thunderStormPS = LoadResource("ThunderStorm");
        base.Start();
    }

    public override void LaunchSpell()
    {
        if (IsSpellLauncheable())
        {
            RaycastHit hit; // raycast object for aiming the spell with the mouse

            /* if the raycast touch something launch the spell else return without cooldown */
            bool hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(transform), _cameraPlayer.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            if (hasFoundHitPoint)
            {
                Instantiate(_thunderStormPS, hit.point, Quaternion.identity);
                base.LaunchSpell();
                Debug.Log("sort lancé");
            }
            else
            {
                return;
            }

            EntityLivingBase entityHit = hit.collider.gameObject.GetComponent<EntityLivingBase>();
            if (entityHit != null && entityHit.gameObject.tag == "Monster")
            {
                ApplyEffectOnHit(entityHit);
                //Debug.Log("Target Touched by ThunderStorm Raycast");
            }
        }
    }

    public void ApplyEffectOnHit(EntityLivingBase entityHit)
    {
        stacks = GetComponentInChildren<VoltageStatus>().GetNumberOfStacks();
        if (stacks < 5)
        {
            entityHit.DamageFor(Damages[0]);
        }
        if (stacks >= 5 && stacks < 10 )
        {
            entityHit.DamageFor(Damages[1]);
        }
        if (stacks >= 10 && stacks < 20)
        {
            entityHit.DamageFor(Damages[2]);
        }
        if (stacks >= 20)
        {
            entityHit.DamageFor(Damages[3]);
        }

        GetComponentInChildren<VoltageStatus>().RemoteStacks();
    }
}
