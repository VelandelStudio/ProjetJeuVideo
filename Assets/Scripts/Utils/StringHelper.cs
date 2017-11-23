using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringHelper
{
    /** SpellDescriptionBuilder, public static string method
     * @Params : Spell, object[]
     * This static method is used to construct the Spell Descriptions that will be displayed on the GUI.
     * This method works as a printf. We get the description from the spell and we replace the variables elements by the descriptionVariables.
     * First of all, we get the title, the cooldown value and the cost of resources associated to the spell.
     * Then, we read the description from the JSON file. and pass it to the DescriptionBuilder.
     **/
    public static string SpellDescriptionBuilder(Spell spell, object[] descriptionVariables)
    {
        string title = spell.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string cooldown = SecToMinConverter(spell.CoolDownValue) + " cooldown";
        cooldown = "<b><size=12><color=lightblue>" + cooldown + "</color></size></b>";

        string description = string.Join("", spell.Description);
        description = DescriptionBuilder(description, descriptionVariables);

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
    public static string AutoAttackDescriptionBuilder(AutoAttackBase autoAttack, object[] descriptionVariables)
    {
        string title = autoAttack.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string description = string.Join("", autoAttack.Description);
        description = DescriptionBuilder(description, descriptionVariables);


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
    public static string PassiveDescriptionBuilder(PassiveBase passive, object[] descriptionVariables)
    {
        string title = passive.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string description = string.Join("", passive.Description);
        description = DescriptionBuilder(description, descriptionVariables);


        string finaldescription = title + "\n"
                                + "\n"
                                + description;
        return finaldescription;
    }

    /** DescriptionBuilder, private static string
	 * @Params : string, object[]
	 * The method is able to detect different patterns :
     * {p0} : The first variable of the array is a Physical damage (Bold + Maroon color).
     * {m1} : The second variable of the array is a Magical damage (Bold + Cyan color).
     * {2}  : The third variable of the array is an anonymous variable (Just Bold, Doritos like).
     * <<IgniteStatus>> : Additionnal effects are surronded by <<>> (Bold + Green color + Get the Description of the variable).
	 * And returns a WONDERFULL string with colors, flowers and unicorns.
	 **/
    private static string DescriptionBuilder(string description, object[] descriptionVariables)
    {
        string pattern = @"(?<=\{).+?(?=\})";
        MatchCollection matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);

        if (descriptionVariables != null)
        {
            for (int i = 0; i < descriptionVariables.Length; i++)
            {
                if (matches[i].Value[0] == 'm' || matches[i].Value[0] == 'p')
                {
                    string color = matches[i].Value[0] == 'm' ? "cyan" : "maroon";
                    description = description.Replace("{" + matches[i].Value + "}", "<b><color=" + color + ">{" + matches[i].Value.TrimStart('m', 'p') + "}</color></b>");
                    continue;
                }
                else
                {
                    description = description.Replace("{" + matches[i].Value + "}", "<b>{" + matches[i].Value + "}</b>");
                }
            }

            description = string.Format(description, descriptionVariables);
        }

        pattern = @"(?<=<<).*?(?=>>)";
        matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);
        foreach (Match m in matches)
        {
            description = description.Replace("<<" + m.Value + ">>", "<b><color=lime>" + m.Value + "</color></b>");
            description += "\n___________________________________________________\n";
            description += "StringHelper TODO : Dynamic Description of Status";
        }

        return description;
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