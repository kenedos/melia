//--- Melia Script ----------------------------------------------------------
// Dadan Jungle
//--- Description -----------------------------------------------------------
// NPCs found in and around Dadan Jungle.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken633NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153095, "", "f_bracken_63_3", -252.8264, 189.2868, -89.98244, 111, "f_bracken_63_3_elt", 2, 1);


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(509, 147392, "Lv1 Treasure Chest", "f_bracken_63_3", -662, 1003, -919, 0, "TREASUREBOX_LV_F_BRACKEN_63_3509", "", "");
	}
}

//-----------------------------------------------------------------------------
// ENVIRONMENTAL DANGER ZONES
//-----------------------------------------------------------------------------
// These zones poison the player AND spawn predators when they pass through
//-----------------------------------------------------------------------------

public class FBracken633DangerZonesScript : GeneralScript
{
	private const int PoisonDamage = 250;
	private const int PoisonDuration = 16;
	private const int TriggerChance = 30;

	protected override void Load()
	{
		var zones = new[]
		{
			new { x = -135, z = -727 },
			new { x = -948, z = 299 },
			new { x = -9, z = 628 },
			new { x = 862, z = 33 },
			new { x = 996, z = -498 },
			new { x = -956, z = -41 },
		};

		for (int i = 0; i < zones.Length; i++)
		{
			var zone = zones[i];
			var zoneIndex = i;

			AddAreaTrigger("f_bracken_63_3", zone.x, zone.z, 50, async (args) =>
			{
				if (args.Initiator is not Character character)
					return;

				if (character.IsDead)
					return;

				var triggeredKey = $"Laima.Environment.f_bracken_63_3.Zone{zoneIndex}.Triggered";

				if (character.Variables.Perm.GetBool(triggeredKey, false))
					return;

				if (RandomProvider.Get().Next(100) < TriggerChance)
				{
					character.Variables.Perm.Set(triggeredKey, true);

					character.StartBuff(
						BuffId.UC_poison,
						1,
						PoisonDamage,
						TimeSpan.FromSeconds(PoisonDuration),
						character
					);

					var spawnCount = RandomProvider.Get().Next(2, 4);
					var monster = RandomProvider.Get().Next(2) == 0 ? MonsterId.Gosaru : MonsterId.Raffly;
					SpawnTempMonsters(character, monster, spawnCount, 70, TimeSpan.FromMinutes(1));

					character.ServerMessage(L("{#66FF66}A choking miasma rises - and predators lunge from the bushes!{/}"));
				}
			});
		}
	}
}
