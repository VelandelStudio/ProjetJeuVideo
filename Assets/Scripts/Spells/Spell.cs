using UnityEngine;

/** Spell abstract class.
 * This abstract class is the mother class of all spells in our game. 
 * This class handles the behaviour the CD of all spells. It also contains the LaunchSpell method launched by the Classe.
 **/
public abstract class Spell : MonoBehaviour
{
    protected float SpellCD;
    protected float CurrentCD;
    protected bool SpellInUse = false;

    /** Start protected virtual void Method,
	 * The Start method first display the name of the spell whe he is created by the Classe.
	 * Then, it initialize the CD of the spell.
	 **/
    protected virtual void Start()
    {
        DisplaySpellCreation(this);
        CurrentCD = SpellCD;
    }

    /** Update protected virtual void Method,
	 * If the spell is not Launcheable (i.e. if it has already been launched), then the current CD is reseting, time after time.
	 **/
    protected virtual void Update()
    {
        if (!IsSpellLauncheable())
            CurrentCD = Mathf.Clamp(CurrentCD + Time.deltaTime, 0, SpellCD);
    }

    /** LaunchSpell public virtual void Method,
	 * This public Method should always be called by the Classe script. It first checks if the spell is Launcheable.
	 * If it is, it switched the boolean SpellInUse to true. This Method should always be used with OnSpellLaunched, to reset the SpellInUse to false.
	 **/
    public virtual void LaunchSpell()
    {
        if (!IsSpellLauncheable())
            DisplaySpellNotLauncheable(this);
        else
            SpellInUse = true;
    }

    /** OnSpellLaunched protected virtual void Method,
	 * This method should be called when the spell has reached is final statement.
	 * When it is the case, this method set the current CD to 0 and tells the game that this spells is not in use anymore.
	 **/
    protected virtual void OnSpellLaunched()
    {
        CurrentCD = 0;
        SpellInUse = false;
    }

    /** IsSpellLauncheable protected virtual bool Method,
	 * This returns if the spell is launcheable or not. In this script, we only check if the spell is under Cooldown or not.
	 * You should overrie this method is you want to not make your spell launcheable in other conditions
	 **/
    protected virtual bool IsSpellLauncheable()
    {
        return (SpellCD == CurrentCD);
    }

    /** IsSpellInUse public bool Method,
	 * This returns if the spell is currently in use.
	 **/
    public bool IsSpellInUse()
    {
        return SpellInUse;
    }

    /** DisplaySpellNotLauncheable protected void Method,
	 * This displays in the logs a message when the spell is not Launchable whereas the player is trying to launche it.
	 * In the future, this method will return a string on the screen of the player.
	 **/
    protected void DisplaySpellNotLauncheable(Spell spell)
    {
        Debug.Log(spell.GetType().ToString() + " is not available for the moment.");
    }

    /** DisplaySpellCreation protected void Method,
	 * This displays in the logs a message when the spell is created at the start of a Classe.
	 * This method is a tool for in-dev feature so far.
	 **/
    protected void DisplaySpellCreation(Spell spell)
    {
        Debug.Log(spell.GetType().ToString() + " created.");
    }
}
