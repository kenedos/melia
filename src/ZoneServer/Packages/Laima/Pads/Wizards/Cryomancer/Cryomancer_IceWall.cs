using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Handlers.Cryomancer;
using Melia.Zone.Skills.Handlers.Pyromancer;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cryomancer_IceWall)]
	public class Cryomancer_IceWallOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float BaseFreezeChance = 20f;
		private const float FreezeChancePerLevel = 4f;
		private const int FreezeDurationMilliseconds = 3000;
		private const float ObstacleSize = 20f;
		private const float FreezeRange = 40f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			// This skill creates obstacles so we don't want players blocking
			// warps or NPCs.
			if (creator.Map.IsCity)
				return;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(FreezeRange);
			pad.SetUpdateInterval(1500);
			var value = 15000;
			if (creator.IsAbilityActive(AbilityId.Cryomancer22))
				value = value + 10000;

			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(value);
			var iceWall = (Mob)PadCreateMonster(pad, "pcskill_icewall", pad.Position, 90f, 0, value, "!SCR_SUMMON_ICEWALL#1", "None", 1, false, "TEST_ICEWALL", "INIT_ICEWALL_MONSTER", true, "SET_CASTER_ICEWALL");
			if (iceWall == null)
				return;

			// Set max HP
			iceWall.Properties.SetFloat(PropertyName.Lv, creator.Level);
			iceWall.Properties.SetFloat(PropertyName.HPCount, skill.Level * 10);
			iceWall.MonsterType = RelationType.Friendly;

			iceWall.Properties.InvalidateAll();
			iceWall.AddEffect(new ReviveEffect());
			iceWall.Faction = FactionType.IceWall;
			iceWall.Direction = creator.Direction;

			// Update current HP/SP
			iceWall.Properties.SetFloat(PropertyName.HP, iceWall.Properties.GetFloat(PropertyName.MHP));
			iceWall.Properties.SetFloat(PropertyName.SP, iceWall.Properties.GetFloat(PropertyName.MSP));

			// Creates Obstacle
			var obstacleSize = ObstacleSize;
			var obstacleShape = new Circle(pad.Position, obstacleSize);
			var obstacle = new DynamicObstacle(pad.Position, obstacleShape, "IceWall");
			pad.Variables.Set("IceWallObstacle", obstacle);
			pad.Map.AddObstacle(obstacle);
			PadCreateObstacle(pad, obstacleSize, obstacleSize);

			// Adds monster to map
			pad.Map.AddMonster(iceWall);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			// Removes obstacle
			if (pad.Variables.TryGet<DynamicObstacle>("IceWallObstacle", out var obstacle))
			{
				pad.Map.RemoveObstacle(obstacle);
			}

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (initiator.Faction == FactionType.IceWall)
				return;

			if (!creator.IsEnemy(initiator))
				return;

			var buffId = BuffId.Cryomancer_Freeze;
			var freezeDuration = FreezeDurationMilliseconds;
			var freezeChance = BaseFreezeChance + (skill.Level * FreezeChancePerLevel);

			if (creator.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, 0, buffId, 1, 0, (int)freezeDuration, 0, (int)freezeChance, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var buffId = BuffId.Cryomancer_Freeze;
			var freezeDuration = FreezeDurationMilliseconds;
			var freezeChance = BaseFreezeChance + (skill.Level * FreezeChancePerLevel);

			if (creator.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			foreach (var target in pad.Trigger.GetAttackableEntities(creator))
			{
				if (!creator.IsEnemy(target))
					continue;

				if (target.Faction == FactionType.IceWall)
					continue;

				PadTargetBuff(pad, target, RelationType.Enemy, 0, 0, buffId, 1, 0, (int)freezeDuration, 0, (int)freezeChance, false);
			}

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f, true, 1, target => target.Faction != FactionType.IceWall);
		}
	}
}
