//--- Melia Script ----------------------------------------------------------
// Hunting Tasks
//--- Description -----------------------------------------------------------
// An account-wide bounty: pick one of three monsters near your level,
// defeat 500 of them and earn Hunting Points to spend in the Hunting Shop.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomHuntingTasksScript : GeneralScript
{
	private const string ShopName = "HuntingTaskShop";
	private const string ShopPointName = "hunting_task_shop";
	private const string ShopPointScript = "GET_PVP_POINT";

	private const string OptionsVar = "Melia.HuntingTask.Options";
	private const string MonsterVar = "Melia.HuntingTask.Monster";
	private const string KillsVar = "Melia.HuntingTask.Kills";
	private const string PointsVar = "Melia.HuntingTask.Points";
	private const string CompletedVar = "Melia.HuntingTask.Completed";

	private const int RequiredKills = 500;
	private const int RewardPoints = 5;
	private const int LevelRange = 10;

	protected override void Load()
	{
		if (!Feature.IsEnabled("CustomNpcs"))
			return;

		PropertyShops.CreateWithVariable(ShopName, ShopPointName, PointsVar, shop =>
		{
			shop.AddItem("Premium_indunReset", ItemId.Premium_IndunReset, 1, 10);
			shop.AddItem("161215Event_Seed", ItemId.I161215Event_Seed, 1, 10);
			shop.AddItem("Event_Goddess_Statue_DLC", ItemId.Event_Goddess_Statue_DLC, 1, 10);
			shop.AddItem("misc_ore15", ItemId.Misc_Ore15, 1, 75);
		});

		Dialog.RegisterPropertyShopForMap("c_Klaipe", ShopName, ShopPointScript);

		AddNpc(57223, L("[Hunting Tasks] Bruno"), "c_Klaipe", -300, 910, 0, this.BrunoDialog);
	}

	[On("EntityKilled")]
	private void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Mob mob || mob.OwnerHandle != 0)
			return;

		var character = mob.GetKillBeneficiary(args.Attacker);
		if (character == null)
			return;

		var vars = character.Connection.Account.Variables.Perm;
		if (vars.GetInt(MonsterVar, 0) != mob.Id)
			return;

		var kills = vars.GetInt(KillsVar, 0) + 1;
		if (kills > RequiredKills)
			return;

		vars.SetInt(KillsVar, kills);

		if (kills == RequiredKills)
		{
			vars.SetInt(PointsVar, vars.GetInt(PointsVar, 0) + RewardPoints);
			vars.SetInt(CompletedVar, vars.GetInt(CompletedVar, 0) + 1);
			character.ServerMessage(LF("Hunting Task complete! You earned {0} Hunting Points. Report to Bruno in Klaipeda for your next task.", RewardPoints));
		}
		else if (kills % 100 == 0)
		{
			character.ServerMessage(LF("Hunting Task: {0}/{1} {2} defeated.", kills, RequiredKills, mob.Data.Name));
		}
	}

	private async Task BrunoDialog(Dialog dialog)
	{
		var character = dialog.Player;
		var vars = character.Connection.Account.Variables.Perm;
		dialog.SetTitle(L("Hunting Tasks"));

		while (true)
		{
			var selection = await dialog.Select(LF("Every hunter on your account shares one task at a time. Finish it and I'll pay you in Hunting Points.{nl}{nl}Hunting Points: {0}{nl}Tasks completed: {1}", vars.GetInt(PointsVar, 0), vars.GetInt(CompletedVar, 0)),
				Option(L("Hunting Task"), "task"),
				Option(L("Hunting Shop"), "shop"),
				Option(L("Leave"), "exit"));

			if (selection == "exit")
				return;

			if (selection == "shop")
			{
				dialog.OpenPropertyShop(ShopName, null, ShopPointName);
				return;
			}

			await this.TaskMenu(dialog);
		}
	}

	private async Task TaskMenu(Dialog dialog)
	{
		var character = dialog.Player;
		var vars = character.Connection.Account.Variables.Perm;
		var monsterId = vars.GetInt(MonsterVar, 0);

		if (monsterId != 0)
		{
			var kills = vars.GetInt(KillsVar, 0);
			var name = ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var data) ? data.Name : "?";

			if (kills < RequiredKills)
			{
				var abandon = await dialog.Select(LF("Your current task: defeat {0} {1}.{nl}Progress: {2}/{0}", RequiredKills, name, kills),
					Option(L("Keep hunting"), "back"),
					Option(L("Abandon the task"), "abandon"));

				if (abandon == "abandon")
				{
					vars.Remove(MonsterVar);
					vars.Remove(KillsVar);
				}

				return;
			}

			vars.Remove(MonsterVar);
			vars.Remove(KillsVar);
			vars.Remove(OptionsVar);
			await dialog.Msg(LF("All {0} {1} taken care of. Fine work! Let's find you another target.", RequiredKills, name));
		}

		while (true)
		{
			var options = GetOptions(character);
			if (options.Count == 0)
			{
				await dialog.Msg(L("I have no bounties fit for you right now."));
				return;
			}

			var choices = options.Select((a, i) => Option(LF("{0} (Lv. {1}) - {2}", a.Name, a.Level, GetSpawnMapNames(a.Id)), i.ToString())).ToList();
			choices.Add(Option(L("Show me other monsters"), "reroll"));
			choices.Add(Option(L("Back"), "back"));

			var selection = await dialog.Select(LF("Choose your target. You'll need to defeat {0} of them.", RequiredKills), choices);

			if (selection == "back")
				return;

			if (selection == "reroll")
			{
				vars.Remove(OptionsVar);
				continue;
			}

			var monster = options[int.Parse(selection)];
			vars.SetInt(MonsterVar, monster.Id);
			vars.SetInt(KillsVar, 0);
			vars.Remove(OptionsVar);

			await dialog.Msg(LF("Good hunting. Come back once {0} {1} have fallen.", RequiredKills, monster.Name));
			return;
		}
	}

	private static List<MonsterData> GetOptions(Character character)
	{
		var vars = character.Connection.Account.Variables.Perm;
		var monsterDb = ZoneServer.Instance.Data.MonsterDb;

		var saved = vars.GetString(OptionsVar, null);
		if (!string.IsNullOrEmpty(saved))
		{
			var savedOptions = saved.Split(',').Select(a => monsterDb.TryFind(int.Parse(a), out var data) ? data : null).Where(a => a != null).ToList();
			if (savedOptions.Count > 0)
				return savedOptions;
		}

		var huntable = ZoneServer.Instance.Data.MapDb.Entries.Values
			.Where(a => a.Type == MapType.Field || a.Type == MapType.Dungeon)
			.SelectMany(a => a.SpawnedMonsterIds)
			.Distinct()
			.Select(a => monsterDb.TryFind(a, out var data) ? data : null)
			.Where(a => a != null && a.Rank == MonsterRank.Normal && a.Faction == FactionType.Monster)
			.ToList();

		var candidates = huntable.Where(a => Math.Abs(a.Level - character.Level) <= LevelRange).ToList();
		if (candidates.Count < 3)
			candidates = huntable.OrderBy(a => Math.Abs(a.Level - character.Level)).Take(20).ToList();

		var rnd = RandomProvider.Get();
		var options = candidates.OrderBy(_ => rnd.Next()).Take(3).ToList();

		vars.SetString(OptionsVar, string.Join(",", options.Select(a => a.Id)));

		return options;
	}

	private static string GetSpawnMapNames(int monsterId)
	{
		var names = ZoneServer.Instance.Data.MapDb.Entries.Values
			.Where(a => (a.Type == MapType.Field || a.Type == MapType.Dungeon) && a.SpawnedMonsterIds.Contains(monsterId))
			.Select(a => a.Name)
			.Take(2)
			.ToList();

		return string.Join(", ", names);
	}
}
