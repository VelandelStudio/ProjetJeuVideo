using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackableSpell : Spell
{

    public float NumberOfStacks;
    public float StackCD;
    public float CurrentStackCD;

    protected override void Start()
    {
        base.Start();
        NumberOfStacks = SpellDefinition.NumberOfStack;
        StackCD = SpellCD;
        Debug.Log(StackCD);
    }

    /** LaunchSpell public virtual void Method,
	 * This public Method should always be called by the Classe script. It first checks if the spell is Launcheable.
	 * If it is, it switched the boolean spellInUse to true. This Method should always be used with OnSpellLaunched, to reset the spellInUse to false.
	 **/
    public override void LaunchSpell()
    {
        if (!IsSpellLauncheable())
        {
            DisplaySpellNotLauncheable(this);
        }
        else
        {
            spellInUse = true;
        }
    }
    /** OnSpellLaunched protected virtual void Method,
	 * This method should be called when the spell has reached is final statement.
	 * When it is the case, this method set the current CD to 0 and tells the game that this spells is not in use anymore.
	 **/
    protected override void OnSpellLaunched()
    {
        SpellCD = spellGCD;
        CurrentCD = spellGCD;
        spellInUse = false;
        NumberOfStacks--;
    }

    protected override void Update()
    {
        base.Update();
        if (NumberOfStacks < SpellDefinition.NumberOfStack)
        {
            if (CurrentStackCD == 0)
            {
                CurrentStackCD = StackCD;
            }
            ReloadStack();
        }

        if (NumberOfStacks == 0 && CurrentCD == 0)
        {
            SpellCD = StackCD;
            CurrentCD = CurrentStackCD;
        }
    }

    protected virtual void ReloadStack()
    {
        CurrentStackCD = Mathf.Clamp(CurrentStackCD - Time.deltaTime, 0, StackCD);
        if (CurrentStackCD == 0)
        {
            NumberOfStacks++;
            if (NumberOfStacks + 1 == SpellDefinition.NumberOfStack)
            {
                CurrentStackCD = StackCD;
            }
        }
    }

    public virtual bool IsSpellLauncheable()
    {
        return (CurrentCD == 0 && NumberOfStacks > 0);
    }

}
