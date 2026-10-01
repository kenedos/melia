using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class AchievementReward : GeneralScript
{
	public static void SetAllowHairColor(Character character, string newColor, string achieveName)
	{
		string[] colorTable = { "default", "black", "blue", "white", "pink", "blond", "red", "green", "gray", "lightsalmon", "purple", "orange", "midnightblue" };
		var index = -1;
		for (var i = 0; i < colorTable.Length; i++)
		{
			if (newColor == colorTable[i])
			{
				index = i;
				break;
			}
		}

		if (index == -1)
			return;

		var etcColor = $"HairColor_{colorTable[index]}";
		var etc = character.Etc.Properties;

		if (etc[etcColor] != 1)
			character.SetEtcProperty(etcColor, 1);
	}

	public static bool HaveHairColor(Character character, string color)
	{
		var etc = character.Etc.Properties;
		var nowAllowedColor = etc.GetString("AllowedHairColor");

		if (nowAllowedColor.Contains(color) || etc[$"HairColor_{color}"] == 1)
			return true;
		return false;
	}

	public static void SetAllowHairColorBlack(Character character, string achieveName)
	{
		var etc = character.Etc.Properties;
		var etcPropName = "AchieveReward_" + achieveName;

		if (etc == null || etc[etcPropName] == 1)
		{
			return;
		}

		if (HaveHairColor(character, "black"))
		{
			character.SetEtcProperty(etcPropName, 1);
			character.SystemMessage("GainAchieveHairBefore");
			character.AddonMessage("ACHIEVE_REWARD");
			return;
		}

		SetAllowHairColor(character, "black", achieveName);

		character.SystemMessage("GainAchieveHairBlack");
		character.AddonMessage("ACHIEVE_REWARD", "", 0);
	}
}
