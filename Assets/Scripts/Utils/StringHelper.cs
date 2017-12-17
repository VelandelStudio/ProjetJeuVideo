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
    /** DescriptionBuilder, public static string
	 * @param IDisplayable
	 * This method is used to build the description field of a displayable, by setting the title, the cooldown etc... on the screen.
	 **/
    public static string DescriptionBuilder(IDisplayable displayable)
    {
        string title = "";
        string resources = "";
        string cooldown = "";
        string description = "";
        string statusType = "";

        title = FormateTitle(displayable.Name);

        if (displayable is IStatusDisplayable)
        {
            statusType = FormateStatusType(displayable);
        }

        /*string resources = FormateResources(displayable.ResourceValue, displayable.ResourceType); */
        if (displayable.CoolDownValue != 0)
        {
            cooldown = FormateCoolDown(SecToMinConverter(displayable.CoolDownValue));
        }

        description = FormateDescription(displayable, displayable.Description);

        string finaldescription = title
                                + statusType
                                + resources
                                + cooldown
                                + "\n"
                                + description;
        return finaldescription;
    }

    /** FormateDescription, public static string
	 * @param IDisplayable, string[]
	 * This method is used to formate the Description field (not the title, resources etc...) 
	 * First we get the Description and Join it to make it a simple string. 
	 * Then we launch the HandleVariablesDetection and the HandleStatusDetection to attach dynamic references of elements set in the description.
	 **/
    private static string FormateDescription(IDisplayable displayable, string[] descriptionTable)
    {
        string description = string.Join("", descriptionTable);
        description = HandleVariablesDetection(displayable, description);
        description = HandleStatusDetection(displayable, description);
        return description;
    }

    /** HandleVariablesDetection, public static string
	 * @param IDisplayable, string
	 * This method is used to detect different variables, set into the pattern {Variable} in the description.
	 * It can also handles the variables that are arrays. For each elements that conrresponds to the pattern, we try to get the variable in the Class displayable. 
	 * Then we display its value.
	 **/
    private static string HandleVariablesDetection(IDisplayable displayable, string description)
    {
        string pattern = @"(?<=\{).+?(?=\})";
        MatchCollection matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);

        for (int i = 0; i < matches.Count; i++)
        {
            string color = "";
            if (matches[i].Value.Contains("[") && matches[i].Value.Contains("]"))
            {
                int arrayIndex = GetIndexFromString(matches[i].Value);

                if (matches[i].Value.Contains("Damages"))
                {
                    color = displayable.DamagesType[arrayIndex] == "m" ? "cyan" : "maroon";
                }

                description = description.Replace("{" + matches[i].Value + "}", "<b><color=" + color + ">" + GetArrayNameFromString(displayable, matches[i].Value).GetValue(arrayIndex) + "</color></b>");
            }
            else
            {
                description = description.Replace("{" + matches[i].Value + "}", "<b><color=" + color + ">" + displayable.GetType().GetProperty(matches[i].Value).GetValue(displayable, null) + "</color></b>");
            }
        }

        return description;
    }

    /** HandleStatusDetection, public static string
	 * @param IDisplayable, string
	 * This method is used to detect different StatusBase, set into the pattern <<Status>> in the description.
	 * For each elements that conrresponds to the pattern, we try to get the variable in the Class displayable. 
	 * Then, we get the description of the status Displayable and call the DescriptionBuilder (Recursivity power <3 ).
	 **/
    private static string HandleStatusDetection(IDisplayable displayable, string description)
    {
        string pattern = @"(?<=<<).*?(?=>>)";
        MatchCollection matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);
        foreach (Match m in matches)
        {
            Array statusDescription = GetArrayNameFromString(displayable, m.Value);

            if (statusDescription != null)
            {
                int arrayIndex = GetIndexFromString(m.Value);
                StatusBase status = ((GameObject)statusDescription.GetValue(arrayIndex)).GetComponent<StatusBase>();
                description = description.Replace("<<" + m.Value + ">>", "<b><color=lime>" + status.Name + "</color></b>");
                description += "\n___________________________________________________";
                description += DescriptionBuilder(status);
            }
            else
            {
                description = description.Replace("<<" + m.Value + ">>", "<color=darkblue>" + m.Value + "</color>");
            }
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

        PropertyInfo prop = src.GetType().GetProperty(arrayName);
        if (prop == null)
        {
            return null;
        }

        return (Array)prop.GetValue(src, null);
    }

    /** GetIndexFromString, private static int
     * @Params : string
     * This method is used the get an ArrayIndex from a pattern type of NameOfArray[Index].
	 * We try to get the index inside the [] ans a string parameter.
     **/
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

    /** FormateTitle, private static string
     * @Params : string
     * Formates and returns the title of the Displayable
     **/
    private static string FormateTitle(string title)
    {
        return "<b><size=14><color=darkblue>" + title + "</color></size></b> \n";
    }

    /** FormateResources, private static string
     * @Params : float, string
     * Formates and returns the Resources needed in the Displayable
     **/
    private static string FormateResources(float value, string resourceType)
    {
        return "<b><size=12><color=lightblue>" + value.ToString() + " " + resourceType + "</color></size></b> \n";
    }

    /** FormateCoolDown, private static string
     * @Params : string
     * Formates and returns the cooldown of the Displayable
     **/
    private static string FormateCoolDown(string cooldown)
    {
        return "<b><size=12><color=lightblue>" + cooldown + " Cooldown </color></size></b> \n";
    }

    /** FormateCoolDown, private static string
     * @Params : IDisplayable
     * We try to check if we are handling a IStatusDisplayable If it is, we Formate the type of Displayable.
     **/
    private static string FormateStatusType(IDisplayable status)
    {
        string statusType = status is IBuff ? "Empowerment" : "Curse";
        return "<b><size=12><color=lightblue>" + statusType + "</color></size></b> \n";
    }
}