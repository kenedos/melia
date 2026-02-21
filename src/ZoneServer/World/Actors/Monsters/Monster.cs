using System;
using System.Globalization;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Network;
using Melia.Zone.Scripting;

namespace Melia.Zone.World.Actors.Monsters
{
	public interface IMonster : ISubActor, IMonsterAppearance, IMonsterAppearanceBase, ISpawn
	{
		/// <summary>
		/// Returns the monster's type.
		/// </summary>
		RelationType MonsterType { get; }

		/// <summary>
		/// Returns the monster's race.
		/// </summary>
		RaceType Race { get; }

		/// <summary>
		/// Returns the monster's element/attribute.
		/// </summary>
		AttributeType Attribute { get; }

		/// <summary>
		/// Returns whether the monster emerged from the ground.
		/// </summary>
		/// <remarks>
		/// This determines the animation played when the monster spawns
		/// and should probably not be set all the time.
		/// </remarks>
		bool FromGround { get; set; }

		/// <summary>
		/// Returns the monster's current HP.
		/// </summary>
		int Hp { get; }

		int Shield { get; }

		/// <summary>
		/// Returns a reference to the monster's properties.
		/// </summary>
		Properties Properties { get; }
	}

	/// <summary>
	/// Represents a portion of a "monster", or in other words, an entity
	/// that is not a player, but can exist on a map.
	/// </summary>
	/// <remarks>
	/// Primarily used in communicating monster data to the client.
	/// </remarks>
	public interface IMonsterAppearanceBase
	{
		/// <summary>
		/// Returns the monster's class id.
		/// </summary>
		int Id { get; }

		/// <summary>
		/// Returns the monster's maximum HP.
		/// </summary>
		int MaxHp { get; }

		/// <summary>
		/// Returns the monster's maximum shield.
		/// </summary>
		int MaxShield { get; }

		/// <summary>
		/// Returns the monster's level.
		/// </summary>
		int Level { get; }

		/// <summary>
		/// Returns the monster's AoE/Skill Defense Ratio.
		/// </summary>
		float SDR { get; }

		/// <summary>
		/// Returns the monster's gen type, which is similar to an id.
		/// </summary>
		int GenType { get; }
	}

	/// <summary>
	/// Represents a portion of a "monster", or in other words, an entity
	/// that is not a player, but can exist on a map.
	/// </summary>
	/// <remarks>
	/// Primarily used in communicating monster data to the client.
	/// </remarks>
	public interface IMonsterAppearance
	{
		/// <summary>
		/// Returns the monster's name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Returns the monster's unique name (?).
		/// </summary>
		string UniqueName { get; }

		/// <summary>
		/// Returns the name of the dialog function to call when the
		/// monster gets triggered.
		/// </summary>
		/// <remarks>
		/// Not currently used in Melia, but required for the client to
		/// mark a monster as a potential conversation target.
		/// </remarks>
		string DialogName { get; }

		/// <summary>
		/// Returns the name of the function to call when someone enters
		/// this monster's trigger area.
		/// </summary>
		/// <remarks>
		/// Not currently used in Melia, but might serve a purpose on the
		/// client-side, similar to DialogName.
		/// </remarks>
		string EnterName { get; }

		/// <summary>
		/// Returns the name of the function to call when someone leaves
		/// this monster's trigger area.
		/// </summary>
		/// <remarks>
		/// Not currently used in Melia, but might serve a purpose on the
		/// client-side, similar to DialogName.
		/// </remarks>
		string LeaveName { get; }
	}

	public static class MobExtensions
	{
		/// <summary>
		/// Applies a property list string to the monster's properties.
		/// Format: PropertyName#Value#PropertyName2#Value2...
		/// </summary>
		public static void ApplyPropList(this Mob mob, string propertyList, DungeonScript owner, object skill)
		{
			if (string.IsNullOrEmpty(propertyList))
				return;

			var props = propertyList.Split('#');

			for (var i = 0; i < props.Length - 1; i += 2)
			{
				var propName = props[i].Trim();
				var propValue = props[i + 1].Trim();

				if (propName == "Name")
					mob.Name = propValue;
				if (propName == "Faction")
				{
					if (Enum.TryParse<FactionType>(propValue, out var faction))
						mob.SetFaction(faction);
				}

				// Skip if property doesn't exist in the property table
				if (!PropertyTable.Exists(mob.Properties.Namespace, propName))
					continue;

				// Try to parse as float first, then string
				if (float.TryParse(propValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var floatValue))
					mob.Properties.SetFloat(propName, floatValue);
				else
					mob.Properties.SetString(propName, propValue);
			}

			// Invalidate calculated properties after applying changes
			mob.Properties.InvalidateAll();
		}
	}
}
