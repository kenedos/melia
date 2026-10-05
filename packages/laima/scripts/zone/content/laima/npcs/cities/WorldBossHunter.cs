using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using static Melia.Zone.Scripting.Shortcuts;


namespace ScriptsZoneLaima.content.laima.npcs.cities
{
	internal class WorldBossHunter : GeneralScript
	{
		private const string QuestClassName = "world_boss";
		private const int QuestIdValue = 1001;
		private const string ParticipationsVariable = "Laima.WorldBoss.Participations";
		private const int GoldenCoinItemId = 902064;
		private const int GoldenCoinsPerParticipation = 5;

		protected override void Load()
		{
			AddNpc(151049, L("[World Boss Hunter] Eduardo"), "Eduardo", "c_Klaipe", -650, 720, 0, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId(QuestClassName, QuestIdValue);

				dialog.SetTitle(L("World Boss Manager"));
				dialog.SetPortrait("KLAPEDA_BLACKSMITH");

				if (!character.Quests.IsActive(questId))
				{
					await this.ShowQuestIntroduction(dialog, character, questId);
					return;
				}

				await this.ShowMainMenu(dialog, character);
			});
		}

		private async Task ShowQuestIntroduction(Dialog dialog, Character character, QuestId questId)
		{
			var response = await dialog.Select(
				L("Powerful World Bosses appear throughout the world every four hours.{nl}{nl}Participate in their defeat and I will record your contribution. Each successful participation can later be exchanged for five Golden Coins."),
				Option(L("Register as a World Boss Hunter"), "accept"),
				Option(L("Leave"), "leave"));

			if (response != "accept")
				return;

			await character.Quests.Start(questId);

			await dialog.Msg(L("You are now registered as a World Boss Hunter.{nl}{nl}World Bosses appear every four hours at:{nl}00:00{nl}04:00{nl}08:00{nl}12:00{nl}16:00{nl}20:00{nl}{nl}Brasília time.{nl}{nl}Deal damage to a World Boss and help defeat it to earn participation credit."));

			await this.ShowMainMenu(dialog, character);
		}

		private async Task ShowMainMenu(Dialog dialog, Character character)
		{
			var participations = character.Variables.Perm.GetInt(ParticipationsVariable, 0);

			var response = await dialog.Select(
				L("World Boss Hunter Registry{nl}{nl}Unclaimed participations: " + participations + "{nl}Reward per participation: 5 Golden Coin"),
				Option(L("Claim Golden Coins"), "claim"),
				Option(L("Current World Boss"), "current_boss"),
				Option(L("World Boss Information"), "info"),
				Option(L("Leave"), "leave"));

			switch (response)
			{
				case "claim":
					await this.ClaimRewards(dialog, character);
					break;

				case "current_boss":
					await this.ShowCurrentWorldBoss(dialog, character);
					break;

				case "info":
					await this.ShowWorldBossInformation(dialog, character);
					break;
			}
		}

		private async Task ClaimRewards(Dialog dialog, Character character)
		{
			var participations = character.Variables.Perm.GetInt(ParticipationsVariable, 0);

			if (participations <= 0)
			{
				await dialog.Msg(L("You do not have any unclaimed World Boss participations."));
				await this.ShowMainMenu(dialog, character);
				return;
			}

			var goldenCoins = participations * GoldenCoinsPerParticipation;

			character.Inventory.Add(GoldenCoinItemId, goldenCoins, InventoryAddType.PickUp);
			character.Variables.Perm.Set(ParticipationsVariable, 0);
			WorldBossParticipationObjective.ResetParticipation(character);

			await dialog.Msg(L("World Boss rewards claimed!{nl}{nl}Participations: " + participations + "{nl}Golden Coins received: " + goldenCoins));

			await this.ShowMainMenu(dialog, character);
		}

		private async Task ShowCurrentWorldBoss(Dialog dialog, Character character)
		{
			var manager = WorldBossManager.Instance;

			if (manager == null || !manager.HasActiveBoss)
			{
				await dialog.Msg(L("There is currently no active World Boss.{nl}{nl}World Bosses appear every four hours at:{nl}00:00{nl}04:00{nl}08:00{nl}12:00{nl}16:00{nl}20:00{nl}{nl}Brasília time."));
				await this.ShowMainMenu(dialog, character);
				return;
			}

			var monsterName = "Unknown World Boss";

			if (ZoneServer.Instance.Data.MonsterDb.TryFind(manager.ActiveMonsterId, out var monsterData))
				monsterName = monsterData.Name;

			var mapName = string.IsNullOrEmpty(manager.ActiveMapName) ? "Unknown Map" : manager.ActiveMapName;

			await dialog.Msg(L("Current World Boss{nl}{nl}Boss: " + monsterName + "{nl}Map: " + mapName + "{nl}{nl}The World Boss is currently alive."));

			await this.ShowMainMenu(dialog, character);
		}

		private async Task ShowWorldBossInformation(Dialog dialog, Character character)
		{
			await dialog.Msg(L("World Bosses appear every four hours at fixed Brasília times:{nl}{nl}00:00{nl}04:00{nl}08:00{nl}12:00{nl}16:00{nl}20:00{nl}{nl}You must have the World Boss Hunters' Ledger active and participate in the battle to receive credit."));

			await this.ShowMainMenu(dialog, character);
		}
	}

	public class WorldBossHuntersLedgerQuest : QuestScript
	{
		protected override void Load()
		{
			this.SetId("world_boss", 1001);
			this.SetName(L("World Boss Hunters' Ledger"));
			this.SetType(QuestType.Repeat);
			this.SetDescription(L("Participate in World Boss battles throughout the world. Each World Boss you help defeat grants one participation, which can be exchanged for five Golden Coins."));
			this.SetLocation("c_Klaipe");
			this.SetAutoTracked(true);
			this.SetReceive(QuestReceiveType.Manual);
			this.SetCancelable(true);
			this.SetUnlock(QuestUnlockType.AllAtOnce);
			this.AddQuestGiver(L("[World Boss Manager] Eduardo"), "c_Klaipe");

			this.AddObjective("worldBossParticipation", L("Participate in World Boss battles"), new WorldBossParticipationObjective());
		}
	}
}
