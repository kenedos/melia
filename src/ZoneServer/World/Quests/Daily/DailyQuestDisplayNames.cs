using System;
using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Quests.Daily
{
	public static class DailyQuestDisplayNames
	{
		public static string GetRaceDisplayName(
		RaceType race)
		{
			return race switch
			{
				RaceType.Klaida => "Insect",
				RaceType.Paramune => "Mutant",
				RaceType.Forester => "Plant",
				RaceType.Velnias => "Demon",
				RaceType.Widling => "Beast",
				RaceType.Human => "Human",
				RaceType.Item => "Item",
				RaceType.None => "Unknown",
				_ => FormatInternalName(
				race.ToString()),
			};
		}

		public static string GetRaceDisplayName(
		int race)
		{
			return GetRaceDisplayName(
			(RaceType)race);
		}

		public static string GetAttributeDisplayName(
		AttributeType attribute)
		{
			return attribute switch
			{
				AttributeType.None => "Unknown",
				AttributeType.Soul => "Psychokinesis",
				AttributeType.Melee => "Melee",
				AttributeType.Magic => "Magic",
				_ => FormatInternalName(
				attribute.ToString()),
			};
		}

		public static string GetAttributeDisplayName(
		int attribute)
		{
			return GetAttributeDisplayName(
			(AttributeType)attribute);
		}

		public static string GetSizeDisplayName(
		SizeType size)
		{
			return size switch
			{
				SizeType.VS => "Very Small",
				SizeType.SS => "Super Small",
				SizeType.S => "Small",
				SizeType.M => "Medium",
				SizeType.L => "Large",
				SizeType.XL => "Extra Large",
				SizeType.XXL => "Extra Extra Large",
				SizeType.XXXL => "Extra Extra Extra Large",
				SizeType.EX => "Extremely Large",
				_ => FormatInternalName(
				size.ToString()),
			};
		}

		public static string GetSizeDisplayName(
		int size)
		{
			return GetSizeDisplayName(
			(SizeType)size);
		}

		public static string GetMapDisplayName(
		string mapClassName)
		{
			if (string.IsNullOrWhiteSpace(
			mapClassName))
			{
				return "Unknown Map";
			}

			var mapData =
			ZoneServer.Instance.Data.MapDb.Find(
			mapClassName);

			if (mapData != null &&
			!string.IsNullOrWhiteSpace(
			mapData.Name))
			{
				return mapData.Name;
			}

			return FormatMapClassName(
			mapClassName);
		}

		private static string FormatMapClassName(
		string mapClassName)
		{
			var normalized =
			mapClassName
			.Trim()
			.ToLowerInvariant();

			if (normalized.StartsWith("f_") ||
			normalized.StartsWith("c_") ||
			normalized.StartsWith("d_"))
			{
				normalized =
				normalized.Substring(2);
			}

			var parts =
			normalized.Split(
			'_',
			StringSplitOptions.RemoveEmptyEntries);

			while (parts.Length > 1 &&
			int.TryParse(
			parts[parts.Length - 1],
			out _))
			{
				Array.Resize(
				ref parts,
				parts.Length - 1);
			}

			return FormatInternalName(
			string.Join(
			" ",
			parts));
		}

		private static string FormatInternalName(
		string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return "Unknown";

			var normalized =
			value
			.Trim()
			.Replace("_", " ")
			.Replace("-", " ");

			var words =
			normalized.Split(
			' ',
			StringSplitOptions.RemoveEmptyEntries);

			for (var i = 0; i < words.Length; i++)
			{
				if (words[i].Length == 1)
				{
					words[i] =
					words[i].ToUpperInvariant();

					continue;
				}

				words[i] =
				char.ToUpperInvariant(
				words[i][0]) +
				words[i]
				.Substring(1)
				.ToLowerInvariant();
			}

			return string.Join(
			" ",
			words);
		}
	}
}
