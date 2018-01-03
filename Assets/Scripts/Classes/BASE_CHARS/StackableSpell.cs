using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** StackableSpell, public abstract class
 * @Extends Spell
 * This class is specific to spells that have a Stackable Option.
 * It is used as a common spell but we also have a second coroutine that handles the stack CD.
 **/
public abstract class StackableSpell : Spell
{

    public float CurrentNumberOfStacks;
    public float StackCD;
    public float CurrentStackCD;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        CurrentNumberOfStacks = NumberOfStacks;
        StackCD = CoolDownValue;
    }

    /** OnSpellLaunched protected virtual void Method,
	 * This method should be called when the spell has reached is final statement.
	 * When the spell was used, one stack is lost.
	 **/
    protected override void OnSpellLaunched()
    {
        CoolDownValue = spellGCD;
        CurrentCD = spellGCD;
        spellInUse = false;
        CurrentNumberOfStacks--;
    }

    /** Update, protected override void
	 * Same as a basic Update from the Spell mother class but we also check the number of stacks.
	 * If at least a stack is missing, then we reload the stack.
	 * If there are no stacks anymore, then the CD of the spell is replaced from the GCD to the StackCD
	 **/
    protected override void Update()
    {
        base.Update();
        if (CurrentNumberOfStacks < NumberOfStacks)
        {
            if (CurrentStackCD == 0)
            {
                CurrentStackCD = StackCD;
            }
            ReloadStack();
        }

        if (CurrentNumberOfStacks == 0)
        {
            CoolDownValue = StackCD;
            CurrentCD = CurrentStackCD;
        }
    }

    /** ReloadStack, protected virtual void
	 * This method is launched when at leats when stack is missing.
	 * When the CurrentStackCD is reload, we add a stack to the pool.
	 **/
    protected virtual void ReloadStack()
    {
        CurrentStackCD = Mathf.Clamp(CurrentStackCD - Time.deltaTime, 0, StackCD);
        if (CurrentStackCD == 0)
        {
            CurrentNumberOfStacks++;
            if (CurrentNumberOfStacks + 1 == NumberOfStacks)
            {
                CurrentStackCD = StackCD;
            }
        }
    }

    /** IsSpellLauncheable, public override bool 
	 * A Stackable spell is launcheable if it is reloaded and if it has at least one stack.
	 **/
    public override bool IsSpellLauncheable()
    {
        return (CurrentCD == 0 && CurrentNumberOfStacks > 0);
    }

}
