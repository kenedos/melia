//--- Melia Script ----------------------------------------------------------
// Khonot Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Khonot Forest.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken421NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(157042, "", "f_bracken_42_1", 782.5125, 558.6927, -747.0397, 0, "f_bracken_42_1_elt", 2, 1);
		AddTrackNPC(157043, "", "f_bracken_42_1", 1067.31, 557.06, -425.56, 0, "f_bracken_42_1_elt2", 2, 1);

	}
}

//-----------------------------------------------------------------------------
// ENVIRONMENTAL DANGER ZONES
//-----------------------------------------------------------------------------
// Canopy ambush spots. When a player passes through one of these zones, they
// have a chance to either be struck by a falling-frond toxin or be ambushed
// by Blue Gosarus dropping from the canopy. Each zone fires at most once per
// character.
//-----------------------------------------------------------------------------

public class FBracken421DangerZonesScript : GeneralScript
{
	private const int PoisonDamage = 500;
	private const int PoisonDuration = 10;
	private const int TriggerChance = 30;

	protected override void Load()
	{
		var zones = new[]
		{
			new { x = -641.559, z = -120.55955 },
			new { x = 258.43613, z = 438.93112 },
			new { x = -640.4996, z = 586.57043 }
		};

		for (int i = 0; i < zones.Length; i++)
		{
			var zone = zones[i];
			var zoneIndex = i;

			AddAreaTrigger("f_bracken_42_1", zone.x, zone.z, 50, async (args) =>
			{
				if (args.Initiator is not Character character)
					return;

				if (character.IsDead)
					return;

				var poisonKey = $"Laima.Environment.f_bracken_42_1.Zone{zoneIndex}.Poisoned";
				var spawnKey = $"Laima.Environment.f_bracken_42_1.Zone{zoneIndex}.Spawned";

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

						character.ServerMessage(L("{#66FF66}A toxin from the canopy fronds seeps into your skin!{/}"));
					}
					else
					{
						character.Variables.Perm.Set(spawnKey, true);

						var spawnCount = RandomProvider.Get().Next(2, 4);
						if (SpawnTempMonsters(character, MonsterId.Gosaru_Blue, spawnCount, 70, TimeSpan.FromMinutes(1)))
						{
							character.ServerMessage(L("{#FF6666}Blue Gosarus drop from the canopy!{/}"));
						}
					}
				}
			});
		}
	}
}
