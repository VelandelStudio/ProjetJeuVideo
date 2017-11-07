using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** ConflagrationSpell Class, extends Spell
 * This spell is associated with the FireMageClass
 * The objectif of this spell is to propagate the IgniteStatus from targets to other targets, making it EXPLODE.
 * For example if 1 ennemy is circled by 4 other, the ConflagrationSpell will explode the first ennemy.
 * The Ignite status on the first ennemy will be destroyed but 4 new ignite status will be applied to the 4 other monsters.
 * Inversely, if we do it again, 4 explosions will occur, destroying 4 ignites status but only the one monster who had not it will receive a ignite status.
 **/
public class ConflagrationSpell : Spell
{

    public List<IgniteStatus> Targets = new List<IgniteStatus>(); //List of all targets with an IgniteStatus.
    public List<Collider> TargetsExploded = new List<Collider>(); //List of all targets that have already exploded when we activate this spell.
    public bool CritSuccess = false;

    /** Start : protected override void Method
	 * The Start Method is used here to apply the CD of the spell and launch the mother Method to initialize the spell.
	 **/
    protected override void Start()
    {
        spellCD = 12.0f;
        base.Start();
    }

    /** Update : protected override void Method
	 * The Update Method is used in two ways. 
	 * First, we ensure that our target Vector does not contain null Ignite status. If we found some, we remove them from the vector.
	 * Then, we ensure that the cooldown is reseting properly by the mother method.
	 **/
    protected override void Update()
    {
        Targets.RemoveAll(IgniteStatus => IgniteStatus == null);
        base.Update();
    }

    /** LaunchSpell : public override void Method
	 * The LauncheSpell Method is called by the abstract Class Classe when the player press the key associated to the spell.
	 * First at all, we launch the mother method to initialize the spell launching.
     * If the spell is Launcheable, we create a new List that will contains the targets freshly affected by new IgniteStatus.
	 * Then, we explode every targets already affected by the IgniteStatus (making damage to this target). In order to get the new targets that are getting new ignite status, we use an overlapSphere.
	 * For every collider touched by the overlap sphere we only want to get Colliders of Monsters that have not explosed and are not explosable. 
	 * Once we get the correct targets, We first apply an explosion damage.
	 * Then, we launch a random to apply or not ignite on the correct target. Please note that the random can be ignored if CritSuccess was set to true by the FireBlessingSpell.
	 * If the random is a success, we apply a fresh ignite on the target (by adding a new one or reseting the current one).
	 * After that, we add the new Frsh targets with fresh ignites to the targetsToAdd List. Then, we refresh the targetsExploded list, by adding the target that just exploded
	 * These steps ensures that targets can not explode 2 times and get a new fresh igniteStatus after an explosion.
	 * Then, we clear the old ignites on explosed targets and we add the new fresh targets to the target List. We also ensure that CritSuccess is reset to false.
	 * Finally we call the OnSpellLaunched mother method to tell the spell is not in use anymore.
	 **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (!IsSpellLauncheable())
            return;

        List<IgniteStatus> targetsToAdd = new List<IgniteStatus>();

        foreach (IgniteStatus target in Targets)
        {
            EntityLivingBase entity = target.GetComponent<EntityLivingBase>();
            entity.DamageFor(20);
            Collider[] cols = Physics.OverlapSphere(entity.transform.position, 10f);
            TargetsExploded.Add(target.GetComponent<Collider>());
            target.ExplodeIgniteStatus();
            foreach (Collider col in cols)
            {
                if (col.gameObject.GetComponent<EntityLivingBase>()
                    && !col.isTrigger
                    && !TargetsExploded.Contains(col)
                    && !Targets.Contains(col.GetComponent<IgniteStatus>()))
                {
                    col.gameObject.GetComponent<EntityLivingBase>().DamageFor(10);
                    if (Random.Range(0, 100) < 50 || CritSuccess)
                    {
                        IgniteStatus ignite = col.gameObject.GetComponent<IgniteStatus>();
                        if (ignite != null)
                        {
                            ignite.ResetStatus();
                        }
                        else
                        {
                            ignite = col.gameObject.AddComponent<IgniteStatus>();
                        }
                        targetsToAdd.Add(ignite);
                    }

                }
            }
        }
        TargetsExploded.Clear();
        Targets.Clear();
        Targets = targetsToAdd;
        CritSuccess = false;
        base.OnSpellLaunched();
    }

    /** IsSpellLauncheable : protected override bool Method
	 * This method override the parent one adding a condition to the utilisation of this spell :
	 * If the targets List is empty (i.e. if there are no igniteStatus in the game), the spell can not be launched.
	 **/
    protected override bool IsSpellLauncheable()
    {
        return Targets.Count >= 1 && base.IsSpellLauncheable();
    }

    /** AvailableForGUI public override bool Method,
	 * The GUICD image is dependant on the number of Ignites in ythe game.
	 **/
    public override bool AvailableForGUI()
    {
        return Targets.Count >= 1;
    }
}