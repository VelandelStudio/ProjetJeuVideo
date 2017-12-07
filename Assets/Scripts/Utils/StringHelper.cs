using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Reflection;

/** StringHelper, public static class :
 * This static class contains every methods that are able to help us in string building.
 * You will find here methods to build strings that will be displayed on the screen or converter from seconds to mins.
 **/
public static class StringHelper
{
    /** SpellDescriptionBuilder, public static string method
     * @Params : Spell, object[]
     * This static method is used to construct the Spell Descriptions that will be displayed on the GUI.
     * This method works as a printf. We get the description from the spell and we replace the variables elements by the descriptionVariables.
     * First of all, we get the title, the cooldown value and the cost of resources associated to the spell.
     * Then, we read the description from the JSON file. and pass it to the DescriptionBuilder.
     **/
    public static string SpellDescriptionBuilder(Spell spell)
    {
        string title = spell.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string cooldown = SecToMinConverter(spell.CoolDownValue) + " cooldown";
        cooldown = "<b><size=12><color=lightblue>" + cooldown + "</color></size></b>";

        string description = string.Join("", spell.Description);
        description = DescriptionBuilder(spell, description);

        string finaldescription = title + "\n"
                                + resources + "\n"
                                + cooldown + "\n"
                                + "\n"
                                + description;
        return finaldescription;
    }

    /** AutoAttackDescriptionBuilder, public static string method
     * @Params : AutoAttackBase, object[]
     * This static method is used to construct the AutoAttack Descriptions that will be displayed on the GUI.
     * This method works as a printf. We get the description from the spell and we replace the variables elements by the descriptionVariables.
     * First of all, we get the title and the cost of resources associated to the spell.
     * Then, we read the description from the JSON file. and pass it to the DescriptionBuilder.
     **/
    public static string AutoAttackDescriptionBuilder(AutoAttackBase autoAttack)
    {
        string title = autoAttack.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string description = string.Join("", autoAttack.Description);
        description = DescriptionBuilder(autoAttack, description);


        string finaldescription = title + "\n"
                                + "\n"
                                + description;
        return finaldescription;
    }

    /** PassiveDescriptionBuilder, public static string method
     * @Params : PassiveBase, object[]
     * This static method is used to construct the Passive Descriptions that will be displayed on the GUI.
     * This method works as a printf. We get the description from the spell and we replace the variables elements by the descriptionVariables.
     * First of all, we get the title and the cost of resources associated to the spell.
     * Then, we read the description from the JSON file. and pass it to the DescriptionBuilder.
     **/
    public static string PassiveDescriptionBuilder(PassiveBase passive)
    {
        string title = passive.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string description = string.Join("", passive.Description);
        description = DescriptionBuilder(passive, description);


        string finaldescription = title + "\n"
                                + "\n"
                                + description;
        return finaldescription;
    }

    /** DescriptionBuilder, private static string
	 * @Params : object, string
	 * The method works with reflexions conepts. We get a string from the description inside a {} block.
     * This string correspond to an array or a value contained inside the object we have in parameters.
     * We get the string and the field associated to it inside the object. If the Field is a "Damages" one, we try to catch the "DemagesType" inside the object.
     * After that, we formate and return a string that will be ready the be displayed on the screen
     * We are also able to detect a <<Status>> Pattern. The pattern will be displayed with its description.
	 **/
    public static string DescriptionBuilder(object src, string description)
    {
        string pattern = @"(?<=\{).+?(?=\})";
        MatchCollection matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);

        for (int i = 0; i < matches.Count; i++)
        {
            string color = "";

            if (matches[i].Value.Contains("[") && matches[i].Value.Contains("]"))
            {
                int arrayIndex;
                arrayIndex = GetIndexFromString(matches[i].Value);

                if (matches[i].Value.Contains("Damages"))
                {
                    color = (string)(GetArrayNameFromString(src, "DamagesType").GetValue(arrayIndex)) == "m" ? "cyan" : "maroon";
                }

                description = description.Replace("{" + matches[i].Value + "}", "<b><color=" + color + ">" + GetArrayNameFromString(src, matches[i].Value).GetValue(arrayIndex) + "</color></b>");
            }
            else
            {
                description = description.Replace("{" + matches[i].Value + "}", "<b><color=" + color + ">" + src.GetType().GetField(matches[i].Value).GetValue(src) + "</color></b>");
            }
        }
        pattern = @"(?<=<<).*?(?=>>)";
        matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);
        foreach (Match m in matches)
        {
            Array statusDescription = GetArrayNameFromString(src, m.Value);
            int arrayIndex = GetIndexFromString(m.Value);
            GameObject obj = (GameObject)statusDescription.GetValue(arrayIndex);
            StatusBase status = obj.GetComponent<StatusBase>();

            description = description.Replace("<<" + m.Value + ">>", "<b><color=lime>" + status.Name + "</color></b>");

            string titleStatus = status.Name;
            titleStatus = "<b><size=14><color=darkblue>" + status.Name + "</color></size></b>";

            string debuffType = status is IBuff ? "Empowerment" : "Curse";
            debuffType = "<b><size=12><color=lightblue>" + debuffType + "</color></size></b>";

            string descriptionStatus = DescriptionBuilder(status, string.Join("", status.Description));

            description += "\n___________________________________________________"
                        + "\n"
                        + titleStatus
                        + "\n"
                        + debuffType
                        + "\n"
                        + "\n"
                        + descriptionStatus;
        }
        return description;
    }

    /** GetArrayNameFromString, private static Array
     * @Params : object ,string, object[]
     * This method is used the get an ArrayName from a pattern type of NameOfArray[Index].
     * In this exemple we just return the NameOfArray as an array and not as a string anymore.
     **/
    private static Array GetArrayNameFromString(object src, string str)
    {
        string arrayName = "";

        foreach (char c in str)
        {
            if (c == '[')
            {
                break;
            }
            else
            {
                arrayName += c;
            }
        }

        return (Array)src.GetType().GetField(arrayName).GetValue(src);
    }

    private static int GetIndexFromString(string str)
    {
        string indexPattern = @"(?<=\[).+?(?=\])";
        return int.Parse(Regex.Match(str, indexPattern, RegexOptions.IgnoreCase).Value);
    }

    /** SecToMinConverter, public static string method
     * @Params : float
     * Transform a seconds value into a friendly Format mm.s or ss
     **/
    public static string SecToMinConverter(float seconds)
    {
        if (seconds < 60)
        {
            return seconds + " sec.";
        }
        else
        {
            return string.Format("{0:##.#} min.", (seconds / 60.0));
        }
    }
}