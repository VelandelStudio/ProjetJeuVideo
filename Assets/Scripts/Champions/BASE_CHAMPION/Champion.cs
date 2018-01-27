using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** Champion abstract class.
 * This abstract class is the mother class of all classes in our game. It ensures that the class is well constructed with all of the spells, passive and auto-attack.
 * This script also detects the input keys of the player, and launches the spells.
 * In order to build correctly a class, you have to write the names of all your spells in the ChampionData.json
 * This script ensure that a class can be self constructed with informations you give in the JSON file.
 **/
public abstract class Champion : MonoBehaviour
{
    protected PassiveBase passiveBase;
    protected List<Spell> spells = new List<Spell>();
    protected AutoAttackBase autoAttack;
    protected ChampionData championData;

    protected SpellBarBehaviour spellBar;
    public string Name
    {
        get { return championData.Name; }
        protected set { }
    }

    /// <summary>
    /// The Awake methos is here to construct the class, attributing the spells passive and auto-attack.
	/// First at all, we try to read the ChampionData.json file.After that, we collect every ChampionData declared in the JSON file.
    /// Then, we parse the Array of ChampionData and try to find the one corresponding to the Champion name.
    /// If we find one, we construct the class. If we do not, we Load the DefaultChampion.
    /// </summary>
    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "ChampionData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            ChampionData[] data = JsonHelper.getJsonArray<ChampionData>(jsonFile);
            foreach (ChampionData character in data)
            {
                if (character.Name == GetType().ToString())
                {
                    championData = character;
                    break;
                }
            }

            if (championData == null)
            {
                LoadDeafaultChamp();
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    /** Start protected virtual void Method.
     * From the Awake Method
	 * These elements are constructed in three separated methods.
	 **/
    protected virtual void Start()
    {
        spellBar = GameObject.Find("SpellBar").GetComponent<SpellBarBehaviour>();
        spellBar.SetChildrenActives(true);

        AttributePassiveToClass();
        AttributeAutoAttackToClass();
        AttributeSpellsToClass();
    }

    /** Update protected virtual void Method.
	 * The Update method is used to detect Inputs of the player and then launch the corrects methods.
	 **/
    protected virtual void Update()
    {
        if (CursorBehaviour.CursorIsVisible)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            AutoAttack();
        }

        if (Input.GetKeyDown(InputsProperties.ActiveSpell1))
        {
            LaunchSpell(0);
        }
        if (Input.GetKeyDown(InputsProperties.ActiveSpell2))
        {
            LaunchSpell(1);
        }
        if (Input.GetKeyDown(InputsProperties.ActiveSpell3))
        {
            LaunchSpell(2);
        }
        if (Input.GetKeyDown(InputsProperties.ActiveSpell4))
        {
            LaunchSpell(3);
        }
    }

    public void DestroyChampion()
    {
        GameObject normalPlayer = (GameObject) Resources.Load("Player/Summoner");
        spellBar.SetChildrenActives(false);
        Instantiate(normalPlayer,transform.position, transform.rotation);
        Destroy(gameObject);
    }

    /** AutoAttack protected virtual void Method.
	 * This method call the AutoAttack method present in all AutoAttackSpells.
	 **/
    protected virtual void AutoAttack()
    {
        autoAttack.AutoAttack();
    }

    /** LaunchSpell protected virtual void Method.
	 * @Params : int spellIndex;
	 * This method is called with an int argument, which is the index of the spell in the spells list.
	 * Once the spell is get from the list, we launch the method LaunchSpell inside the spell.
	 * We also launch the Coroutine LaunchGCD() of each other spells.
	 **/
    protected virtual void LaunchSpell(int spellIndex)
    {
        Spell spell = spells[spellIndex];
        if (spell.IsSpellLauncheable())
        {
            spell.LaunchSpell();
            if (spell.HasGCD)
            {
                for (int i = 0; i < spells.Count; i++)
                {
                    if (i != spellIndex && spells[i].HasGCD && spells[i].CurrentCD < spell.SpellGCD)
                    {
                        StartCoroutine(spells[i].LaunchGCD());
                    }
                }
            }
        }
        else
        {
            spell.DisplaySpellNotLauncheable();
        }
    }

    /** AttributePassiveToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the Passive spell name in the championData instance.
	 * Then, it get the script in the scripts library and attach it to the player.
	 * If the script is not found or mispelled, we load a DefaultPassive instead.
	 * After that, we call the AttributeDisplayable method to give to the GUI all information in requires to display informations about the Passive.
	 **/
    protected virtual void AttributePassiveToClass()
    {
        Type t = Type.GetType(championData.Passive);
        if (t == null)
        {
            Debug.Log("The Passive : " + championData.Passive + " can not be loaded. Please cheack the Passive name inside the ChampionData.json file. DefaultPassive substitued.");
            passiveBase = (PassiveBase)gameObject.AddComponent(Type.GetType("DefaultPassive"));
        }
        else
        {
            passiveBase = (PassiveBase)gameObject.AddComponent(t);
        }
        GUIPassiveDisplayer passiveDisplayer = GameObject.Find("Passive").GetComponent<GUIPassiveDisplayer>();
        passiveDisplayer.AttributeDisplayable(passiveBase);
    }

    /** AttributeAutoAttackToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the AutoAttack spell name in the championData instance.
	 * Then, it get the script in the scripts library and attach it to the player.
	 * If the script is not found or mispelled, we load a DefaultAutoAttack instead.
	 * After that, we call the AttributeDisplayable method to give to the GUI all information in requires to display informations about the AutoAttack.
	 **/
    protected virtual void AttributeAutoAttackToClass()
    {
        Type t = Type.GetType(championData.AutoAttack);
        if (t == null)
        {
            Debug.Log("The AutoAttack : " + championData.AutoAttack + " can not be loaded. Please cheack the AutoAttack name inside the ChampionData.json file. DefaultAutoAttack substitued.");
            autoAttack = (AutoAttackBase)gameObject.AddComponent(Type.GetType("DefaultAutoAttack"));
        }
        else
        {
            autoAttack = (AutoAttackBase)gameObject.AddComponent(t);
        }

        GUIAutoAttackDisplayer autoAttackDisplayer = GameObject.Find("AutoAttack").GetComponent<GUIAutoAttackDisplayer>();
        autoAttackDisplayer.AttributeDisplayable(autoAttack);
    }

    /** AttributeSpellsToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the spell names in the championData instance.
	 * Then, it get all of the script in the scripts library and attach it to the player.
	 * If one of the scripts is not found or mispelled, we load a DefaultSpell instead.
	 * After that, we call the AttributeDisplayable method to give to the GUI all information in requires to display informations about each spell.
	 **/
    protected virtual void AttributeSpellsToClass()
    {
        for (int i = 1; i <= 4; i++)
        {
            string SpellName = championData.ActiveSpells[i - 1];
            Spell spellToAdd;

            if (Type.GetType(SpellName) == null)
            {
                Debug.Log("The Spell : " + SpellName + " can not be loaded. Please cheack the Spell names inside the ChampionData.json file. DefaultSpell substitued. ");
                spellToAdd = (Spell)gameObject.AddComponent(Type.GetType("DefaultSpell"));
            }
            else
            {
                spellToAdd = (Spell)gameObject.AddComponent(Type.GetType(SpellName));
            }

            spells.Add(spellToAdd);
            GUISpellDisplayer spellDisplayer = GameObject.Find("Spell" + i).GetComponent<GUISpellDisplayer>();
            spellDisplayer.AttributeDisplayable(spellToAdd);
        }
    }

    /** LoadDeafaultChamp, protected void method
	 * This method should always be launched from the Awake one, when the ChampionData could not be loaded.
	 * As the Awake method, we just parse the Champion.json file in order to substitute a broken to champion to a Default one.
	 * This method is used for dev' so far.
	 **/
    protected void LoadDeafaultChamp()
    {
        Debug.Log("Champion could not be created. Please check the ChampionData.json file. A DefaultChampion is created.");
        string filePath = Path.Combine(Application.streamingAssetsPath, "ChampionData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            ChampionData[] data = JsonHelper.getJsonArray<ChampionData>(jsonFile);
            foreach (ChampionData character in data)
            {
                if (character.Name == "DefaultChampion")
                {
                    championData = character;
                    break;
                }
            }
        }
    }

    /** ReplaceByDefaultDisplayable public void Method
	 * @param : IDisplayable
	 * This method should be called by Displayables (Spells, AutoAttacks and Passives) when they are not able to load their own descriptions.
	 * When launched, this methods checks which script called her and substitute the Displayable element by a Default one. In that way, if something went wrong during the loadind,
	 * You van not have a broken champion. Please note that the method substitute the GUI element also.
	 **/
    public void ReplaceByDefaultDisplayable(IDisplayable brokenDisplayable)
    {
        if (brokenDisplayable is Spell)
        {
            Spell defaultSpell = (Spell)gameObject.AddComponent(Type.GetType("DefaultSpell"));
            int i = spells.IndexOf((Spell)brokenDisplayable);
            spells[i] = defaultSpell;
            GUISpellDisplayer spellDisplayer = GameObject.Find("Spell" + (i + 1)).GetComponent<GUISpellDisplayer>();
            spellDisplayer.AttributeDisplayable(defaultSpell);
        }

        if (brokenDisplayable is PassiveBase)
        {
            passiveBase = (PassiveBase)gameObject.AddComponent(Type.GetType("DefaultPassive"));
            GUIPassiveDisplayer passiveDisplayer = GameObject.Find("Passive").GetComponent<GUIPassiveDisplayer>();
            passiveDisplayer.AttributeDisplayable(passiveBase);
        }

        if (brokenDisplayable is AutoAttackBase)
        {
            autoAttack = (AutoAttackBase)gameObject.AddComponent(Type.GetType("DefaultAutoAttack"));
            GUIAutoAttackDisplayer autoAttackDisplayer = GameObject.Find("AutoAttack").GetComponent<GUIAutoAttackDisplayer>();
            autoAttackDisplayer.AttributeDisplayable(autoAttack);
        }

        Destroy((Behaviour)brokenDisplayable);
    }

    /** ChampionData protected Serializable class.
	 * This class were designed to be at the service of the Champion class.
	 * It is used as a JSON Object to stock every variables read from the JSON file.
	 **/
    [System.Serializable]
    protected class ChampionData
    {
        public string Name;
        public string Passive;
        public string AutoAttack;
        public string[] ActiveSpells;
        public string Element;
        public string[] Description;
        public string[] Tags;
    }
}