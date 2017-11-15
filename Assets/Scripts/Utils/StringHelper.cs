using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringHelper
{

    public static string SpellDescriptionBuilder(Spell spell, object[] descriptionVariables)
    {
        string title = spell.SpellDefinition.Name;
        title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";

        string resources = "Resources_Value" + " " + "Resources_Type";
        resources = "<b><size=12><color=lightblue>" + resources + "</color></size></b>";

        string cooldown = SecToMinConverter(spell.spellCD) + " cooldown";
        cooldown = "<b><size=12><color=lightblue>" + cooldown + "</color></size></b>";

        string description = string.Join("", spell.SpellDefinition.Description);
        string pattern = @"(?<=\{).+?(?=\})";
        MatchCollection matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);

        if (descriptionVariables != null)
        {
            for (int i = 0; i < descriptionVariables.Length; i++)
            {
                if (matches[i].Value[0] == 'm' || matches[i].Value[0] == 'p')
                {
                    string color = matches[i].Value[0] == 'm' ? "cyan" : "maroon";
                    description = description.Replace("{" + matches[i].Value + "}", "<b><color=" + color + ">{" + matches[i].Value.TrimStart('m') + "}</color></b>");
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

        string finaldescription = title + "\n"
                                + resources + "\n"
                                + cooldown + "\n"
                                + "\n"
                                + description;
        return finaldescription;
    }

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