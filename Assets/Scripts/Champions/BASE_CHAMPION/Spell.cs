using UnityEngine;
using System.Collections;

/** Spell abstract class.
 * This abstract class is the mother class of all spells in our game. 
 * This class handles the behaviour the CD of all spells. It also contains the LaunchSpell method launched by the Champion.
 **/
public abstract class Spell : MonoBehaviour, ISpellDisplayable
{
    /** Fields of Spell
     * The Spell class contains a lot of differents fields.
     * Here, you can find fin every component of the SpellData that can be found in the JSON file associated to the spell.
     * All public fields that are set from the _spellData in the Awake method should be used by other scripts.
     * The spellInUse field is use to tell when a spell is starting and when it is ending. For example, a Charge spell has a real duration in time.
     * The CurrentCD field is used to know how much time ypu have to wait until the next use of the spell.
     * The SpellGCD field is used to set a GlobalCooldown to all Spells. Is a Spell is under GCD, the field IsUnderGCD is true.
     * The IsSpellUsable field is used to determine if you can use a spell or not (stun or special condition on a spell etc ...)
     **/
    #region Fields
    public SpellData _spellData { get; protected set; }
    public string Name { get { return _spellData.Name; } protected set { } }
    public string Element { get { return _spellData.Element; } protected set { } }
    public string Type { get { return _spellData.Type; } protected set { } }
    public float CoolDownValue { get { return _spellData.CoolDownValue; } protected set { } }
    public int[] Damages { get { return _spellData.Damages; } protected set { } }
    public string[] DamagesType { get { return _spellData.DamagesType; } protected set { } }
    public string[] OtherValues { get { return _spellData.OtherValues; } protected set { } }
    public GameObject[] Status { get { return _spellData.Status; } protected set { } }
    public string[] Description { get { return _spellData.Description; } protected set { } }
    public int NumberOfStacks { get { return _spellData.NumberOfStacks; } protected set { } }
    public bool IsLoaded { get { return _spellData.IsLoaded; } protected set { } }

    public bool HasGCD;

    protected bool spellInUse = false;
    protected Champion champion;

    public float CurrentCD { get; protected set; }

    protected float spellGCD = 1f;

    public float SpellGCD
    {
        get { return spellGCD; }
        protected set { }
    }

    public bool IsUnderGCD { get; protected set; }

    private bool _IsSpellUsable =true;

    public bool IsSpellUsable
    {
        get { return _IsSpellUsable; }
        set { _IsSpellUsable = value; }
    }

    #endregion

    #region Functionnal Methods

    /** Awake protected virtual void Method,
	 * The Awake method is used to create an instance of a SpellData which is responsible of the JSON Loading.
	 **/
    protected virtual void Awake()
    {
        champion = GetComponentInParent<Champion>();
        _spellData = new SpellData(this.GetType().ToString());
    }

    /** Start protected virtual void Method,
	 * We initialize the CD of the spell.
	 **/
    protected virtual void Start()
    {
        CurrentCD = 0;
    }

    /** Update protected virtual void Method,
	 * If the spell is not Launcheable (i.e. if it has already been launched), then the current CD is reseting, time after time.
	 **/
    protected virtual void Update()
    {
        if (!IsSpellLauncheable())
        {
            CurrentCD = Mathf.Clamp(CurrentCD - Time.deltaTime, 0, CoolDownValue);
        }
    }

    /** LaunchSpell public virtual void Method,
	 * This public Method should always be called by the Classe script. It first checks if the spell is Launcheable.
	 * If it is, it switched the boolean spellInUse to true. This Method should always be used with OnSpellLaunched, to reset the spellInUse to false.
	 **/
    public virtual void LaunchSpell()
    {
        spellInUse = true;
    }

    /** LoadResource, protected virtual GameObject Method
	 * @param : string,
	 * @return : GameObject
	 * This method is used to load a GameObject prefab inside the champion folder.
	 **/
    protected virtual GameObject LoadResource(string prefabName)
    {
        return (GameObject)Resources.Load(champion.Name + "/" + prefabName);
    }

    /** OnSpellLaunched protected virtual void Method,
	 * This method should be called when the spell has reached is final statement.
	 * When it is the case, this method set the current CD to 0 and tells the game that this spells is not in use anymore.
	 **/
    protected virtual void OnSpellLaunched()
    {
        CurrentCD = CoolDownValue;
        spellInUse = false;
    }

    /** IsSpellLauncheable protected virtual bool Method,
	 * This returns if the spell is launcheable or not. In this script, we only check if the spell is under Cooldown or not.
	 * You should overrie this method is you want to not make your spell launcheable in other conditions
	 **/
    public virtual bool IsSpellLauncheable()
    {
        return (CurrentCD == 0) && IsSpellUsable;
    }

    /** IsSpellInUse public bool Method,
	 * This returns if the spell is currently in use.
	 **/
    public bool IsSpellInUse()
    {
        return spellInUse;
    }

    /** DisplaySpellNotLauncheable protected void Method,
	 * This displays in the logs a message when the spell is not Launchable whereas the player is trying to launche it.
	 * In the future, this method will return a string on the screen of the player.
	 **/
    public void DisplaySpellNotLauncheable()
    {
        Debug.Log(this.GetType().ToString() + " is not available for the moment.");
    }

    /** AvailableForGUI public virtual bool Method,
	 * Each spell is associated to a GUI Spell Slot. 
	 * Globally this Slot has an image that represents the remaining CD timer of the Spell. 
	 * If the cooldown is reload, the image is clear. But, sometimes, we have spells that can not be launched in some conditions.
	 * This is what this method does. By Default, every spell is AvailableForGUI, meaning that the image associated is only CD dependant.
	 **/
    public virtual bool AvailableForGUI()
    {
        return true;
    }

    /**GetDescriptionGUI, public string Method
	 * Return the description of our spell built by the DescriptionBuilder of the StringHelper static class.
	 * This method allows to get a dynamic and colored description on the screen.
	 **/
    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
    }

    /** ApplyStatus, protected virtual GameObject
     * @Params : GameObject, Transform
     * @Returns: GameObject
     * This method should be called by spells that are able to apply a Status on their targets.
     * The first param (GameObject status) should be a GameObject that has a StatusBase Script attached. 
     * Most of the time, this gameObject is contained in the _spellData.Status Table, and the Transform is the target one.
     * The method will instantiate a new Status and attach to it the status that is already attached on the first parameter.
     * When we instantiate an object, the StatusBase element if reseted, so we need to attach this instance of the StatusBase because of previous modifications,
     * such as damages or CDReduction of the Status of the player. 
     * Then, we return the fresh GameObject constructed if we want to use it later.
     **/
    protected virtual GameObject ApplyStatus(GameObject status, Transform tr)
    {
        GameObject objInst = Instantiate(status, tr);
        StatusBase statusInst = objInst.GetComponent<StatusBase>();
        statusInst.StartStatus(status.GetComponent<StatusBase>());
        return objInst;
    }
    #endregion

    /** ReduceCurrentCooldown, public void Method
	 * @param : float
	 * Reduce the current CD with a float. Usefull for cooldown reduction items
	 **/
    public void ReduceCurrentCooldown(float f)
    {
        CurrentCD = Mathf.Clamp(CurrentCD - f, 0, CoolDownValue);
    }

    #region Ienumerators and Coroutines
    /** LaunchGCD, public virtual IEnumerator Method
	 * This Method should be launched by other scripts in order to activate the Global Cooldown of the spell.
	 * If a spell is stackable and if it has more than 1 stacks, this method can be launched by the spell itself.
	 **/
    public virtual IEnumerator LaunchGCD()
    {
        CurrentCD = SpellGCD;
        IsUnderGCD = true;
        spellInUse = true;

        yield return new WaitForSeconds(SpellGCD);

        spellInUse = false;
        IsUnderGCD = false;
    }
    #endregion
}