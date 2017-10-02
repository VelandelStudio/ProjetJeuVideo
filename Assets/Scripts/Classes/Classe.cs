using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/** Classe abstract class.
 * This abstract class is the mother class of all classes in our game. It ensures that the class is well constructed with all of the spells, passive and auto-attack.
 * This script also detects the input keys of the player, and launches the spells.
 * In order to build correctly a class, you have to write the names of all your spells in the editor in the correct fields.
 * Please note for the future :
 * This way to build a classe is not really easy to use at the moment, but the main aim his to make this script evolve in a way to use JSON Libraries.
 * In this way, we will have to write all elements we need for each classes in one single JSON File, then the script will be able to read this file and construct the classe itself.
 **/
public abstract class Classe : MonoBehaviour
{
    public string PassiveClassName;
    public string AutoAttackClassName;
    public List<string> spellClassNames = new List<string>();
    protected List<Spell> spells = new List<Spell>();
    protected AutoAttackBase autoAttack;

    /** Start protected virtual void Method.
	 * The Start methos is here to construct the class, attributing the spells passive and auto-attack.
	 * These elements are constructed in three separated methods.
	 **/
    protected virtual void Start()
    {
        AttributePassiveToClass();
        AttributeAutoAttackToClass();
        AttributeSpellsToClass();
    }

    /** Update protected virtual void Method.
	 * The Update method is used to detect Inputs of the player and then launch the corrects methods.
	 * First, it ensures than no spells are currently in use. In this way, we are sure that we can't fire 2 spells at the same time.
	 * Then wa can launch one of the four spells or an auto-attack.
	 **/
    protected virtual void Update()
    {
        foreach (Spell s in spells)
        {
            if (s.IsSpellInUse())
                return;
        }

        if (Input.GetMouseButton(0))
            AutoAttack();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            LaunchSpell(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            LaunchSpell(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            LaunchSpell(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            LaunchSpell(3);
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
	 **/
    protected virtual void LaunchSpell(int spellIndex)
    {
        Spell spell = spells[spellIndex];
        spell.LaunchSpell();
    }

    /** AttributePassiveToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the Passive spell name in the PassiveClassName field.
	 * Then, it get the script in the scripts library and attach it to the player.
	 * If the script is not found or mispelled, the HandleException(1) is launched.
	 **/
    protected virtual void AttributePassiveToClass()
    {
        Type t = Type.GetType(PassiveClassName.ToString());
        if (t == null)
        {
            HandleException(1);
            return;
        }
        gameObject.AddComponent(t);
    }

    /** AttributeAutoAttackToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the AutoAttack spell name in the AutoAttackClassName field.
	 * Then, it get the script in the scripts library and attach it to the player.
	 * If the script is not found or mispelled, the HandleException(2) is launched.
	 **/
    protected virtual void AttributeAutoAttackToClass()
    {
        Debug.Log(AutoAttackClassName.ToString());
        Type t = Type.GetType(AutoAttackClassName.ToString());
        if (t == null)
        {
            HandleException(2);
            return;
        }
        autoAttack = (AutoAttackBase)gameObject.AddComponent(t);
    }

    /** AttributeSpellsToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the spell names in the spellClassNames List.
	 * Then, it get all of the script in the scripts library and attach it to the player.
	 * If one of the scripts is not found or mispelled, the HandleException(3) is launched.
	 **/
    protected virtual void AttributeSpellsToClass()
    {
        Type t;
        foreach (string SpellName in spellClassNames)
        {
            t = Type.GetType(SpellName.ToString());
            if (t == null)
            {
                HandleException(3);
                return;
            }
            spells.Add((Spell)gameObject.AddComponent(t));
        }
    }

    /** HandleException private void Method.
	 * @Params : int e;
	 * This method is called when the Classe could not be constructed correctly because one component is absent or mispelled.
	 * The int e (for error) field is used for the switc case statement, in order to display the good informations to the developpers.
	 * Please note that if you did a mistake and if this method is launched, the Editor stops the game Simulation. 
	 **/
    private void HandleException(int e)
    {
        switch (e)
        {
            case 1: Debug.LogError("Passive null ou inexistante (faute de frappe ?) PassiveClassName : " + PassiveClassName); break;
            case 2: Debug.LogError("AutoAttack null ou inexistante (faute de frappe ?) AutoAttackClassName : " + AutoAttackClassName); break;
            case 3: Debug.LogError("Liste de sorts null ou Sort inexistant (faute de frappe ?)"); break;

            default: break;
        }
        UnityEditor.EditorApplication.isPlaying = false;
    }
}