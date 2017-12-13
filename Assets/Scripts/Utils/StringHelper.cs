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

    public static string DescriptionBuilder(IDisplayer displayer)
    {
        string title = displayer.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string cooldown = "";
        if (displayer.CoolDownValue != 0)
        {
            cooldown = SecToMinConverter(displayer.CoolDownValue) + " cooldown";
            cooldown = cooldown == "0" ? "" : cooldown;
            cooldown = "<b><size=12><color=lightblue>" + cooldown + "</color></size></b>";
        }

        string description = string.Join("", displayer.Description);
        description = DescriptionBuilder(displayer, description);

        string finaldescription = title + "\n"
                                + resources + "\n"
                                + cooldown + "\n"
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
                description = description.Replace("{" + matches[i].Value + "}", "<b><color=" + color + ">" + src.GetType().GetProperty(matches[i].Value).GetValue(src, null) + "</color></b>");
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
        return (Array)src.GetType().GetProperty(arrayName).GetValue(src, null);
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