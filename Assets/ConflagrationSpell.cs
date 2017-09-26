using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConflagrationSpell : Spell
{

    public List<IgniteStatus> targets = new List<IgniteStatus>();
    public List<Collider> targetsExploded = new List<Collider>();

    public bool CritSuccess = false;
    protected override void Start()
    {
        SpellCD = 12.0f;
        base.Start();
    }

    protected override void Update()
    {
        targets.RemoveAll(IgniteStatus => IgniteStatus == null);
        base.Update();
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (!IsSpellLauncheable())
            return;

        List<IgniteStatus> targetsToAdd = new List<IgniteStatus>();

        foreach (IgniteStatus target in targets)
        {
            EntityLivingBase entity = target.GetComponent<EntityLivingBase>();
            entity.DamageFor(20);
            Collider[] cols = Physics.OverlapSphere(entity.transform.position, 10f);
            foreach (Collider col in cols)
            {
                if (col.gameObject.GetComponent<EntityLivingBase>() && col.gameObject != target.gameObject && !col.isTrigger
                    && !targetsExploded.Contains(col) && !targets.Contains(col.GetComponent<IgniteStatus>()))
                {
                    if (Random.Range(0, 100) < 50 || CritSuccess)
                    {
                        if (col.gameObject.GetComponent<IgniteStatus>() != null)
                            Destroy(col.gameObject.GetComponent<IgniteStatus>());

                        IgniteStatus ignite = col.gameObject.AddComponent<IgniteStatus>();
                        col.gameObject.GetComponent<EntityLivingBase>().DamageFor(10);
                        targetsToAdd.Add(ignite);
                    }
                }
            }
            targetsExploded.Add(target.GetComponent<Collider>());
            Destroy(target);
        }
        targetsExploded.Clear();
        targets.Clear();
        targets = targetsToAdd;
        CritSuccess = false;
        base.OnSpellLaunched();
    }

    protected override bool IsSpellLauncheable()
    {
        return targets.Count >= 1 && base.IsSpellLauncheable();
    }
}