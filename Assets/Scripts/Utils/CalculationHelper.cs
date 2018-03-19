using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static Class that contains all the calcul in the game.
/// Depending on Characteristics for instance.
/// </summary>
public static class CalculationHelper {

    /// <summary>
    /// The ApplyDamages Method, the calcul to return damages dealt.
    /// Use all the characs of the attacking and defending Entity in the game.
    /// </summary>
    /// <param name="attackEntity">EntityLivingBase of attacking Character</param>
    /// <param name="defenseEntity">EntityLivingBase of defending Character</param>
    /// <param name="baseDamage">Json damage of the spell, auto ...</param>
    /// <returns>The Real damage dealt to an unit in the game</returns>
    public static int ApplyDamages(EntityLivingBase attackEntity, EntityLivingBase defenseEntity, int baseDamage)
    {
        int power = (int)attackEntity.Characteristics.Power;
        int def = (int)defenseEntity.Characteristics.Defense;
        float critStrike = attackEntity.Characteristics.CritChance;
        float attfactor = attackEntity.Characteristics.DamageFactor;
        float defFactor = defenseEntity.Characteristics.DefenseFactor;
        float damage = baseDamage;

        attfactor += power / 100;
        damage *= attfactor;

        if (Random.Range(1, 101) <= critStrike)
        {
            damage *= 1.5f;
        }

        def *= (int)defFactor;
        damage -= def;

        Debug.Log("Real Damages = " + damage);
        return (int)damage;
    }
}
