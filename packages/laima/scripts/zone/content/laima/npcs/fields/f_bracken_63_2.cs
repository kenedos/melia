//--- Melia Script ----------------------------------------------------------
// Knidos Jungle
//--- Description -----------------------------------------------------------
// NPCs found in and around Knidos Jungle.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken632NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(304, 40120, "Statue of Goddess Vakarine", "f_bracken_63_2", 196.9207, 284.1552, 998.8207, 90, "WARP_F_BRACKEN_63_2", "STOUP_CAMP", "STOUP_CAMP");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153112, "", "f_bracken_63_2", -47.54993, 253.4295, -541.7952, 0, "f_bracken_63_2_elt", 2, 1);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(493, 147392, "Lv1 Treasure Chest", "f_bracken_63_2", 589, 84, -1976, 0, "TREASUREBOX_LV_F_BRACKEN_63_2493", "", "");
	}
}

//-----------------------------------------------------------------------------
// ENVIRONMENTAL DANGER ZONES
//-----------------------------------------------------------------------------
// These zones trigger either poison or monster spawns when players pass through
//-----------------------------------------------------------------------------

public class FBracken632DangerZonesScript : GeneralScript
{
	private const int PoisonDamage = 500;
	private const int PoisonDuration = 10;
	private const int TriggerChance = 30;

	protected override void Load()
	{
		var zones = new[]
		{
			new { x = -126, z = 1287 },
			new { x = 332, z = 1454 },
			new { x = 1363, z = 655 },
			new { x = 950, z = -410 },
			new { x = -34, z = -1047 },
		};

		for (int i = 0; i < zones.Length; i++)
		{
			var zone = zones[i];
			var zoneIndex = i;

			AddAreaTrigger("f_bracken_63_2", zone.x, zone.z, 50, async (args) =>
			{
				if (args.Initiator is not Character character)
					return;

				if (character.IsDead)
					return;

				var poisonKey = $"Laima.Environment.f_bracken_63_2.Zone{zoneIndex}.Poisoned";
				var spawnKey = $"Laima.Environment.f_bracken_63_2.Zone{zoneIndex}.Spawned";

				var alreadyPoisoned = character.Variables.Perm.GetBool(poisonKey, false);
				var alreadySpawned = character.Variables.Perm.GetBool(spawnKey, false);

				if (alreadyPoisoned || alreadySpawned)
					return;

				if (RandomProvider.Get().Next(100) < TriggerChance)
				{
					var eventType = RandomProvider.Get().Next(2);

					if (eventType == 0)
					{
						character.Variables.Perm.Set(poisonKey, true);

						character.StartBuff(
							BuffId.UC_poison,
							1,
							PoisonDamage,
							TimeSpan.FromSeconds(PoisonDuration),
							character
						);

						character.ServerMessage(L("{#66FF66}You feel a toxic miasma seeping into your body!{/}"));
					}
					else
					{
						character.Variables.Perm.Set(spawnKey, true);

						var spawnCount = RandomProvider.Get().Next(2, 4);
						var monster = RandomProvider.Get().Next(2) == 0 ? MonsterId.Loktanun : MonsterId.Ponpon;
						if (SpawnTempMonsters(character, monster, spawnCount, 70, TimeSpan.FromMinutes(1)))
						{
							character.ServerMessage(L("{#FF6666}Predators erupt from the poisonous undergrowth!{/}"));
						}
					}
				}
			});
		}
	}
}
