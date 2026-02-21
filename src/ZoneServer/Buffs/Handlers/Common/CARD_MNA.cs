using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for CARD_* stat buffs (CARD_MNA, CARD_CON).
	/// This is a generic buff that increases any stat property based on the property name passed.
	/// Used by cards like Lavenzard (SPR +[★ * 6] for 10s) and Stone Froster (CON +[★ * 6] for 10s).
	/// </summary>
	/// <remarks>
	/// NumArg1: Star level
	/// Vars["Melia.Card.PropertyName"]: Property name to modify (e.g., "MNA_ITEM_BM", "CON_ITEM_BM", "DEX_ITEM_BM")
	/// Note: Only CARD_CON and CARD_MNA exist in the client. Other stat cards (DEX, INT, STR) use CARD_MNA as fallback.
	/// </remarks>
	[BuffHandler(BuffId.CARD_MNA, BuffId.CARD_CON)]
	public class CARD_MNA : BuffHandler
	{
		private const float BonusPerStar = 6f;
		private const string PropertyNameKey = "Melia.Card.PropertyName";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var starLevel = buff.NumArg1;
			var propertyName = buff.Vars.GetString(PropertyNameKey);

			if (string.IsNullOrEmpty(propertyName))
				return;

			if (!target.Properties.Has(propertyName))
				return;

			var bonus = starLevel * BonusPerStar;

			AddPropertyModifier(buff, target, propertyName, bonus);

			// Invalidate the main stat property (e.g., DEX_ITEM_BM -> DEX)
			if (target is Character character)
			{
				var mainStatName = GetMainStatName(propertyName);
				if (mainStatName != null)
					character.InvalidateProperties(mainStatName);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			var propertyName = buff.Vars.GetString(PropertyNameKey);

			if (string.IsNullOrEmpty(propertyName))
				return;

			RemovePropertyModifier(buff, target, propertyName);

			// Invalidate the main stat property (e.g., DEX_ITEM_BM -> DEX)
			if (target is Character character)
			{
				var mainStatName = GetMainStatName(propertyName);
				if (mainStatName != null)
					character.InvalidateProperties(mainStatName);
			}
		}

		/// <summary>
		/// Extracts the main stat name from a bonus property name.
		/// E.g., "DEX_ITEM_BM" -> "DEX", "CON_ITEM_BM" -> "CON"
		/// </summary>
		private static string GetMainStatName(string bonusPropertyName)
		{
			// Pattern: STAT_ITEM_BM or STAT_BM
			var underscoreIndex = bonusPropertyName.IndexOf('_');
			if (underscoreIndex > 0)
				return bonusPropertyName.Substring(0, underscoreIndex);

			return null;
		}
	}
}
