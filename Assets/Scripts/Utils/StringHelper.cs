using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringHelper {
	
	public static string SpellDescriptionBuilder(Spell spell, object[] descriptionVariables)
	{
		string title = spell.SpellDefinition.Name;
		string description = string.Join("",spell.SpellDefinition.Description); 
		title = "<b><size=14><color=darkblue>" + title + "</color></size></b>";
		
		string pattern = @"(?<=\{).+?(?=\})";
		MatchCollection matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);
		
		if(descriptionVariables != null) 
		{
			for (int i = 0; i < descriptionVariables.Length; i++)
			{
				if(matches[i].Value[0] == 'm')
				{
					string color = "cyan";
					description = description.Replace("{"+matches[i].Value+"}", "<b><color="+color+">{"+matches[i].Value.TrimStart('m')+"}</color></b>");
					continue;
				}
				
				if(matches[i].Value[0] == 'p')
				{
					string color = "maroon";
					description = description.Replace("{"+matches[i].Value+"}", "<b><color="+color+">{"+matches[i].Value.TrimStart('p')+"}</color></b>");
					continue;
				}
				
				if(matches[i].Value[0] == 'd')
				{
					string color = spell.SpellDefinition.Type == "Magical" ? "cyan":"maroon";
					description = description.Replace("{"+matches[i].Value+"}", "<b><color="+color+">{"+matches[i].Value.TrimStart('d')+"}</color></b>");
				}
				else
				{
					description = description.Replace("{"+matches[i].Value+"}", "<b>{"+matches[i].Value+"}</b>");
				}
			}
		
			description = string.Format(description, descriptionVariables);
		}
		
		pattern = @"(?<=<<).*?(?=>>)";
		matches = Regex.Matches(description, pattern, RegexOptions.IgnoreCase);
		foreach (Match m in matches)
		{
			description = description.Replace("<<"+m.Value+">>", "<b><color=lime>"+m.Value+"</color></b>");
			description += "\n___________________________________________________\n";
			description += "StringHelper TODO : Dynamic Description of Status";
		}
		
		string finaldescription = title + "\n"
		                        + description; 
		return finaldescription;
	}
}
