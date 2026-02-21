using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cryomancer_FrostPillar)]
	public class Cryomancer_FrostPillarOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float PadRange = 60f;
		private const int BasePadLifeTimeMilliseconds = 8000;
		private const int PadLifeTimePerLevelMilliseconds = 1100;
		private const int UpdateInterval = 500;
		private const int FreezeDurationMilliSeconds = 4000;
		private const int FreezeChance = 100;

		/// <summary>
		/// Handles the creation of the Frost Pillar pad.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(PadRange);
			pad.SetUpdateInterval(UpdateInterval);
			var lifeTime = BasePadLifeTimeMilliseconds + (PadLifeTimePerLevelMilliseconds * pad.Skill.Level);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(lifeTime);
			pad.Trigger.MaxActorCount = 10;

			this.CreatePillarMonster(pad);
		}

		/// <summary>
		/// Handles the destruction of the Frost Pillar pad.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Handles an entity entering the Frost Pillar pad area.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			var buffTime = FreezeDurationMilliSeconds;
			this.ApplyFrostPillarBuff(pad, creator, initiator, buffTime);
		}

		/// <summary>
		/// Handles an entity leaving the Frost Pillar pad area.
		/// </summary>
		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			PadTargetBuffRemoveMonster(pad, initiator, RelationType.Enemy, 0, 0, BuffId.Gust_Debuff);
		}

		/// <summary>
		/// Handles periodic updates of the Frost Pillar pad.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			this.DealDamageToEnemies(pad);
			this.RefreshFrostPillarBuff(pad, creator, skill);
			this.ApplyFrostPillarDebuff(pad);
		}

		private void CreatePillarMonster(Pad pad)
		{
			var lifeTime = BasePadLifeTimeMilliseconds + (PadLifeTimePerLevelMilliseconds * pad.Skill.Level);
			var monster = PadCreateMonster(pad, "attract_pillar", pad.Position, 0f, 0, lifeTime, "", "None", 1, true, "None", "None", false, "SET_PVE_NODAMAGE");
			var mob = monster as Mob;
			mob.MonsterType = RelationType.Friendly;
			mob.Faction = FactionType.Law;
			mob.StartBuff(BuffId.Invincible);
		}

		private void ApplyFrostPillarBuff(Pad pad, ICombatEntity creator, ICombatEntity target, float duration)
		{
			var freezeChance = FreezeChance;

			if (creator.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			PadTargetBuffAfterBuffCheck(pad, target, RelationType.Enemy, 0, 0, BuffId.Cryomancer_Freeze, BuffId.Cryomancer_Freeze, 1, 0, (int)duration, 1, freezeChance, false);
		}

		private void DealDamageToEnemies(Pad pad)
		{
			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
		}

		private void RefreshFrostPillarBuff(Pad pad, ICombatEntity creator, Skill skill)
		{
			var buffTime = FreezeDurationMilliSeconds;
			var freezeChance = FreezeChance;

			if (creator.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			PadBuffCheckBuffEnemy(pad, RelationType.Enemy, 0, 0, BuffId.Cryomancer_Freeze, BuffId.Cryomancer_Freeze, 1, 0, buffTime, 1, freezeChance);
		}

		private void ApplyFrostPillarDebuff(Pad pad)
		{
			PadBuffEnemyMonster(pad, RelationType.Enemy, 0, 0, BuffId.FrostPillar_Debuff, 1, 0, 1, 1, FreezeChance);
		}
	}
}
