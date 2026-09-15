//--- Melia Script ----------------------------------------------------------
// Stylist Custom NPC
//--- Description -----------------------------------------------------------
// Allows players to change their hair style and color.
//--- Details ---------------------------------------------------------------
// Due to the way the game handles hair types it's not possible to easily
// iterate over the available types and specific numbers of colors within
// them, since the number of styles varies between genders, same as the
// number and types of colors per style. As such, we need to constantly
// iterate over all styles and pick out the next/previous one that makes
// sense to get a RO-like stylist.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomNpcStylist : GeneralScript
{
	protected override void Load()
	{
		AddNpc(57223, L("[Stylist] Jeremy"), "c_Klaipe", -66, -547, 180, NpcDialog);
		AddNpc(57223, L("[Stylist] Jeremy"), "c_fedimian", 245, -137, 0, NpcDialog);
		AddNpc(57223, L("[Stylist] Jeremy"), "c_orsha", 385, 600, 0, NpcDialog);
	}

	private async Task NpcDialog(Dialog dialog)
	{
		var pc = dialog.Player;
		var hairType = GetHairType(pc.Gender, pc.Hair);

		dialog.SetTitle(L("Jeremy"));
		dialog.SetPortrait("Dlg_port_Vlaentinas_Naimon");

		// If player's current hairstyle doesn't exist in database (e.g., after removing styles),
		// default to the first available hairstyle for their gender
		if (hairType == null)
		{
			hairType = ZoneServer.Instance.Data.HairTypeDb.Entries.FirstOrDefault(a => a.Gender == pc.Gender);
			if (hairType != null)
				pc.ChangeHair(hairType.Index);
		}

		while (true)
		{
			var selection = await dialog.Select(L("What can I do for you today?"),
				Option(L("Change Hair Style"), "hair"),
				Option(L("Change Hair Color"), "color"),
				Option(L("Nothing"), "end")
			);

			if (selection == "end")
			{
				await dialog.Msg(L("Please come back any time."));
				return;
			}

			var changeType = selection == "hair" ? StyleChangeType.Hair : StyleChangeType.Color;
			var direction = RotationDirection.Forward;

			while (true)
			{
				var options = GetOptions(direction);

				pc.ChangeHair(hairType.Index);

				var coloredName = GetColoredText(L(hairType.Name), hairType.Color);
				var coloredColor = GetColoredText(hairType.Color, hairType.Color);
				selection = await dialog.Select(LF("This style is called {0} in {1} (#{2}), what do you think?", coloredName, coloredColor, hairType.Index), options);

				switch (selection)
				{
					case "next":
					{
						if (changeType == StyleChangeType.Hair)
							hairType = GetNextStyle(pc.Gender, hairType);
						else
							hairType = GetNextColor(pc.Gender, hairType);

						direction = RotationDirection.Forward;
						break;
					}
					case "prev":
					{
						if (changeType == StyleChangeType.Hair)
							hairType = GetPrevStyle(pc.Gender, hairType);
						else
							hairType = GetPrevColor(pc.Gender, hairType);

						direction = RotationDirection.Backward;
						break;
					}
					case "jump":
					{
						var jumpStr = await dialog.Input(L("Which style would you like to see, do you have a number for me?"));

						if (!int.TryParse(jumpStr, out var index))
						{
							await dialog.Msg(L("Hm... Not a number, is it?"));
							break;
						}

						hairType = GetHairType(pc.Gender, index);
						if (hairType == null)
						{
							await dialog.Msg(L("I'm sorry, I don't know that style."));
							break;
						}

						break;
					}
					default:
					{
						// Go back to main menu
						break;
					}
				}

				if (selection == "end")
					break;
			}
		}
	}

	private DialogOption[] GetOptions(RotationDirection dir)
	{
		var options = new DialogOption[4];

		if (dir == RotationDirection.Forward)
		{
			options[0] = Option(L("Next"), "next");
			options[1] = Option(L("Previous"), "prev");
		}
		else
		{
			options[0] = Option(L("Previous"), "prev");
			options[1] = Option(L("Next"), "next");
		}

		options[2] = Option(L("Jump"), "jump");
		options[3] = Option(L("I like it"), "end");
		return options;
	}

	private HairTypeData GetHairType(Gender gender, int index)
		=> ZoneServer.Instance.Data.HairTypeDb.Entries.FirstOrDefault(a => a.Gender == gender && a.Index == index);

	private string GetColoredText(string text, string colorName)
	{
		var hexColor = colorName.ToLower() switch
		{
			"default" => "9B7653",
			"black" => "2A2A2A",
			"blue" => "4A7DB8",
			"pink" => "D48AA0",
			"white" => "A0A0A0",
			"blond" => "C9A86C",
			"red" => "B85454",
			"green" => "5A9B6B",
			"gray" => "7A7A7A",
			"lightsalmon" => "D4937A",
			"purple" => "8B6AAE",
			"orange" => "D4864A",
			"rightorange" => "D4864A",
			"brown" => "8B6B4A",
			"midnightblue" => "3A4A6B",
			"rightviolet" => "9B6AAE",
			"ashgrey" => "8A8A8A",
			"ashblue" => "6B7A8B",
			"ashgreen" => "6B8B7A",
			"indigo" => "4A5A8B",
			"whiteviolet" => "B8A0C8",
			"rightmint" => "6BB8A0",
			"silvergreen" => "8BAA8B",
			"gold" => "C9A840",
			"applemint" => "7AC8A0",
			"rightgreen" => "5AAA6B",
			"rightpink" => "D48AAA",
			"rubywine" => "8B4A5A",
			"ashwine" => "8B6A7A",
			"charcoal" => "4A4A4A",
			"copper" => "B87A4A",
			"flame" => "D46A4A",
			"violet" => "8B5AAE",
			"mint" => "6AC8A0",
			"lightbrown" => "AA8B6A",
			"darkblue" => "3A4A7A",
			"paleblue" => "8AAAC8",
			_ => "9B9B9B"
		};
		return $"{{#{hexColor}}}{text}{{/}}";
	}

	private HairTypeData GetHairType(Gender gender, string className)
		=> ZoneServer.Instance.Data.HairTypeDb.Entries.FirstOrDefault(a => a.Gender == gender && a.ClassName == className);

	private HairTypeData GetNextStyle(Gender gender, HairTypeData curType)
	{
		var list = gender == Gender.Male ? MaleStyles : FemaleStyles;
		var index = Array.IndexOf(list, curType.ClassName);
		var nextIndex = index + 1;

		if (nextIndex >= list.Length)
			nextIndex = 0;

		var className = list[nextIndex];
		return GetHairType(gender, className);
	}

	private HairTypeData GetPrevStyle(Gender gender, HairTypeData curType)
	{
		var list = gender == Gender.Male ? MaleStyles : FemaleStyles;
		var index = Array.IndexOf(list, curType.ClassName);
		var prevIndex = index - 1;

		if (prevIndex < 0)
			prevIndex = list.Length - 1;

		var className = list[prevIndex];
		return GetHairType(gender, className);
	}

	private HairTypeData GetNextColor(Gender gender, HairTypeData curType)
	{
		var result = (HairTypeData)null;

		foreach (var hairType in ZoneServer.Instance.Data.HairTypeDb.Entries)
		{
			if (hairType.Gender == gender && hairType.ClassName == curType.ClassName)
			{
				if (hairType.Index > curType.Index)
				{
					result = hairType;
					break;
				}

				if (result == null)
					result = hairType;
			}
		}

		return result;
	}

	private HairTypeData GetPrevColor(Gender gender, HairTypeData curType)
	{
		var result = (HairTypeData)null;

		foreach (var hairType in ZoneServer.Instance.Data.HairTypeDb.Entries)
		{
			if (hairType.Gender == gender && hairType.ClassName == curType.ClassName)
			{
				if (hairType.Index == curType.Index && result != null)
					break;

				result = hairType;
			}
		}

		return result;
	}

	private readonly string[] MaleStyles = ZoneServer.Instance.Data.HairTypeDb.Entries.Where(a => a.Gender == Gender.Male).OrderBy(a => a.Index).Select(a => a.ClassName).Distinct().ToArray();
	private readonly string[] FemaleStyles = ZoneServer.Instance.Data.HairTypeDb.Entries.Where(a => a.Gender == Gender.Female).OrderBy(a => a.Index).Select(a => a.ClassName).Distinct().ToArray();

	private enum StyleChangeType
	{
		Hair,
		Color,
	}

	private enum RotationDirection
	{
		Forward,
		Backward,
	}
}
