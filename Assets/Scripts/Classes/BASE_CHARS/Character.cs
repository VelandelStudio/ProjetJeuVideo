using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** Character abstract class.
 * This abstract class is the mother class of all classes in our game. It ensures that the class is well constructed with all of the spells, passive and auto-attack.
 * This script also detects the input keys of the player, and launches the spells.
 * In order to build correctly a class, you have to write the names of all your spells in the CharacterData.json
 * This script ensure that a class can be self constructed with informations you give in the JSON file.
 **/
public abstract class Character : MonoBehaviour
{
    protected PassiveBase passiveBase;
    protected List<Spell> spells = new List<Spell>();
    protected AutoAttackBase autoAttack;
    protected CharacterData characterData;

    /** Start protected virtual void Method.
	 * The Start methos is here to construct the class, attributing the spells passive and auto-attack.
	 * First at all, we try to read the CharacterData.json file. After that, we collect every CharacterData declared in the JSON file.
	 * Then, we parse the Array of CharacterData and try to find the one corresponding to the Character name.
	 * If we find one, we construct the class.
	 * These elements are constructed in three separated methods.
	 **/
    protected virtual void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "CharacterData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            CharacterData[] data = JsonHelper.getJsonArray<CharacterData>(jsonFile);
            foreach (CharacterData character in data)
            {
                if (character.Name == this.GetType().ToString())
                {
                    characterData = character;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }

        AttributePassiveToClass();
        AttributeAutoAttackToClass();
        AttributeSpellsToClass();
    }

    /** Update protected virtual void Method.
	 * The Update method is used to detect Inputs of the player and then launch the corrects methods.
	 **/
    protected virtual void Update()
    {
        if(CursorBehaviour.CursorIsVisible)
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
	 * This method is called by the Start method. The Objective of the method is to get the Passive spell name in the characterData instance.
	 * Then, it get the script in the scripts library and attach it to the player.
	 * If the script is not found or mispelled, the HandleException(1) is launched.
	 * After that, we call the AttributePassiveToClass method to give to the GUI all information in requires to display informations about the Passive.
	 **/
    protected virtual void AttributePassiveToClass()
    {
        Type t = Type.GetType(characterData.Passive);
        if (t == null)
        {
            HandleException(1);
            return;
        }
        passiveBase = (PassiveBase) gameObject.AddComponent(t);
        GUIPassiveDisplayer passiveDisplayer = GameObject.Find("Passive").GetComponent<GUIPassiveDisplayer>();
        passiveDisplayer.AttributePassiveToGUI(passiveBase);
    }

    /** AttributeAutoAttackToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the AutoAttack spell name in the characterData instance.
	 * Then, it get the script in the scripts library and attach it to the player.
	 * If the script is not found or mispelled, the HandleException(2) is launched.
	 * After that, we call the AttributeAutoAttackToClass method to give to the GUI all information in requires to display informations about the AutoAttack.
	 **/
    protected virtual void AttributeAutoAttackToClass()
    {
        Type t = Type.GetType(characterData.AutoAttack);
        if (t == null)
        {
            HandleException(2);
            return;
        }
        autoAttack = (AutoAttackBase)gameObject.AddComponent(t);

        GUIAutoAttackDisplayer autoAttackDisplayer = GameObject.Find("AutoAttack").GetComponent<GUIAutoAttackDisplayer>();
        autoAttackDisplayer.AttributeAutoAttackToGUI(autoAttack);
    }

    /** AttributeSpellsToClass protected virtual void Method.
	 * This method is called by the Start method. The Objective of the method is to get the spell names in the characterData instance.
	 * Then, it get all of the script in the scripts library and attach it to the player.
	 * If one of the scripts is not found or mispelled, the HandleException(3) is launched.
	 * After that, we call the AttributeSpellToGUI method to give to the GUI all information in requires to display informations about each spell.
	 **/
    protected virtual void AttributeSpellsToClass()
    {
        for (int i = 1; i <= 4; i++)
        {
            string SpellName = characterData.ActiveSpells[i - 1];
            if (Type.GetType(SpellName) == null)
            {
                HandleException(3);
                return;
            }
            Spell spellToAdd = (Spell)gameObject.AddComponent(Type.GetType(SpellName));
            spells.Add(spellToAdd);

            GUISpellDisplayer spellDisplayer = GameObject.Find("Spell" + i).GetComponent<GUISpellDisplayer>();
            spellDisplayer.AttributeSpellToGUI(spellToAdd);
        }
    }

    /** HandleException private void Method.
	 * @Params : int e;
	 * This method is called when the Character could not be constructed correctly because one component is absent or mispelled.
	 * The int e (for error) field is used for the switc case statement, in order to display the good informations to the developpers.
	 * Please note that if you did a mistake and if this method is launched, the Editor stops the game Simulation. 
	 **/
    private void HandleException(int e)
    {
        switch (e)
        {
            case 1: Debug.LogError("Passive null ou inexistante (faute de frappe ?) PassiveClassName : " + characterData.Passive); break;
            case 2: Debug.LogError("AutoAttack null ou inexistante (faute de frappe ?) AutoAttackClassName : " + characterData.AutoAttack); break;
            case 3: Debug.LogError("Liste de sorts null ou Sort inexistant (faute de frappe ?)"); break;

            default: break;
        }
        UnityEditor.EditorApplication.isPlaying = false;
    }

    /** CharacterData protected Serializable class.
	 * This class were designed to be at the service of the Character class.
	 * It is used as a JSON Object to stock every variables read from the JSON file.
	 **/
    [System.Serializable]
    protected class CharacterData
    {
        public string Name;
        public string Passive;
        public string AutoAttack;
        public string[] ActiveSpells;
    }
}