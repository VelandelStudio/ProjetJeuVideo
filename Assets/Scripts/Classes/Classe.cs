using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Classe : MonoBehaviour
{
    public string PassiveClassName;
    public string AutoAttackClassName;
    public List<string> spellClassNames = new List<string>();
    protected List<Spell> spells = new List<Spell>();
    protected AutoAttackBase autoAttack;

    protected virtual void Start()
    {
        AttributeSpellsToClass();
        AttributeAutoAttackToClass();
        AttributePassiveToClass();
    }

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

    protected virtual void AutoAttack()
    {
        autoAttack.AutoAttack();
    }

    protected virtual void LaunchSpell(int spellIndex)
    {
        Spell spell = spells[spellIndex];
        spell.LaunchSpell();
    }

    protected void AttributeSpellsToClass()
    {
        Type t;
        foreach (string SpellName in spellClassNames)
        {
            t = Type.GetType(SpellName.ToString());
            if (t == null)
            {
                HandleException(1);
                return;
            }
            spells.Add((Spell)gameObject.AddComponent(t));
        }
    }

    protected void AttributeAutoAttackToClass()
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

    protected void AttributePassiveToClass()
    {
        Type t = Type.GetType(PassiveClassName.ToString());
        if (t == null)
        {
            HandleException(3);
            return;
        }
        gameObject.AddComponent(t);
    }

    private void HandleException(int e)
    {
        switch (e)
        {
            case 1: Debug.LogError("Liste de sorts null ou Sort inexistant (faute de frappe ?)"); break;
            case 2: Debug.LogError("AutoAttack null ou inexistante (faute de frappe ?) AutoAttackClassName : " + AutoAttackClassName); break;
            case 3: Debug.LogError("Passive null ou inexistante (faute de frappe ?) PassiveClassName : " + PassiveClassName); break;

            default: break;
        }
        UnityEditor.EditorApplication.isPlaying = false;
    }
}