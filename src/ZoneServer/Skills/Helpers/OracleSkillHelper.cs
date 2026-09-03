using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Oracle skills that read and change a
	/// monster's drops.
	/// </summary>
	public static class OracleSkillHelper
	{
		private const int MaxPreviewedItems = 8;
		private const float PreviewDuration = 5;
		private const string PreviewBoxType = "reward_itembox";
		private const string PreviewStyle = "{@st43}";
		// A system message with no text, so the balloon shows only the item
		private const string PreviewMessage = "JunkSilverGachaResultInRaidRewardSmall";
		private const string PreviewTargetsVar = "Melia.Oracle.DropPreviewTargets";
		private const float DeprioritizedDropChance = 10f;
		private const float ProtectedDropChance = 1f;
		// The client keys one balloon frame per actor handle and accumulates
		// items into it; this is the addon function that empties and hides one
		private const string ClearBalloonScript = "ITEM_BALLOON_CLEAR({0})";

		/// <summary>
		/// Returns how many monsters the skill reads, from its first caption
		/// ratio, rounded up.
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static int GetTargetCount(Skill skill)
		{
			var value = skill.Properties.GetFloat(PropertyName.CaptionRatio);

			// A per-level coefficient like 0.6 lands just above a whole
			// number in float, which would grant a target a level too early
			return (int)Math.Ceiling(Math.Round(value, 4));
		}

		/// <summary>
		/// Returns the monsters worth rolling again for the given character,
		/// the ones already holding something uncommon last and the ones
		/// holding something genuinely rare not at all.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="targets"></param>
		/// <param name="maxTargets"></param>
		/// <returns></returns>
		public static List<Mob> SelectRerollTargets(Character character, IList<ICombatEntity> targets, int maxTargets)
		{
			var candidates = new List<(Mob Monster, int Rank)>();

			foreach (var target in targets)
			{
				if (target is not Mob monster)
					continue;

				var rarest = GetRarestDropChance(monster.PeekDrops(character));

				// Rolling again could lose it, so it is left alone entirely
				if (rarest <= ProtectedDropChance)
					continue;

				candidates.Add((monster, rarest <= DeprioritizedDropChance ? 1 : 0));
			}

			// OrderBy is stable, so monsters of equal worth stay in the
			// distance order they were selected in
			return candidates
				.OrderBy(a => a.Rank)
				.Take(maxTargets)
				.Select(a => a.Monster)
				.ToList();
		}

		/// <summary>
		/// Returns the lowest drop chance among the given stacks, or no
		/// chance at all if there are none.
		/// </summary>
		/// <param name="dropStacks"></param>
		/// <returns></returns>
		private static float GetRarestDropChance(List<DropStack> dropStacks)
		{
			var rarest = float.MaxValue;

			if (dropStacks == null)
				return rarest;

			foreach (var stack in dropStacks)
				rarest = Math.Min(rarest, stack.DropChance);

			return rarest;
		}

		/// <summary>
		/// Adds every item the monster is going to drop for the given
		/// character to the balloon above it, or an empty box if it is going
		/// to drop nothing. The client keys that balloon by actor handle and
		/// stacks the items into it.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monster"></param>
		/// <param name="dropStacks"></param>
		public static void ShowDropPreview(Character character, Mob monster, List<DropStack> dropStacks)
		{
			// Money drops in dozens of stacks on jackpot monsters, so the
			// balloons show one entry per item, not one per stack.
			var amounts = new Dictionary<int, int>();
			var order = new List<int>();

			if (dropStacks != null)
			{
				foreach (var stack in dropStacks)
				{
					if (!amounts.ContainsKey(stack.ItemId))
						order.Add(stack.ItemId);

					amounts.TryGetValue(stack.ItemId, out var amount);
					amounts[stack.ItemId] = amount + stack.Amount;
				}
			}

			var shown = 0;

			foreach (var itemId in order)
			{
				if (shown == MaxPreviewedItems)
					break;

				if (!ZoneServer.Instance.Data.ItemDb.Exists(itemId))
					continue;

				ShowBalloon(character, monster, new Item(itemId, amounts[itemId]));
				shown++;
			}

			// An empty box reads as "this one drops nothing"
			if (shown == 0)
				ShowBalloon(character, monster, null);

			GetPreviewTargets(character).Add(monster);
		}

		/// <summary>
		/// Hides the item balloons the character's previous preview put up,
		/// so only the newest cast's are on screen.
		/// </summary>
		/// <param name="character"></param>
		public static void HideDropPreviews(Character character)
		{
			var previewTargets = GetPreviewTargets(character);

			foreach (var monster in previewTargets)
				Send.ZC_EXEC_CLIENT_SCP(character.Connection, string.Format(ClearBalloonScript, monster.Handle));

			previewTargets.Clear();
		}

		/// <summary>
		/// Adds one item to the balloon above the monster, an empty box if no
		/// item is given.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monster"></param>
		/// <param name="item"></param>
		private static void ShowBalloon(Character character, Mob monster, Item item)
			=> Send.ZC_NORMAL.ShowItemBalloon(character, monster, item, PreviewBoxType, PreviewStyle, PreviewMessage, PreviewDuration);

		/// <summary>
		/// Returns the monsters the character currently has a drop preview
		/// on, creating the list if it doesn't exist yet.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		private static List<Mob> GetPreviewTargets(Character character)
		{
			var previewTargets = character.Variables.Temp.Get<List<Mob>>(PreviewTargetsVar);
			if (previewTargets == null)
			{
				previewTargets = new List<Mob>();
				character.Variables.Temp.Set(PreviewTargetsVar, previewTargets);
			}

			return previewTargets;
		}
	}
}
