using UnityEngine;

public class AutoAttackWarrior : AutoAttackBase
{
    protected override void Start()
    {
        base.Start();
    }

    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            Debug.Log("AutoAttackWarrior Launched");
            base.AutoAttack();
        }
    }

    protected override object[] getDescriptionVariables()
    {
        return new object[] { };
    }
}