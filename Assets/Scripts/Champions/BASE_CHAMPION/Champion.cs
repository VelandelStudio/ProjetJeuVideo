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
    public bool ChampionDestroyable = true;
    protected PassiveBase passiveBase;
    protected List<Spell> spells = new List<Spell>();
    protected AutoAttackBase autoAttack;
    protected ChampionData championData;

    protected Animator anim;
    protected SpellBarBehaviour spellBar;
    protected float timeSinceLastChampionSwap;

    public string Name
    {
        get { return championData.Name; }
        protected set { }
    }

    /** Awake, privat void Method,
	 * Instanciate a new ChampionData. 
	 **/
    private void Awake()
    {
        championData = new ChampionData(GetType().ToString());
    }

    /** Start, protected virtual void Method.
     * In the Start Method, we build the champion by attrubuting to hit the correct AutoAttack, Passive and Spells.
	 **/
    protected virtual void Start()
    {
        spellBar = GameObject.Find("SpellBar").GetComponent<SpellBarBehaviour>();
        spellBar.SetChildrenActives(true);

        AttributePassiveToClass();
        AttributeAutoAttackToClass();
        AttributeSpellsToClass();

        anim = GetComponent<Animator>();
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
            anim.SetTrigger("Spell2");
            LaunchSpell(1);
        }
        if (Input.GetKeyDown(InputsProperties.ActiveSpell3))
        {
            anim.SetTrigger("Spell3");
            LaunchSpell(2);
        }
        if (Input.GetKeyDown(InputsProperties.ActiveSpell4))
        {
            LaunchSpell(3);
        }
    }

    /** DestroyChampion, public void
     * This Method is used to Destroy the champion and set the Player back to its summoner state
     **/
    public void DestroyChampion()
    {
        GameObject normalPlayer = (GameObject)Resources.Load("Player/Summoner");
        spellBar.SetChildrenActives(false);
        Camera.main.transform.parent = null;
        Instantiate(normalPlayer, transform.position, transform.rotation);
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
            anim.SetTrigger("Spell" + (spellIndex + 1));

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
	 * Please note that we always check if the passive we are trying to ttach already exists. If that's the case, we just enable the existant script instead of adding one.
	 * This features is usefull for champion that has the ability to switch between different forms with differents passives.
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
            bool passiveFound = false;
            PassiveBase[] passives = GetComponents<PassiveBase>();
            if (passives.Length > 0)
            {
                for (int i = 0; i < passives.Length; i++)
                {
                    if (passives[i].GetType() == t)
                    {
                        passives[i].enabled = true;
                        passiveBase = passives[i];
                        passiveFound = true;
                    }
                    else
                    {
                        passives[i].enabled = false;
                    }
                }
            }

            if (!passiveFound)
            {
                passiveBase = (PassiveBase)gameObject.AddComponent(t);
            }
        }
        GUIPassiveDisplayer passiveDisplayer = GameObject.Find("Passive").GetComponent<GUIPassiveDisplayer>();
        passiveDisplayer.AttributeDisplayable(passiveBase);
    }

    /** AttributeAutoAttackToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the AutoAttack spell name in the championData instance.
	 * Then, it get the script in the scripts library and attach it to the player.
	 * If the script is not found or mispelled, we load a DefaultAutoAttack instead.
	 * After that, we call the AttributeDisplayable method to give to the GUI all information in requires to display informations about the AutoAttack.
	 * Please note that we always check if the autoAttack we are trying to ttach already exists. If that's the case, we just enable the existant script instead of adding one.
	 * This features is usefull for champion that has the ability to switch between different forms with differents autoAttacks.
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
            bool autoAttackFound = false;
            AutoAttackBase[] autoAttacks = GetComponents<AutoAttackBase>();
            if (autoAttacks.Length > 0)
            {
                for (int i = 0; i < autoAttacks.Length; i++)
                {
                    if (autoAttacks[i].GetType() == t)
                    {
                        autoAttacks[i].enabled = true;
                        autoAttack = autoAttacks[i];
                        autoAttack.ReduceCurrentCooldown(Time.fixedTime - timeSinceLastChampionSwap);

                        autoAttackFound = true;
                    }
                    else
                    {
                        autoAttacks[i].enabled = false;
                    }
                }
            }

            if (!autoAttackFound)
            {
                autoAttack = (AutoAttackBase)gameObject.AddComponent(t);
            }
        }

        GUIAutoAttackDisplayer autoAttackDisplayer = GameObject.Find("AutoAttack").GetComponent<GUIAutoAttackDisplayer>();
        autoAttackDisplayer.AttributeDisplayable(autoAttack);
    }

    /** AttributeSpellsToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the spell names in the championData instance.
	 * Then, it get all of the script in the scripts library and attach it to the player.
	 * If one of the scripts is not found or mispelled, we load a DefaultSpell instead.
	 * After that, we call the AttributeDisplayable method to give to the GUI all information in requires to display informations about each spell.
	 * Please note that we always check if the spell we are trying to ttach already exists. If that's the case, we just enable the existant script instead of adding one.
	 * This features is usefull for champion that has the ability to switch between different forms with differents spells.
	 **/
    protected virtual void AttributeSpellsToClass()
    {
        spells = new List<Spell>();
        Spell[] spellTab = GetComponents<Spell>();

        for (int i = 1; i <= 4; i++)
        {
            string SpellName = championData.ActiveSpells[i - 1];
            Spell spellToAdd = null;

            if (Type.GetType(SpellName) == null)
            {
                Debug.Log("The Spell : " + SpellName + " can not be loaded. Please cheack the Spell names inside the ChampionData.json file. DefaultSpell substitued. ");
                spellToAdd = (Spell)gameObject.AddComponent(Type.GetType("DefaultSpell"));
            }
            else
            {
                bool spellFound = false;
                if (spellTab.Length > 0)
                {
                    for (int j = 0; j < spellTab.Length; j++)
                    {
                        if (spellTab[j].GetType() == Type.GetType(SpellName))
                        {
                            spellTab[j].enabled = true;
                            spellToAdd = spellTab[j];
                            spellToAdd.ReduceCurrentCooldown(Time.fixedTime - timeSinceLastChampionSwap);
                            spellFound = true;
                        }
                        else
                        {
                            spellTab[j].enabled = spells.Contains(spellTab[j]);
                        }
                    }
                }

                if (!spellFound)
                {
                    spellToAdd = (Spell)gameObject.AddComponent(Type.GetType(SpellName));
                }
            }

            spells.Add(spellToAdd);
            GUISpellDisplayer spellDisplayer = GameObject.Find("Spell" + i).GetComponent<GUISpellDisplayer>();
            spellDisplayer.AttributeDisplayable(spellToAdd);
        }
    }

    /** SwapChampion, oublic virtual void
	 * @param : string
	 * Use this method carefully ! 
	 * You shoudl call this method with champion that has the ability to swap between many forms such as the Summoner.
	 * This method will not destroy the current champion, it will add spells from another champion to the current one.
	 * Spells that was previously on the champion will be disabled but not destroyed.
	 **/
    public virtual void SwapChampion(string champion)
    {
        championData = new ChampionData(champion);
        AttributePassiveToClass();
        AttributeAutoAttackToClass();
        AttributeSpellsToClass();
        timeSinceLastChampionSwap = Time.fixedTime;
    }
}