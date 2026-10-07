//--- Melia Script ----------------------------------------------------------
// World Boss
//--- Description -----------------------------------------------------------
// Spawns a random world boss on a random field at fixed hours. Registered
// hunters who damage it earn a participation, which the World Boss Hunter
// exchanges for Golden Coins.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomWorldBossScript : GeneralScript
{
	private static readonly int[] SpawnHours = { 0, 4, 8, 12, 16, 20 };
	private const string BossClassNamePrefix = "M_random_boss_";
	private const int MinMapLevel = 200;

	private const int GoldenCoinItemId = ItemId.Event_Steam_Night_Market_Gold;
	private const int CoinsPerParticipation = 5;
	private const string RegisteredVar = "Melia.WorldBoss.Registered";
	private const string ParticipationsVar = "Melia.WorldBoss.Participations";

	private readonly object _syncLock = new();
	private readonly HashSet<Character> _participants = new();
	private Mob _boss;
	private DateTime _lastSlot;

	protected override void Load()
	{
		if (!Feature.IsEnabled("CustomNpcs"))
			return;

		AddNpc(151049, L("[World Boss Hunter] Eduardo"), "c_Klaipe", -650, 720, 0, this.HunterDialog);
	}

	[On("HourTick")]
	private void OnHourTick(object sender, TimeEventArgs args)
	{
		if (!SpawnHours.Contains(args.Now.Hour))
			return;

		var slot = args.Now.Date.AddHours(args.Now.Hour);

		lock (_syncLock)
		{
			if (_lastSlot == slot)
				return;

			_lastSlot = slot;
			this.RemoveBoss();
		}

		if (!this.SpawnBoss())
			Log.Warning("World Boss: Failed to spawn a boss for the {0:HH:mm} slot.", slot);
	}

	private bool SpawnBoss()
	{
		var bosses = ZoneServer.Instance.Data.MonsterDb.Entries.Values.Where(a => a.ClassName.StartsWith(BossClassNamePrefix, StringComparison.OrdinalIgnoreCase)).ToList();
		if (bosses.Count == 0)
			return false;

		var maps = ZoneServer.Instance.World.Maps.GetList(a => a.Data.Type == MapType.Field && a.Data.SpawnedMonsterIds.Count > 0 && a.Ground.HasData()).ToList();
		var highLevelMaps = maps.Where(a => a.Data.Level >= MinMapLevel).ToList();
		if (highLevelMaps.Count > 0)
			maps = highLevelMaps;

		if (maps.Count == 0)
			return false;

		var rnd = RandomProvider.Get();
		var map = maps[rnd.Next(maps.Count)];
		if (!map.Ground.TryGetRandomPosition(out var position))
			return false;

		var data = bosses[rnd.Next(bosses.Count)];
		var boss = new Mob(data.Id, RelationType.Enemy);
		boss.Position = position;
		boss.SpawnPosition = position;
		boss.Direction = new Direction(rnd.Next(360));
		boss.Tendency = TendencyType.Aggressive;
		boss.Components.Add(new MovementComponent(boss));
		boss.Components.Add(new AiComponent(boss, "BasicMonster"));
		boss.Damaged += this.OnBossDamaged;
		boss.Died += this.OnBossDied;

		lock (_syncLock)
		{
			_participants.Clear();
			_boss = boss;
		}

		map.AddMonster(boss);

		Log.Info("World Boss: {0} spawned in {1} ({2}).", data.Name, map.Data.Name, map.ClassName);
		Send.ZC_NORMAL.WorldMessage(1, LF("[World Boss] {0} has appeared in {1}!", data.Name, map.Data.Name));

		return true;
	}

	private void RemoveBoss()
	{
		if (_boss == null)
			return;

		_boss.Damaged -= this.OnBossDamaged;
		_boss.Died -= this.OnBossDied;

		if (!_boss.IsDead)
			_boss.Map?.RemoveMonster(_boss);

		_boss = null;
		_participants.Clear();
	}

	private void OnBossDamaged(Mob boss, ICombatEntity attacker, float damage)
	{
		var character = attacker as Character
			?? attacker.Components.Get<AiComponent>()?.Script.GetMaster() as Character
			?? (attacker as Summon)?.Owner as Character;

		if (character == null || !character.Variables.Perm.GetBool(RegisteredVar))
			return;

		lock (_syncLock)
		{
			if (boss == _boss)
				_participants.Add(character);
		}
	}

	private void OnBossDied(Mob boss, ICombatEntity killer)
	{
		List<Character> participants;

		lock (_syncLock)
		{
			if (boss != _boss)
				return;

			participants = _participants.ToList();
			_boss.Damaged -= this.OnBossDamaged;
			_boss.Died -= this.OnBossDied;
			_boss = null;
			_participants.Clear();
		}

		foreach (var character in participants)
		{
			if (character.Connection == null)
				continue;

			character.Variables.Perm.SetInt(ParticipationsVar, character.Variables.Perm.GetInt(ParticipationsVar, 0) + 1);
			character.ServerMessage(LF("World Boss participation recorded. Visit the World Boss Hunter in Klaipeda for your {0} Golden Coins.", CoinsPerParticipation));
		}

		Send.ZC_NORMAL.WorldMessage(1, LF("[World Boss] {0} has been defeated in {1}!", boss.Data.Name, boss.Map?.Data.Name));
	}

	private async Task HunterDialog(Dialog dialog)
	{
		var character = dialog.Player;
		var vars = character.Variables.Perm;
		dialog.SetTitle(L("World Boss Hunter"));

		if (!vars.GetBool(RegisteredVar))
		{
			var accept = await dialog.Select(LF("Powerful bosses roam the fields at {0}. Help bring one down and I'll pay you {1} Golden Coins for each hunt you joined.", string.Join(", ", SpawnHours.Select(a => $"{a:00}:00")), CoinsPerParticipation),
				Option(L("Register as a World Boss Hunter"), "accept"),
				Option(L("Leave"), "exit"));

			if (accept == "exit")
				return;

			vars.SetBool(RegisteredVar, true);
		}

		while (true)
		{
			var participations = vars.GetInt(ParticipationsVar, 0);
			var selection = await dialog.Select(LF("Unclaimed participations: {0}", participations),
				Option(L("Claim Golden Coins"), "claim"),
				Option(L("Where is the boss?"), "boss"),
				Option(L("Leave"), "exit"));

			switch (selection)
			{
				case "exit":
					return;

				case "claim":
					if (participations == 0)
					{
						await dialog.Msg(L("You haven't joined a hunt since your last visit."));
						break;
					}

					vars.SetInt(ParticipationsVar, 0);
					character.AddItem(GoldenCoinItemId, participations * CoinsPerParticipation);
					await dialog.Msg(LF("Here are your {0} Golden Coins.", participations * CoinsPerParticipation));
					break;

				case "boss":
					var boss = _boss;
					if (boss == null || boss.IsDead)
						await dialog.Msg(L("No world boss is roaming right now."));
					else
						await dialog.Msg(LF("{0} was last seen in {1}.", boss.Data.Name, boss.Map?.Data.Name));
					break;
			}
		}
	}
}
