//--- Melia Script ----------------------------------------------------------
// Death Penalty
//--- Description -----------------------------------------------------------
// Takes silver and drops gems, cards and Blessed Shards from characters
// that die on maps with enough stars. Dropped items can only be picked
// back up by the character that lost them.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;

public class DeathPenaltyScript : GeneralScript
{
	private class Penalty
	{
		public float SilverRate;
		public int Gems;
		public int Cards;
		public int BlessedStones;
	}

	private static readonly Dictionary<int, Penalty> Penalties = new()
	{
		[2] = new Penalty { SilverRate = 0.02f },
		[3] = new Penalty { SilverRate = 0.02f, Gems = 2, Cards = 1 },
		[4] = new Penalty { SilverRate = 0.05f, Gems = 3, Cards = 1, BlessedStones = 1 },
	};

	[On("PlayerEnteredMap")]
	private void OnPlayerEnteredMap(object sender, PlayerEventArgs args)
	{
		var character = args.Character;

		if (!TryGetPenalty(character, out var penalty))
			return;

		var msg = (penalty.Gems > 0 || penalty.Cards > 0 || penalty.BlessedStones > 0) ? "DeathPenaltyGemSilverETC" : "DeathPenaltyGemSilver";
		character.SystemMessage(msg, false);
	}

	[On("EntityKilled")]
	private void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Character character || character is DummyCharacter)
			return;

		if (args.Attacker is Character || character.Map.IsPVP || character.Map.IsGTW)
			return;

		if (!TryGetPenalty(character, out var penalty))
			return;

		TakeSilver(character, penalty.SilverRate);

		var droppedGems = DropRandomItems(character, penalty.Gems, static i => i.Data.Group == ItemGroup.Gem || i.Data.Group == ItemGroup.Gem_High_Color);
		DropRandomItems(character, penalty.Cards, static i => i.Data.Group == ItemGroup.Card);
		DropRandomItems(character, penalty.BlessedStones, static i => i.Data.ClassName.StartsWith("misc_BlessedStone"));

		if (droppedGems > 0)
			character.SystemMessage("DropGemByDeathPenalty");
	}

	private static bool TryGetPenalty(Character character, out Penalty penalty)
	{
		penalty = null;

		if (!ZoneServer.Instance.Conf.World.MapDeathPenalty || character.Map == null)
			return false;

		var rank = ZoneServer.Instance.Data.MapRankDb.GetRank(character.Map.ClassName);
		return Penalties.TryGetValue(rank, out penalty);
	}

	private static void TakeSilver(Character character, float rate)
	{
		var silver = character.Inventory.CountItem(ItemId.Silver);
		var amount = (int)(silver * rate);
		if (amount <= 0)
			return;

		character.Inventory.Remove(ItemId.Silver, amount, InventoryItemRemoveMsg.Destroyed);
		character.SystemMessage("YouDeadSoSomeSilverHasBeenLost{SILVER}", new MsgParameter("SILVER", amount));
	}

	/// <summary>
	/// Drops one piece each of up to count random inventory items that
	/// match the predicate, and returns how many were dropped.
	/// </summary>
	private static int DropRandomItems(Character character, int count, Func<Item, bool> predicate)
	{
		if (count <= 0)
			return 0;

		var candidates = character.Inventory.GetItems(predicate).Values.ToList();
		var rnd = GameRandom.Get();
		var dropped = 0;

		while (dropped < count && candidates.Count > 0)
		{
			var index = rnd.Next(candidates.Count);
			var item = candidates[index];
			var fullStack = item.Amount <= 1;

			if (character.Inventory.Remove(item, 1, InventoryItemRemoveMsg.Destroyed) != InventoryResult.Success)
			{
				candidates.RemoveAt(index);
				continue;
			}

			if (fullStack)
				candidates.RemoveAt(index);

			DropOwned(character, fullStack ? item : new Item(item, 1));
			dropped++;
		}

		return dropped;
	}

	private static void DropOwned(Character character, Item item)
	{
		var lifetime = TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.DropDisappearSeconds + 5);
		var rnd = GameRandom.Get();

		item.IsLocked = false;
		item.SetLootProtection(character, lifetime);
		item.Drop(character.Map, character.Position, new Direction(rnd.Next(0, 360)), rnd.Next(10, 40), character.AccountObjectId, character.Layer);
	}
}
